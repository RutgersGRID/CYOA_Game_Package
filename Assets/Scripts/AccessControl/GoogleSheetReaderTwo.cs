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
    private const string CONTROL_SHEET_URL = "https://sheets.googleapis.com/v4/spreadsheets/1OKYGVe7owHc-y_8GbxK-j1NO8LBMOowch6-SgTL7HxM/values/Access?key=AIzaSyDxlgY5nx2_JX89Grs3KZ7cnxlpRO2Nedg";

    public delegate void OnDataLoaded();
    public event OnDataLoaded onDataLoaded;

    private void Start()
    {
        //StartCoroutine(ObtainSheetData());
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
        //Debug.Log("ObtainSheetData started. Fetching data...");
        UnityWebRequest www = UnityWebRequest.Get(CONTROL_SHEET_URL);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("Error: " + www.error);
            // Handle the error accordingly.
        }
        else
        {
            //Debug.Log("Data fetched successfully. Processing...");
            var parsedJson = JSON.Parse(www.downloadHandler.text);
            var valuesArray = parsedJson["values"].AsArray;
            for (int i = 1; i < valuesArray.Count; i++)
            {
                var item = valuesArray[i];
                var accessCode = item[0].Value;
                var workshopIdCode = item[1].Value;
                logins.Add(CreateLoginSO(accessCode, workshopIdCode));

            }

            // Check if logins list was populated
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
