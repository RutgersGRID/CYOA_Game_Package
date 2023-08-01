using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
public class CCSVtoSO : MonoBehaviour
{
    public TextAsset csvFile;

    public List<LoginSO> logins = new List<LoginSO>();
    void Start()
    {
        ParseCSVData();
    }

    void Update()
    {
        
    }   
    private void ParseCSVData()
    {
        string[] rows = csvFile.text.Split('\n');
        logins = (from row in rows.Skip(1)
                     let values = ParseCSVRow(row)
                     select CreateLoginSO(values)).ToList();
    }

    private LoginSO CreateLoginSO(string[] values)
    {
        LoginSO login = ScriptableObject.CreateInstance<LoginSO>();
        login.AccessCodes = values[0];
        login.WorkshopIDCodes = values[1];

        return login;
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
