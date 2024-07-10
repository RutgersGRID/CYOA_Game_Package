using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Sprites;

public class DCSVtoSO : MonoBehaviour
{

    public TextAsset csvFile;
    public List<DialogueSO> dialogues = new List<DialogueSO>();
    private string ResourcesLoadC = "Characters/";
    private string ResourcesLoadP = "Props/";
    private string ResourcesLoadBG = "Backgrounds/";

    private void Start()
    {
        ParseCSVData();
    }

    private void ParseCSVData()
    {
        string[] rows = csvFile.text.Split('\n');
        dialogues = (from row in rows.Skip(1)
                     let values = ParseCSVRow(row)
                     select CreateDialogueSO(values)).ToList();
        
        foreach (var dialogue in dialogues)
        {
            Debug.Log("ID: " + dialogue.ID + ", Speaker: " + dialogue.Speaker + ", Left and Right Speaker: " + dialogue.LeftSideSpeaker + " " + dialogue.RightSideSpeaker + ", Prop: " + dialogue.Prop + ", Checkpoint: " + dialogue.Checkpoint + ", Type: " + dialogue.Type + ", GoToID: " + dialogue.GoToID + ", A1Answer: " + dialogue.A1Answer);
        }
    }

    private DialogueSO CreateDialogueSO(string[] values)
    {
        DialogueSO dialogue = ScriptableObject.CreateInstance<DialogueSO>();
        dialogue.ID = ParseInt(values[0]);
        dialogue.Speaker = values[1];
        dialogue.Line = values[2];
        dialogue.LeftSideSpeaker = Resources.Load<Sprite>(ResourcesLoadC + values[3]);
        dialogue.RightSideSpeaker = Resources.Load<Sprite>(ResourcesLoadC + values[4]);
        dialogue.Prop = Resources.Load<Sprite>(ResourcesLoadP + values[5]);
        dialogue.Background = Resources.Load<Sprite>(ResourcesLoadBG + values[6]);
        dialogue.Checkpoint = ParseInt(values[7]);
        dialogue.Type = values[8];
        dialogue.GoToID = ParseInt(values[9]);
        dialogue.Effect = ParseInt(values[10]);

        dialogue.A1Answer = values[11];
        dialogue.GoToIDA1 = ParseInt(values[12]);
        dialogue.EffectA1 = ParseInt(values[13]);

        dialogue.A2Answer = values[14];
        dialogue.GoToIDA2 = ParseInt(values[15]);
        dialogue.EffectA2 = ParseInt(values[16]);

        dialogue.A3Answer = values[17];
        dialogue.GoToIDA3 = ParseInt(values[18]);
        dialogue.EffectA3 = ParseInt(values[19]);

        return dialogue;
    }

private int ParseInt(string s)
{
    if (string.IsNullOrEmpty(s))
    {
        return -1;
    }

    if (int.TryParse(s, out int result))
    {
        return result;
    }
    else
    {
        //Debug.LogWarning("Unable to parse integer value: " + s + ". Defaulting to -1.");
        return -1;
    }
}

private string[] ParseCSVRow(string row)
{
    List<string> values = new List<string>();
    StringReader reader = new StringReader(row);
    bool insideQuote = false;
    string value = "";

    while (reader.Peek() != -1)
    {
        char c = (char)reader.Read();

        if (c == '"')
        {
            insideQuote = !insideQuote;
        }
        else if (c == ',' && !insideQuote)
        {
            values.Add(string.IsNullOrEmpty(value) ? "-1" : value);
            value = "";
        }
        else
        {
            value += c;
        }
    }

    values.Add(string.IsNullOrEmpty(value) ? "-1" : value);

    return values.ToArray();
}
}
