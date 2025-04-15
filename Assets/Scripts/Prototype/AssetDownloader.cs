using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class AssetDownloader : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Reference to the DownloadSheetReader component")]
    public DownloadSheetReader downloadSheetReader;

    [Header("Optional UI References")]
    [Tooltip("Optional: Image component to display the downloaded texture (for visual assets)")]
    public RawImage previewImage;
    
    [Tooltip("Optional: Text component to show download status")]
    public Text statusText;
    
    [Tooltip("Optional: Slider to show download progress")]
    public Slider progressSlider;

    // Flag to track if we're currently downloading
    private bool isDownloading = false;
    
    // Download queue
    private Queue<DownloadSheetReader.DownloadSO> downloadQueue = new Queue<DownloadSheetReader.DownloadSO>();
    
    // Current asset being downloaded
    private DownloadSheetReader.DownloadSO currentDownload;
    
    // Map of asset types to their resource paths
    private Dictionary<string, string> assetTypePaths = new Dictionary<string, string>
    {
        { "Animation", "AnimationImages" },
        { "Background", "Backgrounds" },
        { "Character", "Characters" },
        { "Prop", "Props" },
        { "Sound", "Sounds" }
    };

    private void Awake()
    {
        // If not assigned in the inspector, try to find the component on this GameObject
        if (downloadSheetReader == null)
        {
            downloadSheetReader = GetComponent<DownloadSheetReader>();
            
            if (downloadSheetReader == null)
            {
                Debug.LogError("DownloadSheetReader reference is missing. Please assign it in the inspector or add the component to this GameObject.");
            }
        }
    }

    private void Start()
    {
        // Register to the event when data is loaded from the sheet
        if (downloadSheetReader != null)
        {
            downloadSheetReader.onDataLoaded += OnSheetDataLoaded;
        }
        
        UpdateUI(0, "Waiting for sheet data to load...");
    }

    private void OnSheetDataLoaded()
    {
        // Add all assets to the download queue
        foreach (var downloadSO in downloadSheetReader.downloads)
        {
            downloadQueue.Enqueue(downloadSO);
        }
        
        SetStatus($"Found {downloadQueue.Count} assets to download.");
        
        // Start downloading if we have items
        if (downloadQueue.Count > 0 && !isDownloading)
        {
            StartCoroutine(ProcessDownloadQueue());
        }
    }

    private IEnumerator ProcessDownloadQueue()
    {
        isDownloading = true;
        int totalAssets = downloadQueue.Count;
        int currentAssetIndex = 0;
        
        while (downloadQueue.Count > 0)
        {
            currentAssetIndex++;
            currentDownload = downloadQueue.Dequeue();
            
            // Update progress
            float progress = (float)currentAssetIndex / totalAssets;
            UpdateUI(progress, $"Downloading {currentAssetIndex}/{totalAssets}: {currentDownload.assetNames}");
            
            // Start downloading the current asset
            yield return StartCoroutine(DownloadAsset(currentDownload));
            
            // Small delay between downloads to prevent rate limiting
            yield return new WaitForSeconds(0.5f);
        }
        
        isDownloading = false;
        UpdateUI(1, "All downloads completed!");
    }

    private IEnumerator DownloadAsset(DownloadSheetReader.DownloadSO assetInfo)
    {
        // Format direct download URL for Google Drive
        string downloadUrl = $"https://drive.google.com/uc?export=download&id={assetInfo.assetIDs}";
        Debug.Log($"Downloading {assetInfo.assetNames} ({assetInfo.assetTypes}) from URL: {downloadUrl}");
        
        SetStatus($"Downloading {assetInfo.assetNames}...");

        // Create the download request
        using (UnityWebRequest www = UnityWebRequest.Get(downloadUrl))
        {
            // Send the request and wait for it to complete
            yield return www.SendWebRequest();

            // Check for errors
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error downloading {assetInfo.assetNames}: {www.error}");
                SetStatus($"Error downloading {assetInfo.assetNames}: {www.error}");
                yield break;
            }

            // Store the downloaded binary content
            byte[] downloadedContent = www.downloadHandler.data;
            Debug.Log($"Successfully downloaded {assetInfo.assetNames}: {downloadedContent.Length} bytes");
            
            // Preview the image if it's a visual asset and we have a preview component
            if (previewImage != null && 
                (assetInfo.assetTypes == "Background" || 
                 assetInfo.assetTypes == "Character" || 
                 assetInfo.assetTypes == "Prop" ||
                 assetInfo.assetTypes == "Animation"))
            {
                Texture2D texture = new Texture2D(2, 2);
                if (texture.LoadImage(downloadedContent))
                {
                    previewImage.texture = texture;
                }
            }

#if UNITY_EDITOR
            // For Editor only: Save to Resources folder with proper asset database handling
            SaveToResourcesInEditor(assetInfo, downloadedContent);
#else
            // For builds: Save to persistent data path
            SaveToPersistentDataPath(assetInfo, downloadedContent);
#endif
        }
    }

