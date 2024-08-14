using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;

public class GoogleSheetReaderTwo : MonoBehaviour
{
    [System.Serializable]
    public class LoginSO : ScriptableObject
    {
        public string AccessCodes;
        public string WorkshopIDCodes;
    }

    public List<LoginSO> logins = new List<LoginSO>();
    public string sheetBaseUrl = "https://sheets.googleapis.com/v4/spreadsheets/";
    public string sheetKey = "?key=AIzaSyDxlgY5nx2_JX89Grs3KZ7cnxlpRO2Nedg";
    public string sheetId;
    public string sheetUrl;
    public delegate void OnDataLoaded();
    public event OnDataLoaded onDataLoaded;

    private void Start()
    {

        sheetId = PlayerPrefs.GetString("SheetId");
        sheetUrl = sheetBaseUrl + sheetId + "/values/Access" + sheetKey;
        Debug.Log("Using SheetID: " + sheetId); // Added for debugging
        StartCoroutine(ObtainSheetData());
    }

    private LoginSO CreateLoginSO(string accessCode, string workshopIdCode)
    {
        LoginSO login = ScriptableObject.CreateInstance<LoginSO>();
        login.AccessCodes = accessCode;
        login.WorkshopIDCodes = workshopIdCode;


        return login;
    }

    public IEnumerator ObtainSheetData()
    {
        UnityWebRequest www = UnityWebRequest.Get(sheetUrl);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("Error: " + www.error);
        }
        else
        {
            var parsedJson = JSON.Parse(www.downloadHandler.text);
            var valuesArray = parsedJson["values"].AsArray;
            logins.Clear(); // Clear the existing list before adding new data
            for (int i = 1; i < valuesArray.Count; i++)
            {
                var item = valuesArray[i];
                var accessCode = item[0].Value;
                var workshopIdCode = item[1].Value;
                logins.Add(CreateLoginSO(accessCode, workshopIdCode));
            }

            if (logins.Count == 0)
            {
                Debug.LogWarning("The logins list was not populated. Please check the source or the structure of the Google Sheet.");
            }
            else
            {
                Debug.Log($"Successfully populated logins list with {logins.Count} entries.");
            }

            onDataLoaded?.Invoke();
        }
    }
}
