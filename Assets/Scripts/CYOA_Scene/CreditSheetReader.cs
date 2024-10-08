using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;
public class CreditSheetReader : MonoBehaviour
{
    [System.Serializable]
    public class CreditSO : ScriptableObject
    {
        public string creditTexts;
        public string htpTexts;
        public string creditGRIDTexts;
    }
    public List<CreditSO> credits = new List<CreditSO>();
    //Pointer to Alex's sheets
    private string sheetBaseUrl = "https://sheets.googleapis.com/v4/spreadsheets/";
    private string sheetKey = "?key=AIzaSyDxlgY5nx2_JX89Grs3KZ7cnxlpRO2Nedg";
    private string sheetId;
    private string sheetUrl;
    public delegate void OnDataLoaded();
    public event OnDataLoaded onDataLoaded;
    void Start()
    {

        sheetId = PlayerPrefs.GetString("SheetId");
        sheetUrl = sheetBaseUrl + sheetId + "/values/CreditSheet" + sheetKey;
        StartCoroutine(ObtainSheetData());
    }
    private CreditSO CreateCreditSO(string creditText, string htpText, string creditGRIDText)
    {
        CreditSO credit = ScriptableObject.CreateInstance<CreditSO>();
        credit.creditTexts = creditText;
        credit.htpTexts = htpText;
        credit.creditGRIDTexts = creditGRIDText;

        return credit;
    }
    public IEnumerator ObtainSheetData()
    {
        UnityWebRequest www = UnityWebRequest.Get(sheetUrl);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("Error: " + www.error);
            // Handle the error accordingly.
        }
        else
        {
            var parsedJson = JSON.Parse(www.downloadHandler.text);
            var valuesArray = parsedJson["values"].AsArray;
            for(int i = 1; i < valuesArray.Count; i++)
            {
                var item = valuesArray[i];
                var creditText = item[0].Value;
                var htpText = item[1].Value;
                var creditGRIDText = item[2].Value;
                
                credits.Add(CreateCreditSO(creditText, htpText, creditGRIDText));
            }
        }
        if (credits.Count == 0)
        {
            Debug.LogWarning("FROM CreditSO");
            Debug.LogWarning("The credits list was not populated. Please check the source or the structure of the Google Sheet.");
        }
        else
        {
            Debug.Log($"FROM CreditSO");
            Debug.Log($"Successfully populated credits list with {credits.Count} entries.");
        }
        onDataLoaded?.Invoke();
    }
}
