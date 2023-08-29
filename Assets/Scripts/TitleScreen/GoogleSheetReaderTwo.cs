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
    // public Text display;
    private const string SHEET_URL = "https://sheets.googleapis.com/v4/spreadsheets/1cC9iRPYMR9jgyKbeBM-wBO03rG5SPvONt8t-5fNJrTs/values/Codes?key=AIzaSyDxlgY5nx2_JX89Grs3KZ7cnxlpRO2Nedg";

    private void Start()
    {
        StartCoroutine(ObtainSheetData());
    }

    private LoginSO CreateLoginSO(string accessCode, string workshopIdCode)
    {
        LoginSO login = ScriptableObject.CreateInstance<LoginSO>();
        login.AccessCodes = accessCode;
        login.WorkshopIDCodes = workshopIdCode;

        return login;
    }

    IEnumerator ObtainSheetData()
    {
        UnityWebRequest www = UnityWebRequest.Get(SHEET_URL);
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
            for (int i = 1; i < valuesArray.Count; i++)  // Start from 1 to skip the first row
            {
                var item = valuesArray[i];
                var accessCode = item[0].Value;
                var workshopIdCode = item[1].Value;
                
                // Create the ScriptableObject and add it to the logins list
                logins.Add(CreateLoginSO(accessCode, workshopIdCode));
            }
        }
    }
}
