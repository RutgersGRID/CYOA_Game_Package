using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SimpleImageDownloader : MonoBehaviour
{
    [Header("Google Drive Settings")]
    [Tooltip("Google Drive file ID from the sharing link")]
    public string fileId = "1mDGQNPTGsB-ai4NXgl5W5IMBc4H9tsQE";
    
    [Tooltip("Name to save the file as")]
    public string saveFileName = "Pills.png";

    [Header("Save Settings")]
    [Tooltip("Relative path within Resources folder (e.g. Props)")]
    public string resourcesSubFolder = "Props";

    [Header("Optional UI References")]
    [Tooltip("Optional: Image component to display the downloaded texture")]
    public RawImage displayImage;
    
    [Tooltip("Optional: Text component to show download status")]
    public Text statusText;

    // Binary content of the downloaded file
    public byte[] downloadedContent;

    // Called when the script instance is being loaded
    private void Start()
    {
        StartDownload();
    }

    // Method to initiate the download
    public void StartDownload()
    {
        StartCoroutine(DownloadFile());
    }

    public IEnumerator DownloadFile()
    {
        SetStatus("Starting download...");
        
        // Format direct download URL for Google Drive
        string downloadUrl = $"https://drive.google.com/uc?export=download&id={fileId}";
        Debug.Log($"Downloading from URL: {downloadUrl}");

        // Create the download request
        using (UnityWebRequest www = UnityWebRequest.Get(downloadUrl))
        {
            // Send the request and wait for it to complete
            yield return www.SendWebRequest();

            // Check for errors
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error downloading file: {www.error}");
                SetStatus($"Error: {www.error}");
                yield break;
            }

            // Store the downloaded binary content
            downloadedContent = www.downloadHandler.data;
            Debug.Log($"Successfully downloaded {downloadedContent.Length} bytes");
            SetStatus($"Downloaded {downloadedContent.Length} bytes");

            // Create a texture from the downloaded data
            Texture2D texture = new Texture2D(2, 2);
            if (texture.LoadImage(downloadedContent))
            {
                // Display the texture if we have a UI element
                if (displayImage != null)
                {
                    displayImage.texture = texture;
                    SetStatus("Image loaded and displayed!");
                }
            }

#if UNITY_EDITOR
            // For Editor only: Save to Resources folder with proper asset database handling
            SaveToResourcesInEditor();
#else
            // For builds: Save to persistent data path
            SaveToPersistentDataPath();
#endif
        }
    }

#if UNITY_EDITOR
    private void SaveToResourcesInEditor()
    {
        try
        {
            // Create the Resources/SubFolder directory if it doesn't exist
            string resourcesPath = Path.Combine(Application.dataPath, "Resources");
            if (!Directory.Exists(resourcesPath))
            {
                Directory.CreateDirectory(resourcesPath);
            }

            string subFolderPath = Path.Combine(resourcesPath, resourcesSubFolder);
            if (!Directory.Exists(subFolderPath))
            {
                Directory.CreateDirectory(subFolderPath);
            }

            // Full path to save the file
            string savePath = Path.Combine(subFolderPath, saveFileName);
            
            // Write the binary data to the file
            File.WriteAllBytes(savePath, downloadedContent);
            Debug.Log($"File saved successfully to: {savePath}");
            SetStatus($"Saved to: {savePath}");

            // Refresh the asset database to recognize the new file
            // This needs to be done on the main thread
            EditorApplication.delayCall += () => {
                AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                Debug.Log("Asset database refreshed");
            };
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error saving file: {e.Message}");
            SetStatus($"Error saving: {e.Message}");
        }
    }
#endif

    private void SaveToPersistentDataPath()
    {
        try
        {
            // For builds, save to persistent data path
            string savePath = Path.Combine(Application.persistentDataPath, saveFileName);
            
            // Write the binary data to the file
            File.WriteAllBytes(savePath, downloadedContent);
            Debug.Log($"File saved successfully to: {savePath}");
            SetStatus($"Saved to: {savePath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error saving file: {e.Message}");
            SetStatus($"Error saving: {e.Message}");
        }
    }

    // Helper method to update status text if available
    private void SetStatus(string message)
    {
        Debug.Log(message);
        if (statusText != null)
        {
            statusText.text = message;
        }
    }
}