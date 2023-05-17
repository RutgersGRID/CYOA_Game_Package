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
            journals = new List<JournalSO>();

            foreach (string row in rows.Skip(1))
            {
                string[] values = ParseCSVRow(row);

                JournalSO journal = ScriptableObject.CreateInstance<JournalSO>();
                journal.ID = int.Parse(values[0]);
                journal.journalTitle = values[1];
                journal.journalEntry = values[2];
                journal.reflectionQuestion = values[3];

                journals.Add(journal);
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

