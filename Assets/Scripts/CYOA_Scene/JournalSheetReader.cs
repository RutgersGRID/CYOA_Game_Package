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
    // public Text display;
    private const string JOURNAL_SHEET_URL = "https://sheets.googleapis.com/v4/spreadsheets/1O88FIl3Z6QkR6Tlteuzb3qDcig6ci1hqNIoUYqqUCx8/values/Journal?key=AIzaSyDxlgY5nx2_JX89Grs3KZ7cnxlpRO2Nedg";

    private void Start()
    {
        StartCoroutine(ObtainSheetData());
    }

    private JournalSO CreateJournalSO(int id, string journaltitle, string journalentry, Sprite doodle, string reflectionquestion)
    {
        JournalSO journal = ScriptableObject.CreateInstance<JournalSO>();
        journal.IDs = id;
        journal.journalTitles = journaltitle;
        journal.journalEntrys = journalentry;
        journal.doodles = doodle;
        journal.reflectionQuestions = reflectionquestion;

        return journal;
    }

    IEnumerator ObtainSheetData()
    {
        Debug.Log("Sheet Data loaded by Journal Sheet Reader One");
        UnityWebRequest www = UnityWebRequest.Get(JOURNAL_SHEET_URL);
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
            for (int i = 1; i < valuesArray.Count; i++)  // Start from 1 to skip the first row
            {
                var item = valuesArray[i];
                var id = int.Parse(item[0].Value);
                var journalTitle = item[1].Value;
                var journalEntry = item[2].Value;
                var doodle = Resources.Load<Sprite>(ResourcesLoadP + item[3].Value);
                var reflectionQuestion = item[4].Value;

                // Create the ScriptableObject and add it to the journals list
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
    }
}
