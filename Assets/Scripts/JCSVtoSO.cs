using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Sprites;

public class JCSVtoSO : MonoBehaviour
{
    public TextAsset csvFile;
        public List<JournalSO> journals = new List<JournalSO>();
        //private string ResourcesLoadC = "Characters/";
        private string ResourcesLoadP = "Props/";
        //private string ResourcesLoadBG = "Backgrounds/";

        private void Start()
        {
            ParseCSVData();
        }


    private void ParseCSVData()
    {
        string[] rows = csvFile.text.Split('\n');
        journals = (from row in rows.Skip(1)
                     let values = ParseCSVRow(row)
                     select CreateJournalSO(values)).ToList();
    }

    private JournalSO CreateJournalSO(string[] values)
    {
        JournalSO journal = ScriptableObject.CreateInstance<JournalSO>();
        journal.ID = ParseInt(values[0]);
        journal.journalTitle = values[1];
        journal.journalEntry = values[2];
        journal.doodle = Resources.Load<Sprite>(ResourcesLoadP + values[3]);
        journal.reflectionQuestion = values[4];

        return journal;
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

