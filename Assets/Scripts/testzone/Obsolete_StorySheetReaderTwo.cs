using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.Networking;
using SimpleJSON;

public class Obsolete_StorySheetReaderTwo : MonoBehaviour
{
    [System.Serializable]
    public class DialogueSO : ScriptableObject
    {
        public int IDs;
        public string Speakers;
        public string Lines;
        public string Keywords;
        public AudioClip SoundEFXs;
        public Sprite LeftSideSpeakers;
        public Sprite RightSideSpeakers;
        public Sprite Props;
        public Sprite Backgrounds;
        public int Checkpoints;
        public string Types;
        public int GoToIDs;
        public int Effects;
        public string A1Answers;
        public int GoToIDA1s;
        public int EffectA1s;
        public string A2Answers;
        public int GoToIDA2s;
        public int EffectA2s;
        public string A3Answers;
        public int GoToIDA3s;
        public int EffectA3s;
        public string EntryPoints;
    }
    public List<DialogueSO> dialogues = new List<DialogueSO>();
    private string ResourcesLoadC = "Characters/";
    private string ResourcesLoadP = "Props/";
    private string ResourcesLoadBG = "Backgrounds/";
    private string ResourcesLoadSEFX = "Sounds/";
    private const string OBSOLETE_SHEET_URL = "https://sheets.googleapis.com/v4/spreadsheets/1ksXqcHwthFPc_jMstAku4xuhxFUs5q93W6L78eyfX2U/values/Sheet1?key=AIzaSyDxlgY5nx2_JX89Grs3KZ7cnxlpRO2Nedg";
    public delegate void OnDataLoaded();
    public event OnDataLoaded onDataLoaded;
    void Start()
    {
        // StartCoroutine(ObtainSheetData());
    }

    private DialogueSO CreateDialogueSO(int ID, string Speaker, string Line, string Keyword, AudioClip SoundEFX, Sprite LeftSideSpeaker, Sprite RightSideSpeaker, Sprite Prop, Sprite Background, int Checkpoint, string Type, int GoToID, int Effect, string A1Answer, int GoToIDA1, int EffectA1, string A2Answer, int GoToIDA2, int EffectA2, string A3Answer, int GoToIDA3, int EffectA3, string EntryPoint)
    {
        DialogueSO dialogue = ScriptableObject.CreateInstance<DialogueSO>();
        dialogue.IDs = ID;
        dialogue.Speakers = Speaker;
        dialogue.Lines = Line;
        dialogue.Keywords = Keyword;
        dialogue.SoundEFXs = SoundEFX;
        dialogue.LeftSideSpeakers = LeftSideSpeaker;
        dialogue.RightSideSpeakers = RightSideSpeaker;
        dialogue.Props = Prop;
        dialogue.Backgrounds = Background;
        dialogue.Checkpoints = Checkpoint;
        dialogue.Types = Type;
        dialogue.GoToIDs = GoToID;
        dialogue.Effects = Effect;
        dialogue.A1Answers = A1Answer;
        dialogue.GoToIDA1s = GoToIDA1;
        dialogue.EffectA1s = EffectA1;
        dialogue.A2Answers= A2Answer;
        dialogue.GoToIDA2s = GoToIDA2;
        dialogue.EffectA2s = EffectA2;
        dialogue.A3Answers = A3Answer;
        dialogue.GoToIDA3s = GoToIDA3;
        dialogue.EffectA3s = EffectA3;
        dialogue.EntryPoints = EntryPoint;

        return dialogue;
    }

    private int SafeIntParse(string str, int defaultValue = -1)
    {
        if (string.IsNullOrEmpty(str))
        {
            return defaultValue;
        }

        if (int.TryParse(str, out int result))
        {
            return result;
        }
        else
        {
            Debug.LogWarning($"Failed to parse \"{str}\" as an integer. Using default value: {defaultValue}.");
            return defaultValue;
        }
    }

    private string SafeStringParse(string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return null;
        }
        return str;
    }

    public IEnumerator ObtainSheetData()
    {
        UnityWebRequest www = UnityWebRequest.Get(OBSOLETE_SHEET_URL);
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
            for (int i = 1; i < valuesArray.Count; i++)
            {
                var item = valuesArray[i];
                var ID = SafeIntParse(item[0].Value);
                var Speaker = item[1].Value;
                var Line = item[2].Value;
                var Keyword = item[3].Value;
                var SoundEFX = Resources.Load<AudioClip>(ResourcesLoadSEFX + item[4].Value);
                var LeftSideSpeaker = Resources.Load<Sprite>(ResourcesLoadC + item[5].Value);
                var RightSideSpeaker = Resources.Load<Sprite>(ResourcesLoadC + item[6].Value);
                var Prop = Resources.Load<Sprite>(ResourcesLoadP + item[7].Value);
                var Background = Resources.Load<Sprite>(ResourcesLoadBG + item[8].Value);
                var Checkpoint = SafeIntParse(item[9].Value);
                var Type = item[10].Value;
                var GoToID = SafeIntParse(item[11].Value);
                var Effect = SafeIntParse(item[12].Value);
                var A1Answer = item[13].Value;
                var GoToIDA1 = SafeIntParse(item[14].Value);
                var EffectA1 = SafeIntParse(item[15].Value);
                var A2Answer = item[16].Value;
                var GoToIDA2 = SafeIntParse(item[17].Value);
                var EffectA2 = SafeIntParse(item[18].Value);
                var A3Answer = item[19].Value;
                var GoToIDA3 = SafeIntParse(item[20].Value);
                var EffectA3 = SafeIntParse(item[21].Value);
                var EntryPoint = item[22].Value;

                dialogues.Add(CreateDialogueSO(ID, Speaker, Line, Keyword, SoundEFX, LeftSideSpeaker, RightSideSpeaker, Prop, Background, Checkpoint, Type, GoToID, Effect, A1Answer, GoToIDA1, EffectA1, A2Answer, GoToIDA2, EffectA2, A3Answer, GoToIDA3, EffectA3, EntryPoint));
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
