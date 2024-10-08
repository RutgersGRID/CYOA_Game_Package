using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;

public class JournalSheetReader : MonoBehaviour
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
    private string sheetKey = "?key=AIzaSyDxlgY5nx2_JX89Grs3KZ7cnxlpRO2Nedg";
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

    private JournalSO CreateJournalSO(int id, string journalTitle, string journalEntry, Sprite doodle, string reflectionQuestion)
    {
        JournalSO journal = ScriptableObject.CreateInstance<JournalSO>();
        journal.IDs = id;
        journal.journalTitles = journalTitle;
        journal.journalEntrys = journalEntry;
        journal.doodles = doodle;
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
            // Handle the error accordingly.
        }
        else
        {
            var parsedJson = JSON.Parse(www.downloadHandler.text);
            var valuesArray = parsedJson["values"].AsArray;
            for(int i = 1; i < valuesArray.Count; i++)
            {
                var item = valuesArray[i];
                var id = int.Parse(item[0].Value);
                var journalTitle = item[1].Value;
                var journalEntry = item[2].Value;
                var doodle = Resources.Load<Sprite>(ResourcesLoadP + item[3].Value);
                var reflectionQuestion = item[4].Value;
                
                journals.Add(CreateJournalSO(id, journalTitle, journalEntry, doodle, reflectionQuestion));
            }
        }
        if (journals.Count == 0)
        {
            Debug.LogWarning("FROM JournalSheetReader");
            Debug.LogWarning("The journals list was not populated. Please check the source or the structure of the Google Sheet.");
        }
        else
        {
            Debug.Log($"FROM JournalSheetReader");
            Debug.Log($"Successfully populated journals list with {journals.Count} entries.");
        }
        onDataLoaded?.Invoke();
    }
}