using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.IO;
using System.Threading;

public class AssetManager : MonoBehaviour
{
    public static AssetManager Instance { get; private set; }
    private Dictionary<string, Sprite> spriteAssets = new Dictionary<string, Sprite>();

    private string[] Scopes = { DriveService.Scope.DriveReadonly };
    private string ApplicationName = "Drive API .NET Quickstart";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
            DownloadAssets();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void DownloadAssets()
    {
        UserCredential credential;

        // Use Application.dataPath to create a relative path
        string credentialsPath = Path.Combine(Application.dataPath, "credentials.json");

        using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
        {
            string credPath = Path.Combine(Application.dataPath, "token.json");
            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.Load(stream).Secrets,
                Scopes,
                "user",
                CancellationToken.None,
                new FileDataStore(credPath, true)).Result;
        }

        // Create Drive API service.
        var service = new DriveService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = ApplicationName,
        });

        // Define parameters of request.
        FilesResource.ListRequest listRequest = service.Files.List();
        listRequest.PageSize = 10;
        listRequest.Fields = "nextPageToken, files(id, name)";

        // List files.
        var request = service.Files.List();
        request.Q = "'1TuvKgztjDlYnHxCzuFAWxb9c1H_Nizal' in parents";
        var result = request.Execute();
        
        foreach (var file in result.Files)
        {
            StartCoroutine(DownloadImage(file.Id, file.Name));
        }
    }

    public IEnumerator DownloadImage(string fileId, string fileName)
    {
        string url = $"https://www.googleapis.com/drive/v3/files/{fileId}?alt=media";
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + www.error);
        }
        else
        {
            Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            spriteAssets[fileName] = sprite;
            Debug.Log($"Downloaded and added asset: {fileName}");
        }
    }

    public Sprite GetSprite(string assetName)
    {
        if (spriteAssets.ContainsKey(assetName))
        {
            return spriteAssets[assetName];
        }
        return null;
    }
}
