using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;

public class StorySheetReader : MonoBehaviour
{
    [System.Serializable]
    public class DialogueSO : ScriptableObject
    {
        public int ID;
        public string Speaker;
        public string Line;
        public string Keyword;
        public AudioClip SoundEFX;
        public Sprite LeftSideSpeaker;
        public Sprite RightSideSpeaker;
        public Sprite Prop;
        public Sprite Background;
        public int Checkpoint;
        public string Type;
        public int GoToID;
        public int Effect;
        public string A1Answer;
        public int GoToIDA1;
        public int EffectA1;
        public string A2Answer;
        public int GoToIDA2;
        public int EffectA2;
        public string A3Answer;
        public int GoToIDA3;
        public int EffectA3;
        //public string EntryPoint;
    }

    public List<DialogueSO> dialogues = new List<DialogueSO>();
    private string ResourcesLoadC = "Characters/";
    private string ResourcesLoadP = "Props/";
    private string ResourcesLoadBG = "Backgrounds/";
    private const string STORY_SHEET_URL = "https://sheets.googleapis.com/v4/spreadsheets/1OKYGVe7owHc-y_8GbxK-j1NO8LBMOowch6-SgTL7HxM/values:batchGet?ranges=Story1&key=AIzaSyDxlgY5nx2_JX89Grs3KZ7cnxlpRO2Nedg";
    
    private void Start()
    {
        StartCoroutine(ObtainSheetData());
    }
    private DialogueSO CreateDialogueSO(int ID, string Speaker, string Line, Sprite LeftSideSpeaker, Sprite RightSideSpeaker, Sprite Prop, Sprite Background, int Checkpoint, string Type, int GoToID, int Effect, string A1Answer, int GoToIDA1, int EffectA1, string A2Answer, int GoToIDA2, int EffectA2, string A3Answer, int GoToIDA3, int EffectA3)
    {
        DialogueSO dialogue = ScriptableObject.CreateInstance<DialogueSO>();

    // Assign all properties to the created object
    dialogue.ID = ID;
    dialogue.Speaker = Speaker;
    dialogue.Line = Line;
    dialogue.LeftSideSpeaker = LeftSideSpeaker;
    dialogue.RightSideSpeaker = RightSideSpeaker;
    dialogue.Prop = Prop;
    dialogue.Background = Background;
    dialogue.Checkpoint = Checkpoint;
    dialogue.Type = Type;
    dialogue.GoToID = GoToID;
    dialogue.Effect = Effect;
    dialogue.A1Answer = A1Answer;
    dialogue.GoToIDA1 = GoToIDA1;
    dialogue.EffectA1 = EffectA1;
    dialogue.A2Answer = A2Answer;
    dialogue.GoToIDA2 = GoToIDA2;
    dialogue.EffectA2 = EffectA2;
    dialogue.A3Answer = A3Answer;
    dialogue.GoToIDA3 = GoToIDA3;
    dialogue.EffectA3 = EffectA3;
    //dialogue.EntryPoint = EntryPoint;

    return dialogue;
    }

    IEnumerator ObtainSheetData()
    {
        Debug.Log("Sheet Data loaded by Story Sheet Reader (One)");
        UnityWebRequest www = UnityWebRequest.Get(STORY_SHEET_URL);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error fetching data: " + www.error);
        }
        else
        {
            var parsedJson = JSON.Parse(www.downloadHandler.text);
            var valuesArray = parsedJson["values"].AsArray;
            for (int i = 1; i < valuesArray.Count; i++)  // Start from 1 to skip the first row
            {
                var item = valuesArray[i];

                Sprite LeftSideSpeaker = Resources.Load<Sprite>(ResourcesLoadC + item[3].Value);
                Sprite RightSideSpeaker = Resources.Load<Sprite>(ResourcesLoadC + item[4].Value);
                Sprite Prop = Resources.Load<Sprite>(ResourcesLoadP + item[5].Value);
                Sprite Background = Resources.Load<Sprite>(ResourcesLoadBG + item[6].Value);

                int ID = int.Parse(item[0].Value);
                string Speaker = item[1].Value;
                string Line = item[2].Value;
                int Checkpoint = int.Parse(item[7].Value);
                string Type = item[8].Value;
                int GoToID = int.Parse(item[9].Value);
                int Effect = int.Parse(item[10].Value);
                string A1Answer = item[11].Value;
                int GoToIDA1 = int.Parse(item[12].Value);
                int EffectA1 = int.Parse(item[13].Value);
                string A2Answer = item[14].Value;
                int GoToIDA2 = int.Parse(item[15].Value);
                int EffectA2 = int.Parse(item[16].Value);
                string A3Answer = item[17].Value;
                int GoToIDA3 = int.Parse(item[18].Value);
                int EffectA3 = int.Parse(item[19].Value);
                //string EntryPoint = Parse(item[20].Value);

                dialogues.Add(CreateDialogueSO(ID, Speaker, Line, LeftSideSpeaker, RightSideSpeaker, Prop, Background, Checkpoint, Type, GoToID, Effect, A1Answer, GoToIDA1, EffectA1, A2Answer, GoToIDA2, EffectA2, A3Answer, GoToIDA3, EffectA3));
                Debug.Log("Dialogue added. Current count: " + dialogues.Count);
            }
        }

        if (dialogues.Count == 0)
        {
                Debug.LogWarning("The dialogues list was not populated. Please check the source or the structure of the Google Sheet.");
        }
            else
            {
                Debug.Log($"Successfully populated dialogues list with {dialogues.Count} entries from "+ STORY_SHEET_URL);
            }
    
        }
    }