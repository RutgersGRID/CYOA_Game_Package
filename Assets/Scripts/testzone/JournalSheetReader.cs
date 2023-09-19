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
        public int ID;
        public string journalTitle;
        public string journalEntry;
        public Sprite Doodle;
        public string reflectionQuestion;
        
    }

    public List<JournalSO> journals = new List<JournalSO>();
    private string ResourcesLoadP = "Props/";
    // public Text display;
    private const string JOURNAL_SHEET_URL = "https://sheets.googleapis.com/v4/spreadsheets/1yCNwNPDCFJP4VmggLayE66kTOhGo2xjYBN_iaiWQFvM/values:batchGet?ranges=Sheet1&key=AIzaSyDxlgY5nx2_JX89Grs3KZ7cnxlpRO2Nedg";

    private void Start()
    {
        StartCoroutine(ObtainSheetData());
    }

    private JournalSO CreateJournalSO(int id, string journaltitle, string journalentry, string doodleLink, string reflectionquestion)
    {
        JournalSO journal = ScriptableObject.CreateInstance<JournalSO>();
        journal.ID = id;
        journal.journalTitle = journaltitle;
        journal.journalEntry = journalentry;
        journal.Doodle = Resources.Load<Sprite>(ResourcesLoadP + doodleLink);
        journal.reflectionQuestion = reflectionquestion;

        return journal;
    }

    IEnumerator ObtainSheetData()
    {
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
                int ID = int.Parse(item[0].Value);
                string journalTitle = item[1].Value;
                string journalEntry = item[2].Value;
                // Assuming the next value is a link to an image for the doodle
                string doodleLink = item[3].Value;
                string reflectionQuestion = item[4].Value;

                // Create the ScriptableObject and add it to the journals list
                journals.Add(CreateJournalSO(ID, journalTitle, journalEntry, doodleLink, reflectionQuestion));
            }
        }
    }
}
