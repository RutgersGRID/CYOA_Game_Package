using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Sprites;

public class CSVtoSOTwo : MonoBehaviour
{
    public TextAsset csvFile;
    //public List<CharacterSO> characters = new List<CharacterSO>();
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
                 select new DialogueSO
                 {
                     ID = int.Parse(values[0]),
                     Speaker = values[1],
                     Line = values[2],
                     LeftSideSpeaker = Resources.Load<Sprite>(ResourcesLoadC + values[3]),
                     RightSideSpeaker = Resources.Load<Sprite>(ResourcesLoadC + values[4]),
                     Prop = Resources.Load<Sprite>(ResourcesLoadP + values[5]),
                     Background = Resources.Load<Sprite>(ResourcesLoadBG + values[6]),
                     Checkpoint = int.Parse(values[7]),
                     Type = values[8],
                     GoToID = int.Parse(values[9]),
                     A1Answer = values[10],
                     GoToIDA1 = ParseInt(values[11]),
                     A2Answer = values[12],
                     GoToIDA2 = ParseInt(values[13]),
                     A3Answer = values[14],
                     GoToIDA3 = ParseInt(values[15])
                 }).ToList();

    // foreach (var dialogue in dialogues)
    // {
    //     Debug.Log("ID:" + dialogue.ID + ", Speaker:" + dialogue.Speaker + ", Left and Right Speaker:" + dialogue.LeftSideSpeaker + dialogue.RightSideSpeaker + ", Prop:" + dialogue.Prop + ", Background:"+ dialogue.Background + ", Checkpoint:" + dialogue.Checkpoint + ", Type:" + dialogue.Type + ", GoToID:" + dialogue.GoToID + ", A1Answer:" + dialogue.A1Answer);
    // }
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
