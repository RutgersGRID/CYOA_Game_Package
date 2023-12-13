using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Networking;
using SimpleJSON;

public class FrameReader : MonoBehaviour
{
    [System.Serializable]
    public class FrameSO : ScriptableObject
    {
        public int IDs;
        public Sprite frameStills;
    }
    public List<FrameSO> frames = new List<FrameSO>();
    private string ResourcesLoadAI = "AnimationImages/";
    private const string SHEET_URL = "https://sheets.googleapis.com/v4/spreadsheets/1yCNwNPDCFJP4VmggLayE66kTOhGo2xjYBN_iaiWQFvM/values/Frames?key=AIzaSyDxlgY5nx2_JX89Grs3KZ7cnxlpRO2Nedg";
    public delegate void OnDataLoaded();
    public event OnDataLoaded onDataLoaded;
    void Start()
    {
        // StartCoroutine(ObtainSheetData());
    }
    private FrameSO CreateFrameSO(int id, Sprite frameStill)
    {
        FrameSO frame = ScriptableObject.CreateInstance<FrameSO>();
        frame.IDs = id;
        frame.frameStills = frameStill;
        return frame;
    }
    private int SafeIntParse(string str, int defaultValue = -1)
    {
        if (string.IsNullOrEmpty(str))
        {
            return defaultValue;
        }

        if (int.TryParse(str, out int result))
        {
            return result;
        }
        else
        {
            Debug.LogWarning($"Failed to parse \"{str}\" as an integer. Using default value: {defaultValue}.");
            return defaultValue;
        }
    }

    private string SafeStringParse(string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return null;
        }
        return str;
    }
    public IEnumerator ObtainSheetData()
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
            for(int i = 1; i < valuesArray.Count; i++)
            {
                var item = valuesArray[i];
                var id = int.Parse(item[0].Value);
                var frameStill = Resources.Load<Sprite>(ResourcesLoadAI + item[1].Value);
                
                frames.Add(CreateFrameSO(id,frameStill));
            }
        }
        if (frames.Count == 0)
        {
            Debug.LogWarning("FROM FrameReader");
            Debug.LogWarning("The frames list was not populated. Please check the source or the structure of the Google Sheet.");
        }
        else
        {
            Debug.Log($"FROM FrameReader");
            Debug.Log($"Successfully populated framess list with {frames.Count} entries.");
        }
        onDataLoaded?.Invoke();
    }
}