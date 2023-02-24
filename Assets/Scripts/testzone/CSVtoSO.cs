using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Sprites;

public class CSVtoSO : MonoBehaviour
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
        // characters = (from row in rows.Skip(1)
        dialogues = (from row in rows.Skip(1)
                      let values = ParseCSVRow(row)
                      //select new CharacterSO
                      select new DialogueSO
                      {
                        ID = int.Parse(values[0]),
                        Speaker = values[1],
                        Line = values[2],
                        // LeftSideSpeaker = values[3],
                        LeftSideSpeaker = Resources.Load<Sprite>(ResourcesLoadC + values[3]),
                        // RightSideSpeaker = values[4],
                        RightSideSpeaker = Resources.Load<Sprite>(ResourcesLoadC + values[4]),
                        //Prop = values[5]
                        Prop = Resources.Load<Sprite>(ResourcesLoadP + values[5]),
                        // Background = values[6],
                        Background = Resources.Load<Sprite>(ResourcesLoadBG + values[6]),
                        Checkpoint = int.Parse(values[7]),
                        Type = values[8],
                        
                        GoToID = int.Parse(values[9]),
                        A1Answer = values[10],
                        GoToIDA1 = int.Parse(values[11]),
                        A2Answer = values[12],
                        GoToIDA2 = int.Parse(values[13]),
                        A3Answer = values[14],
                        GoToIDA3 = int.Parse(values[15])
                        // Character_Name = values[0],
                        // Character_Dialogue = values[3],
                        // Prop_Sprite = Resources.Load<Sprite>("Props/" + values[1]),
                        // Character_Sprite = Resources.Load<Sprite>("Characters/" + values[2])

                      }).ToList();
        
        // foreach (var character in characters)
        // {
        //     Debug.Log("Loaded character " + character.Character_Name + " with prop sprite " + character.Prop_Sprite + " and character sprite " + character.Character_Sprite );
        // }

        foreach (var dialogue in dialogues)
        {
            Debug.Log("ID:" + dialogue.ID + ", Speaker:" + dialogue.Speaker + ", Left and Right Speaker:" + dialogue.LeftSideSpeaker + dialogue.RightSideSpeaker + ", Prop:" + dialogue.Prop +", Checkpoint:" + dialogue.Checkpoint + ", Type:" + dialogue.Type + ", GoToID:" + dialogue.GoToID + ", A1Answer:" + dialogue.A1Answer);
        }
    }


    private string[] ParseCSVRow(string row)
    {
        List<string> values = new List<string>();
        Debug.Log(values);
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
                values.Add(value);
                value = "";
            }
            else
            {
                value += c;
            }
        }

        values.Add(value);

        return values.ToArray();
    }
}
