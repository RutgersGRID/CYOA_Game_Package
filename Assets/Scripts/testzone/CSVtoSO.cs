using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class CSVtoSO : MonoBehaviour
{
    public TextAsset csvFile;
    public List<CharacterSO> characters = new List<CharacterSO>();

    private string ResourcesLoadC = "Characters/";

    private string ResourcesLoadP = "Props/";

    private void Start()
    {
        ParseCSVData();
    }

    private void ParseCSVData()
    {
        string[] rows = csvFile.text.Split('\n');
        characters = (from row in rows.Skip(1)
                      let values = ParseCSVRow(row)
                      select new CharacterSO
                      {
                          Character_Name = values[0],
                          Character_Dialogue = values[1],
                          //Prop_Sprite = Resources.Load<Sprite>(ResourcesLoadP+values[2]),
                          //Character_Sprite = Resources.Load<Sprite>(ResourcesLoadC+values[3])
                          Prop_Sprite = values[2],
                          Character_Sprite = values[3]
                      }).ToList();
  
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
