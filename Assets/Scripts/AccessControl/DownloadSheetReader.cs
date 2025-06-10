using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;
public class DownloadSheetReader : MonoBehaviour
{
    [System.Serializable]
    public class DownloadSO : ScriptableObject
    {
        public string assetTypes;
        public string assetNames;
        public string assetIDs;

    }
    public List<DownloadSO> downloads = new List<DownloadSO>();
    //Pointer to Alex's sheets
    private string sheetBaseUrl = "https://sheets.googleapis.com/v4/spreadsheets/";
    private string sheetKey = "?key=AIzaSyDxlgY5nx2_JX89Grs3KZ7cnxlpRO2Nedg";
    private string sheetId;
    private string sheetUrl;
    public delegate void OnDataLoaded();
    public event OnDataLoaded onDataLoaded;
    void Start()
    {

        sheetId = PlayerPrefs.GetString("SheetId");
        sheetUrl = sheetBaseUrl + sheetId + "/values/Downloads" + sheetKey;
        StartCoroutine(ObtainSheetData());
    }
    private DownloadSO CreateDownloadSO(string assetType, string assetName, string assetID)
    {
        DownloadSO download = ScriptableObject.CreateInstance<DownloadSO>();
        download.assetTypes = assetType;
        download.assetNames = assetName;
        download.assetIDs = assetID;

        return download;
    }
    public IEnumerator ObtainSheetData()
    {
        UnityWebRequest www = UnityWebRequest.Get(sheetUrl);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("Error: " + www.error);
            // Handle the error accordingly.
        }
        else
        {
            var parsedJson = JSON.Parse(www.downloadHandler.text);
            var valuesArray = parsedJson["values"].AsArray;
            for(int i = 1; i < valuesArray.Count; i++)
            {
                var item = valuesArray[i];
                var assetType = item[0].Value;
                var assetName = item[1].Value;
                var assetID = item[2].Value;
                
                downloads.Add(CreateDownloadSO(assetType, assetName, assetID));
            }
        }
        if (downloads.Count == 0)
        {
            Debug.LogWarning("FROM DownloadSO");
            Debug.LogWarning("The downloads list was not populated. Please check the source or the structure of the Google Sheet.");
        }
        else
        {
            Debug.Log($"FROM DownloadSO");
            Debug.Log($"Successfully populated downloads list with {downloads.Count} entries.");
        }
        onDataLoaded?.Invoke();
    }
}
