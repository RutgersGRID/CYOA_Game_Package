using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class oldCSVtoSO : MonoBehaviour
{
    public TextAsset csvFile;
    public List<CharacterSO> characters = new List<CharacterSO>();

    private void Start()
    {
        ParseCSVData();
    }

    private void ParseCSVData()
    {
        // Split the CSV file into rows
        string[] rows = csvFile.text.Split('\n');

        // Skip the header row
        for (int i = 1; i < rows.Length; i++)
        {
            // Split the row into values
            string[] values = ParseCSVRow(rows[i]);

            // Create a new CharacterSO scriptable object
            CharacterSO character = ScriptableObject.CreateInstance<CharacterSO>();
            character.Character_Name = values[0];
            character.Character_Dialogue = values[1];
            //character.Prop_Sprite = values[2];
            //character.Character_Sprite = values[3];

            characters.Add(character);
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