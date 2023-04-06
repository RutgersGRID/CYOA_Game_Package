using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using System;

// Linq extension method for parsing csv files
public static class CsvParser
{
    public static IEnumerable<string[]> Parse(string csv)
    {
        StringReader sr = new StringReader(csv);
        while (sr.Peek() >= 0)
        {
            string line = sr.ReadLine();
            string[] fields = ParseLine(line);
            yield return fields;
        }
    }

    private static string[] ParseLine(string line)
    {
        return line.Split(new char[] { ',' }, System.StringSplitOptions.None)
                   .Select(field => 
                   {
                       if (field.StartsWith("\"") && field.EndsWith("\""))
                       {
                           field = field.Substring(1, field.Length - 2);
                       }
                       return field.Trim();
                   })
                   .ToArray();
    }
}

// Main script to parse the csv file
public class linqtest : MonoBehaviour
{
    public TextAsset csvFile;

    void Start()
    {
        // Get the csv file as a string
        string csv = csvFile.ToString();

        // Use the Linq extension method to parse the csv file
        IEnumerable<string[]> data = CsvParser.Parse(csv);

        // Loop through each line of the csv file
        foreach (string[] fields in data)
        {
            // Do something with the data in each field
            // Example: print each field to the console
            foreach (string field in fields)
            {
                Debug.Log(field);
            }
        }
    }
}