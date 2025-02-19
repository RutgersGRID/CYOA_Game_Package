using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class CloudSaveItem
{
    public string Key { get; set; }
    public string Value { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastSaved { get; set; }
    public string WriteLock { get; set; }
    public long Size { get; set; }
}

public class CloudSaveExporterWindow : EditorWindow
{
    private string projectId = "a85199a0-62b3-4454-bbb3-b3bcee08c7a9"; // Your project ID
    private string environmentId = "a3625a8a-1418-4864-be82-bc54343353be";
    private string apiKey = "18afb795-b99a-4248-a0a0-0f2dccae085b";
    private bool isExporting = false;
    private Vector2 scrollPosition;
    private string exportPath = "";
    private string statusMessage = "";

    [MenuItem("Tools/Cloud Save Exporter")]
    public static void ShowWindow()
    {
        var window = GetWindow<CloudSaveExporterWindow>("Cloud Save Exporter");
        window.minSize = new Vector2(400, 200);
    }

    private void OnGUI()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        GUILayout.Label("Cloud Save Data Exporter", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        // Input fields with tooltips
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel(new GUIContent("Project ID", "The Unity Project ID from your dashboard"));
        projectId = EditorGUILayout.TextField(projectId);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel(new GUIContent("Environment ID", "The Environment ID from your Cloud Save service"));
        environmentId = EditorGUILayout.TextField(environmentId);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel(new GUIContent("API Key", "The API Key generated from your service account"));
        apiKey = EditorGUILayout.TextField(apiKey);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        // Export path
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Export Path");
        EditorGUILayout.SelectableLabel(exportPath, EditorStyles.textField, GUILayout.Height(EditorGUIUtility.singleLineHeight));
        if (GUILayout.Button("Browse", GUILayout.Width(60)))
        {
            string path = EditorUtility.OpenFolderPanel("Choose Export Location", "", "");
            if (!string.IsNullOrEmpty(path))
            {
                exportPath = path;
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        // Validation check before enabling export
        bool canExport = !isExporting && 
                        !string.IsNullOrEmpty(apiKey) && 
                        !string.IsNullOrEmpty(environmentId) &&
                        !string.IsNullOrEmpty(projectId);

        using (new EditorGUI.DisabledGroupScope(!canExport))
        {
            if (GUILayout.Button("Export Player Data"))
            {
                ExportData();
            }
        }

        if (!string.IsNullOrEmpty(statusMessage))
        {
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox(statusMessage, MessageType.Info);
        }

        EditorGUILayout.EndScrollView();
    }

    private async void ExportData()
    {
        if (string.IsNullOrEmpty(exportPath))
        {
            exportPath = EditorUtility.OpenFolderPanel("Choose Export Location", "", "");
            if (string.IsNullOrEmpty(exportPath))
            {
                return;
            }
        }

        isExporting = true;
        statusMessage = "Starting export...";
        Repaint();

        try
        {
            // Test connection first
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                var testUrl = $"https://services.api.unity.com/player-database/v1/projects/{projectId}/environments/{environmentId}/players?limit=1";
                var response = await client.GetAsync(testUrl);
                
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Connection test failed: {response.StatusCode} - {error}");
                }
            }

            var exporter = new CloudSaveExporter(projectId, environmentId, apiKey);
            await exporter.ExportAllPlayerDataAsync(exportPath);
            statusMessage = "Export completed successfully!";
            EditorUtility.RevealInFinder(exportPath);
        }
        catch (Exception ex)
        {
            statusMessage = $"Export failed: {ex.Message}";
            Debug.LogError($"Detailed error: {ex}");
            
            if (ex.Message.Contains("NotFound"))
            {
                statusMessage += "\n\nPossible solutions:\n" +
                               "1. Verify Project ID and Environment ID are correct\n" +
                               "2. Check if API Key has correct permissions\n" +
                               "3. Ensure Cloud Save service is enabled for this project";
            }
        }
        finally
        {
            isExporting = false;
            Repaint();
        }
    }
}

public class CloudSaveExporter
{
    private readonly string projectId;
    private readonly string environmentId;
    private readonly string apiKey;
    private readonly HttpClient client;
    private const string BaseUrl = "https://services.api.unity.com";

    public CloudSaveExporter(string projectId, string environmentId, string apiKey)
    {
        this.projectId = projectId;
        this.environmentId = environmentId;
        this.apiKey = apiKey;
        
        client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    private async Task<JObject> GetAsync(string endpoint)
    {
        var response = await client.GetAsync($"{BaseUrl}{endpoint}");
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"API request failed: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
        }
        
        var content = await response.Content.ReadAsStringAsync();
        return JObject.Parse(content);
    }

    public async Task ExportAllPlayerDataAsync(string outputDirectory)
    {
        Directory.CreateDirectory(outputDirectory);
        var offset = 0;
        const int limit = 100;
        var totalPlayers = 0;

        while (true)
        {
            var endpoint = $"/player-database/v1/projects/{projectId}/environments/{environmentId}/players";
            var queryString = $"?limit={limit}&offset={offset}";
            
            var response = await GetAsync(endpoint + queryString);
            var results = response["results"] as JArray;
            
            if (results == null || !results.HasValues) break;

            foreach (var player in results)
            {
                var playerId = player["id"].ToString();
                try
                {
                    EditorUtility.DisplayProgressBar("Exporting Player Data", 
                        $"Processing player {playerId}", 
                        (float)totalPlayers / (totalPlayers + 1));

                    // Get player's cloud save data
                    var saveDataEndpoint = $"/cloud-save/v1/projects/{projectId}/environments/{environmentId}/players/{playerId}/items";
                    var saveDataResponse = await GetAsync(saveDataEndpoint);

                    var playerItems = new List<CloudSaveItem>();
                    foreach (var prop in saveDataResponse.Properties())
                    {
                        var item = new CloudSaveItem
                        {
                            Key = prop.Name,
                            Value = prop.Value["value"]?.ToString(),
                            Created = DateTime.Parse(prop.Value["created"]?.ToString() ?? DateTime.UtcNow.ToString()),
                            LastSaved = DateTime.Parse(prop.Value["modified"]?.ToString() ?? DateTime.UtcNow.ToString()),
                            WriteLock = prop.Value["writeLock"]?.ToString(),
                            Size = prop.Value["size"]?.Value<long>() ?? 0
                        };
                        playerItems.Add(item);
                    }

                    // Save individual player data
                    var playerDataPath = Path.Combine(outputDirectory, $"player_{playerId}.json");
                    var json = JsonConvert.SerializeObject(new
                    {
                        PlayerId = playerId,
                        PlayerInfo = player,
                        SaveData = playerItems
                    }, Formatting.Indented);
                    
                    await File.WriteAllTextAsync(playerDataPath, json);
                    totalPlayers++;
                    Debug.Log($"Exported data for player {playerId}");
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error exporting player {playerId}: {ex.Message}");
                }
            }

            offset += limit;
        }

        // Create summary file
        var summary = new
        {
            ExportDate = DateTime.UtcNow,
            TotalPlayersExported = totalPlayers,
            ProjectId = projectId,
            EnvironmentId = environmentId
        };

        await File.WriteAllTextAsync(
            Path.Combine(outputDirectory, "export_summary.json"),
            JsonConvert.SerializeObject(summary, Formatting.Indented)
        );

        EditorUtility.ClearProgressBar();
        Debug.Log($"\nExport complete! Exported data for {totalPlayers} players to {outputDirectory}");
    }
}