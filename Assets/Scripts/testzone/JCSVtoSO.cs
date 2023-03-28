using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Sprites;

public class JCSVtoSO : MonoBehaviour
{
    public TextAsset csvFile;
        //public List<CharacterSO> characters = new List<CharacterSO>();
        public List<JournalSO> journals = new List<JournalSO>();

        private void Start()
        {
            ParseCSVData();
        }

        private void ParseCSVData()
        {
            string[] rows = csvFile.text.Split('\n');
            // characters = (from row in rows.Skip(1)
            journals = (from row in rows.Skip(1)
                        let values = ParseCSVRow(row)
                        //select new CharacterSO
                        select new JournalSO
                        {
                            ID = int.Parse(values[0]),
                            eventTitle = values[1],
                            eventText = values[2],
                            journalEntry = values[3],

                        }).ToList();
            
            // foreach (var character in characters)
            // {
            //     Debug.Log("Loaded character " + character.Character_Name + " with prop sprite " + character.Prop_Sprite + " and character sprite " + character.Character_Sprite );
            // }
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

