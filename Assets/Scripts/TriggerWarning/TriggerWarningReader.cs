using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Networking;
using SimpleJSON;

public class TriggerWarningReader : MonoBehaviour
{
    [System.Serializable]
    public class TriggerWarningSO : ScriptableObject
    {
        public string TWText;
    }
    
    public List<TriggerWarningSO> triggerWarnings = new List<TriggerWarningSO>();
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
        triggerWarnings.Clear();
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
            
            sheetUrl = sheetBaseUrl + sheetId + "/values/TriggerWarningSheet" + sheetKey;
            Debug.Log("Fetching trigger warning data from: " + sheetUrl);
            StartCoroutine(ObtainSheetData());
        }
    }

    private TriggerWarningSO CreateTriggerWarningSO(string twText)
    {
        TriggerWarningSO triggerWarning = ScriptableObject.CreateInstance<TriggerWarningSO>();
        triggerWarning.TWText = twText;
        return triggerWarning;
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
            Debug.LogError("Error fetching Trigger Warning Google Sheet: " + www.error);
            Debug.LogError("URL: " + sheetUrl);
        }
        else
        {
            try
            {
                var parsedJson = JSON.Parse(www.downloadHandler.text);
                var valuesArray = parsedJson["values"].AsArray;
                
                Debug.Log($"Retrieved {valuesArray.Count} rows from Trigger Warning Google Sheet");
                
                // Skip header row (index 0)
                for (int i = 1; i < valuesArray.Count; i++)
                {
                    var row = valuesArray[i];
                    
                    // Check if we have enough elements in this row
                    if (row.Count < 1)
                    {
                        Debug.LogWarning($"Row {i} doesn't have enough columns (expected 1, got {row.Count})");
                        continue;
                    }
                    
                    var twText = SafeStringParse(row[0]); // TWText is in the first column
                    
                    if (!string.IsNullOrEmpty(twText))
                    {
                        triggerWarnings.Add(CreateTriggerWarningSO(twText));
                        Debug.Log($"Added trigger warning text: {twText.Substring(0, Mathf.Min(50, twText.Length))}...");
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error parsing Trigger Warning Google Sheet data: " + e.Message);
                Debug.LogError("Response content: " + www.downloadHandler.text);
            }
        }

        if (triggerWarnings.Count == 0)
        {
            Debug.LogWarning("FROM TriggerWarningReader: FAILURE");
            Debug.LogWarning("The triggerWarnings list was not populated. Please check the source or the structure of the Google Sheet: " + sheetUrl);
        }
        else
        {
            Debug.Log($"FROM TriggerWarningReader: SUCCESS");
            Debug.Log($"Successfully populated triggerWarnings list with {triggerWarnings.Count} entries from " + sheetUrl);
            dataLoaded = true;
        }

        // Notify subscribers that data is loaded
        onDataLoaded?.Invoke();
    }
}