#if UNITY_EDITOR
    private void SaveToResourcesInEditor(DownloadSheetReader.DownloadSO assetInfo, byte[] downloadedContent)
    {
        try
        {
            // Get the appropriate subfolder based on asset type
            string subFolder = GetSubfolderForAssetType(assetInfo.assetTypes);
            if (string.IsNullOrEmpty(subFolder))
            {
                Debug.LogError($"Unknown asset type: {assetInfo.assetTypes}");
                SetStatus($"Error: Unknown asset type '{assetInfo.assetTypes}'");
                return;
            }
            
            // Construct the full path
            string resourcesPath = Path.Combine(Application.dataPath, "Resources");
            string subFolderPath = Path.Combine(resourcesPath, subFolder);
            
            // Create directories if they don't exist
            if (!Directory.Exists(resourcesPath))
            {
                Directory.CreateDirectory(resourcesPath);
            }
            
            if (!Directory.Exists(subFolderPath))
            {
                Directory.CreateDirectory(subFolderPath);
            }
            
            // Determine file extension based on asset type
            string fileExtension = DetermineFileExtension(assetInfo.assetTypes, assetInfo.assetNames);
            
            // Full path to save the file
            string fileName = $"{assetInfo.assetNames}{fileExtension}";
            string savePath = Path.Combine(subFolderPath, fileName);
            
            // Write the binary data to the file
            File.WriteAllBytes(savePath, downloadedContent);
            Debug.Log($"{assetInfo.assetNames} saved successfully to: {savePath}");
            SetStatus($"Saved {assetInfo.assetNames} to: {subFolder}");

            // Refresh the asset database to recognize the new file
            // This needs to be done on the main thread
            EditorApplication.delayCall += () => {
                AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                Debug.Log("Asset database refreshed");
            };
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error saving {assetInfo.assetNames}: {e.Message}");
            SetStatus($"Error saving {assetInfo.assetNames}: {e.Message}");
        }
    }
#endif

    private void SaveToPersistentDataPath(DownloadSheetReader.DownloadSO assetInfo, byte[] downloadedContent)
    {
        try
        {
            // Determine file extension based on asset type
            string fileExtension = DetermineFileExtension(assetInfo.assetTypes, assetInfo.assetNames);
            
            // For builds, we'll mimic the Resources structure in persistentDataPath
            string subFolder = GetSubfolderForAssetType(assetInfo.assetTypes);
            string directoryPath = Path.Combine(Application.persistentDataPath, subFolder);
            
            // Create directory if it doesn't exist
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            
            // Full path to save the file
            string fileName = $"{assetInfo.assetNames}{fileExtension}";
            string savePath = Path.Combine(directoryPath, fileName);
            
            // Write the binary data to the file
            File.WriteAllBytes(savePath, downloadedContent);
            Debug.Log($"{assetInfo.assetNames} saved successfully to: {savePath}");
            SetStatus($"Saved {assetInfo.assetNames} to: {savePath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error saving {assetInfo.assetNames}: {e.Message}");
            SetStatus($"Error saving {assetInfo.assetNames}: {e.Message}");
        }
    }

    // Helper method to get the appropriate subfolder based on asset type
    private string GetSubfolderForAssetType(string assetType)
    {
        if (assetTypePaths.TryGetValue(assetType, out string path))
        {
            return path;
        }
        
        return string.Empty;
    }
    
    // Helper method to determine file extension based on asset type and name
    private string DetermineFileExtension(string assetType, string assetName)
    {
        // Check if the asset name already has an extension
        if (Path.HasExtension(assetName))
        {
            return string.Empty; // Return empty if the name already includes extension
        }
        
        // Default extensions by asset type
        switch (assetType)
        {
            case "Sound":
                return ".mp3"; // Default for sounds, adjust as needed
            case "Animation":
            case "Background":
            case "Character":
            case "Prop":
                return ".png"; // Default for image assets
            default:
                return ".asset"; // Generic extension
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
    
    // Helper method to update UI elements
    private void UpdateUI(float progress, string message)
    {
        // Update progress slider
        if (progressSlider != null)
        {
            progressSlider.value = progress;
        }
        
        // Update status text
        SetStatus(message);
    }
    
    // Public method to manually start downloading if needed
    public void StartDownloading()
    {
        if (!isDownloading && downloadQueue.Count > 0)
        {
            StartCoroutine(ProcessDownloadQueue());
        }
        else if (downloadQueue.Count == 0 && downloadSheetReader.downloads.Count > 0)
        {
            // Re-populate the queue and start downloading
            foreach (var downloadSO in downloadSheetReader.downloads)
            {
                downloadQueue.Enqueue(downloadSO);
            }
            
            StartCoroutine(ProcessDownloadQueue());
        }
    }
}