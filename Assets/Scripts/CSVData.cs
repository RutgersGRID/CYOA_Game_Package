using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[CreateAssetMenu(fileName = "CSVData", menuName = "ScriptableObjects/CSVData")]
public class CSVData : ScriptableObject
{
    public string[] headers;
    public string[][] data;

    public static CSVData ParseCSV(string csvText)
    {
        string[] lines = csvText.Split('\n');
        string[] headers = lines[0].Split(',');
        string[][] data = lines
            .Skip(1)
            .Select(line => line.Split(','))
            .ToArray();

        CSVData csvData = CreateInstance<CSVData>();
        csvData.headers = headers;
        csvData.data = data;

        return csvData;
    }
}