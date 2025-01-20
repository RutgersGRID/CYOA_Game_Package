using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;

public class GoogleSheetReader : MonoBehaviour
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

    public void Initialize()
    {
        sheetId = PlayerPrefs.GetString("SheetId");
        if (string.IsNullOrEmpty(sheetId))
        {
            Debug.LogError("SheetID is invalid: Sheet ID is empty");
            return;
        }
        sheetUrl = sheetBaseUrl + sheetId + "/values/Access" + sheetKey;
        Debug.Log("Using SheetID: " + sheetId);
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
        if (string.IsNullOrEmpty(sheetId))
        {
            Debug.LogError("SheetID is invalid: Sheet ID is empty");
            yield break;
        }

        UnityWebRequest www = UnityWebRequest.Get(sheetUrl);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("SheetID is invalid: " + www.error);
            logins.Clear();
        }
        else
        {
            var parsedJson = JSON.Parse(www.downloadHandler.text);
            var valuesArray = parsedJson["values"]?.AsArray;
            
            if (valuesArray == null || valuesArray.Count <= 1)
            {
                Debug.LogError("SheetID is invalid: No data found in sheet or incorrect sheet format");
                logins.Clear();
                yield break;
            }

            logins.Clear();
            for (int i = 1; i < valuesArray.Count; i++)
            {
                var item = valuesArray[i];
                if (item.Count >= 2)
                {
                    var accessCode = item[0].Value;
                    var workshopIdCode = item[1].Value;
                    logins.Add(CreateLoginSO(accessCode, workshopIdCode));
                }
            }

            if (logins.Count == 0)
            {
                Debug.LogError("SheetID is invalid: No valid data rows found in sheet");
            }
            else
            {
                Debug.Log($"Successfully populated logins list with {logins.Count} entries.");
                onDataLoaded?.Invoke();
            }
        }
    }
}