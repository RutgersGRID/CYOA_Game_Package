using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CSVCreator : MonoBehaviour
{
    public string playerName = "John Doe";
    public int playerScore = 100;
    private string baseFileName = "PlayerData_";
    private string directoryPath;
    private string filePath;

    private void Start()
    {
        // Create the directory path for CSV exports
        directoryPath = Path.Combine(Application.dataPath, "Editor", "CSV Exports");
        
        // Check if directory exists, if not, create it
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        // Create a timestamp string for the filename
        string timeStamp = System.DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
        string fileName = baseFileName + timeStamp + ".csv";

        // Combining the directory path with the filename to get the full path
        filePath = Path.Combine(directoryPath, fileName);
        
        // Write the header as this will always be a new file
        string header = "Player Name,Score\n";
        File.WriteAllText(filePath, header);

        // Append player data to the CSV
        AppendToCSV(playerName, playerScore);
    }

    private void AppendToCSV(string name, int score)
    {
        // Convert data to CSV formatted line
        string csvLine = string.Format("{0},{1}\n", name, score);
        
        // Append the line to the file
        File.AppendAllText(filePath, csvLine);
    }

}
