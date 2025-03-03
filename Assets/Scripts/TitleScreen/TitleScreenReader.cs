using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Networking;
using SimpleJSON;

public class TitleScreenReader : MonoBehaviour
{
    [System.Serializable]
    public class TitleScreenSO : ScriptableObject
    {
        public string Titles;
        public string Presents;
        public Sprite TitleImages;
    }
    
    public List<TitleScreenSO> titlescreens = new List<TitleScreenSO>();
    private string ResourcesLoadTS = "Backgrounds/";
    private string sheetBaseUrl = "https://sheets.googleapis.com/v4/spreadsheets/";
    private string sheetKey = "?key=AIzaSyDxlgY5nx2_JX89Grs3KZ7cnxlpRO2Nedg";
    [SerializeField] private string defaultSheetId = "1SLm9j993IbtSKpzmVoshhebh7FxJcZOp2a4BU5aId8g"; // Set default in inspector
    private string sheetId;
    private string sheetUrl;
    public delegate void OnDataLoaded();
    public event OnDataLoaded onDataLoaded;
    private bool dataLoaded = false;
    
    void Awake()
    {
        // Clear any existing data before loading
        titlescreens.Clear();
    }
    
    void Start()
    {
        if (!dataLoaded)
        {
            // Try to get sheet ID from PlayerPrefs, fallback to default if not found
            sheetId = PlayerPrefs.GetString("SheetId", defaultSheetId);
            if (string.IsNullOrEmpty(sheetId))
            {
                Debug.LogWarning("Sheet ID not found in PlayerPrefs, using default: " + defaultSheetId);
                sheetId = defaultSheetId;
            }
            
            sheetUrl = sheetBaseUrl + sheetId + "/values/Title" + sheetKey;
            Debug.Log("Fetching data from: " + sheetUrl);
            StartCoroutine(ObtainSheetData());
        }
    }

    private TitleScreenSO CreateTitleScreenSO(string Title, string Present, Sprite TitleImage)
    {
        TitleScreenSO titlescreen = ScriptableObject.CreateInstance<TitleScreenSO>();
        titlescreen.Titles = Title;
        titlescreen.Presents = Present;
        titlescreen.TitleImages = TitleImage;
        return titlescreen;
    }

    private int SafeIntParse(string str, int defaultValue = -1)
    {
        if (string.IsNullOrEmpty(str))
        {
            return defaultValue;
        }

        if (int.TryParse(str, out int result))
        {
            return result;
        }
        else
        {
            Debug.LogWarning($"Failed to parse \"{str}\" as an integer. Using default value: {defaultValue}.");
            return defaultValue;
        }
    }

    private string SafeStringParse(string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return string.Empty;
        }
        return str;
    }

    public IEnumerator ObtainSheetData()
    {
        UnityWebRequest www = UnityWebRequest.Get(sheetUrl);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error fetching Google Sheet: " + www.error);
            Debug.LogError("URL: " + sheetUrl);
        }
        else
        {
            try
            {
                var parsedJson = JSON.Parse(www.downloadHandler.text);
                var valuesArray = parsedJson["values"].AsArray;
                
                Debug.Log($"Retrieved {valuesArray.Count} rows from Google Sheet");
                
                // Skip header row (index 0)
                for (int i = 1; i < valuesArray.Count; i++)
                {
                    var row = valuesArray[i];
                    
                    // Check if we have enough elements in this row
                    if (row.Count < 3)
                    {
                        Debug.LogWarning($"Row {i} doesn't have enough columns (expected 3, got {row.Count})");
                        continue;
                    }
                    
                    var Present = SafeStringParse(row[0]);
                    var Title = SafeStringParse(row[1]);
                    var imageName = SafeStringParse(row[2]);
                    
                    var imagePath = ResourcesLoadTS + imageName;
                    Debug.Log($"Loading sprite from: {imagePath}");
                    
                    var TitleImage = Resources.Load<Sprite>(imagePath);
                    
                    if (TitleImage == null)
                    {
                        Debug.LogError($"Failed to load sprite at path: {imagePath}");
                        continue;
                    }
                    
                    titlescreens.Add(CreateTitleScreenSO(Title, Present, TitleImage));
                    Debug.Log($"Added title screen: {Title}, {Present}, Image: {imageName}");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error parsing Google Sheet data: " + e.Message);
                Debug.LogError("Response content: " + www.downloadHandler.text);
            }
        }

        if (titlescreens.Count == 0)
        {
            Debug.LogWarning("FROM TitleScreenReader: FAILURE");
            Debug.LogWarning("The titlescreens list was not populated. Please check the source or the structure of the Google Sheet: " + sheetUrl);
        }
        else
        {
            Debug.Log($"FROM TitleScreenReader: SUCCESS");
            Debug.Log($"Successfully populated titlescreens list with {titlescreens.Count} entries from " + sheetUrl);
            dataLoaded = true;
        }

        // Notify subscribers that data is loaded
        onDataLoaded?.Invoke();
    }
}