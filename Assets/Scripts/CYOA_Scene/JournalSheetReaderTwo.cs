using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;

public class JournalSheetReaderTwo : MonoBehaviour
{
    [System.Serializable]
    public class JournalSO : ScriptableObject
    {
        public int IDs;
        public string journalTitles;
        public string journalEntrys;
        public Sprite doodles;
        public string reflectionQuestions;
    }

    public List<JournalSO> journals = new List<JournalSO>();
    private string ResourcesLoadP = "Props/";
    private string sheetBaseUrl = "https://sheets.googleapis.com/v4/spreadsheets/";
    private string sheetKey = "?key=AIzaSyDa6TYGcPDdOCI5V3Rq7YJlo9d-FCugzXQ"; // Replace with your Google Sheets API key
    private string sheetId;
    private string sheetUrl;
    public delegate void OnDataLoaded();
    public event OnDataLoaded onDataLoaded;

    void Start()
    {
        sheetId = PlayerPrefs.GetString("SheetId");
        sheetUrl = sheetBaseUrl + sheetId + "/values/JournalSheet" + sheetKey;
        StartCoroutine(ObtainSheetData());
    }

    private JournalSO CreateJournalSO(int id, string journalTitle, string journalEntry, string doodleFileName, string reflectionQuestion)
    {
        JournalSO journal = ScriptableObject.CreateInstance<JournalSO>();
        journal.IDs = id;
        journal.journalTitles = journalTitle;
        journal.journalEntrys = journalEntry;
        journal.doodles = AssetManager.Instance.GetSprite(doodleFileName);
        journal.reflectionQuestions = reflectionQuestion;

        return journal;
    }

    public IEnumerator ObtainSheetData()
    {
        Debug.Log("Sheet Data loaded by Journal Sheet Reader Two");
        UnityWebRequest www = UnityWebRequest.Get(sheetUrl);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("Error: " + www.error);
        }
        else
        {
            var parsedJson = JSON.Parse(www.downloadHandler.text);
            var valuesArray = parsedJson["values"].AsArray;
            for (int i = 1; i < valuesArray.Count; i++)
            {
                var item = valuesArray[i];
                int id = 0;
                int.TryParse(item[0].Value, out id);
                var journalTitle = item[1].Value;
                var journalEntry = item[2].Value;
                var doodleFileName = item[3].Value;
                var reflectionQuestion = item[4].Value;

                journals.Add(CreateJournalSO(id, journalTitle, journalEntry, doodleFileName, reflectionQuestion));
            }
        }
        if (journals.Count == 0)
        {
            Debug.LogWarning("FROM JournalSheetReaderTwo");
            Debug.LogWarning("The journals list was not populated. Please check the source or the structure of the Google Sheet.");
        }
        else
        {
            Debug.Log($"FROM JournalSheetReaderTwo");
            Debug.Log($"Successfully populated journals list with {journals.Count} entries.");
        }
        onDataLoaded?.Invoke();
    }
}
