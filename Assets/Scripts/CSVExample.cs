using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVExample : MonoBehaviour
{
    public TextAsset csvFile;
    public CSVData csvData;

    private void Start()
    {
        csvData = CSVData.ParseCSV(csvFile.text);
    }
}
