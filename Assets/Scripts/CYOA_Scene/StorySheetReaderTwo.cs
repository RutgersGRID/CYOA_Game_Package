using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class StorySheetReaderTwo : MonoBehaviour
{
    [System.Serializable]
    public class DialogueSO : ScriptableObject
    {
        public string IDs;
        public string Speakers;
        public string Lines;
        public string Keywords;
        public AudioClip SoundEFXs;
        public Sprite LeftSideSpeakers;
        public Sprite RightSideSpeakers;
        public Sprite Props;
        public Sprite Backgrounds;
        public string Checkpoints;
        public string Types;
        public string GoToIDs;
        public int JournalTriggers;
        public string A1Answers;
        public string GoToIDA1s;
        public int JournalTriggerA1s;
        public string A2Answers;
        public string GoToIDA2s;
        public int JournalTriggerA2s;
        public string A3Answers;
        public string GoToIDA3s;
        public int JournalTriggerA3s;
    }
    public List<DialogueSO> dialogues = new List<DialogueSO>();
    private string ResourcesLoadC = "Characters/";
    private string ResourcesLoadP = "Props/";
    private string ResourcesLoadBG = "Backgrounds/";
    private string ResourcesLoadSEFX = "Sounds/";
    private string sheetBaseUrl = "https://sheets.googleapis.com/v4/spreadsheets/";
    private string sheetKey = "?key=AIzaSyDa6TYGcPDdOCI5V3Rq7YJlo9d-FCugzXQ"; // Replace with your Google Sheets API key
    private string sheetId;
    private string sheetUrl;
    public delegate void OnDataLoaded();
    public event OnDataLoaded onDataLoaded;

    void Start()
    {
        sheetId = PlayerPrefs.GetString("SheetId");
        sheetUrl = sheetBaseUrl + sheetId + "/values/StorySheet" + sheetKey;
        StartCoroutine(ObtainSheetData());
    }

    private DialogueSO CreateDialogueSO(string ID, string Speaker, string Line, string Keyword, AudioClip SoundEFX, string leftSideSpeakerFileName, string rightSideSpeakerFileName, string propFileName, string backgroundFileName, string Checkpoint, string Type, string GoToID, int JournalTrigger, string A1Answer, string GoToIDA1, int JournalTriggerA1, string A2Answer, string GoToIDA2, int JournalTriggerA2, string A3Answer, string GoToIDA3, int JournalTriggerA3)
    {
        DialogueSO dialogue = ScriptableObject.CreateInstance<DialogueSO>();

        dialogue.IDs = ID;
        dialogue.Speakers = Speaker;
        dialogue.Lines = Line;
        dialogue.Keywords = Keyword;
        dialogue.SoundEFXs = SoundEFX;

        dialogue.LeftSideSpeakers = AssetManager.Instance.GetSprite(leftSideSpeakerFileName);
        dialogue.RightSideSpeakers = AssetManager.Instance.GetSprite(rightSideSpeakerFileName);
        dialogue.Props = AssetManager.Instance.GetSprite(propFileName);
        dialogue.Backgrounds = AssetManager.Instance.GetSprite(backgroundFileName);

        dialogue.Checkpoints = Checkpoint;
        dialogue.Types = Type;
        dialogue.GoToIDs = GoToID;
        dialogue.JournalTriggers = JournalTrigger;
        dialogue.A1Answers = A1Answer;
        dialogue.GoToIDA1s = GoToIDA1;
        dialogue.JournalTriggerA1s = JournalTriggerA1;
        dialogue.A2Answers = A2Answer;
        dialogue.GoToIDA2s = GoToIDA2;
        dialogue.JournalTriggerA2s = JournalTriggerA2;
        dialogue.A3Answers = A3Answer;
        dialogue.GoToIDA3s = GoToIDA3;
        dialogue.JournalTriggerA3s = JournalTriggerA3;

        return dialogue;
    }

    public IEnumerator ObtainSheetData()
    {
        Debug.Log("Sheet Data loaded by Story Sheet Reader Two");
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
                var ID = item[0].Value;
                var Speaker = item[1].Value;
                var Line = item[2].Value;
                var Keyword = item[3].Value;
                var SoundEFX = Resources.Load<AudioClip>(ResourcesLoadSEFX + item[4].Value);
                var leftSideSpeakerFileName = item[5].Value;
                var rightSideSpeakerFileName = item[6].Value;
                var propFileName = item[7].Value;
                var backgroundFileName = item[8].Value;
                var Checkpoint = item[9].Value;
                var Type = item[10].Value;
                var GoToID = item[11].Value;

                int JournalTrigger = 0;
                int.TryParse(item[12].Value, out JournalTrigger);

                var A1Answer = item[13].Value;
                var GoToIDA1 = item[14].Value;

                int JournalTriggerA1 = 0;
                int.TryParse(item[15].Value, out JournalTriggerA1);

                var A2Answer = item[16].Value;
                var GoToIDA2 = item[17].Value;

                int JournalTriggerA2 = 0;
                int.TryParse(item[18].Value, out JournalTriggerA2);

                var A3Answer = item[19].Value;
                var GoToIDA3 = item[20].Value;

                int JournalTriggerA3 = 0;
                int.TryParse(item[21].Value, out JournalTriggerA3);

                dialogues.Add(CreateDialogueSO(ID, Speaker, Line, Keyword, SoundEFX, leftSideSpeakerFileName, rightSideSpeakerFileName, propFileName, backgroundFileName, Checkpoint, Type, GoToID, JournalTrigger, A1Answer, GoToIDA1, JournalTriggerA1, A2Answer, GoToIDA2, JournalTriggerA2, A3Answer, GoToIDA3, JournalTriggerA3));
            }
        }
        if (dialogues.Count == 0)
        {
            Debug.LogWarning("FROM StorySheetReaderTwo");
            Debug.LogWarning("The dialogues list was not populated. Please check the source or the structure of the Google Sheet.");
        }
        else
        {
            Debug.Log($"FROM StorySheetReaderTwo");
            Debug.Log($"Successfully populated dialogues list with {dialogues.Count} entries.");
        }
        onDataLoaded?.Invoke();
    }
}
