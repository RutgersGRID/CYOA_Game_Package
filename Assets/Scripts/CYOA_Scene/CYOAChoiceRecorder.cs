using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CYOAChoiceRecorder : MonoBehaviour
{
    [Header("Google Sheet Settings")]
    [Tooltip("URL to the Google Apps Script Web App")]
    [SerializeField] private string webAppURL = "";

    [Tooltip("Should data be automatically saved on game exit")]
    [SerializeField] private bool autoSaveOnQuit = true;
    
    [Header("References")]
    [Tooltip("Reference to UIPopulator component")]
    [SerializeField] private UIPopulator uiPopulator;

    // Queue for data waiting to be sent
    private Queue<ChoiceDataEntry> dataQueue = new Queue<ChoiceDataEntry>();
    private bool isSending = false;

    [System.Serializable]
    public class ChoiceDataEntry
    {
        // Player and session identifiers
        public string playerID;
        public string accessCode; 
        public string workshopID;
        public string timestamp;
        public string sessionID;
        
        // Choice data
        public string currentSceneID;
        public string questionText;
        public string selectedChoice;
        public int choiceIndex;
        public string nextSceneID;
        
        public ChoiceDataEntry(
            string playerID, 
            string accessCode,
            string workshopID,
            string sessionID,
            string currentSceneID,
            string questionText,
            string selectedChoice,
            int choiceIndex,
            string nextSceneID)
        {
            this.playerID = playerID;
            this.accessCode = accessCode;
            this.workshopID = workshopID;
            this.timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.sessionID = sessionID;
            this.currentSceneID = currentSceneID;
            this.questionText = questionText;
            this.selectedChoice = selectedChoice;
            this.choiceIndex = choiceIndex;
            this.nextSceneID = nextSceneID;
        }
    }

    void Start()
    {
        // Find UIPopulator if not assigned
        if (uiPopulator == null)
        {
            uiPopulator = FindObjectOfType<UIPopulator>();
            if (uiPopulator == null)
            {
                Debug.LogError("UIPopulator not found! Make sure it exists in the scene or assign it in the inspector.");
            }
        }
    }

    private void OnApplicationQuit()
    {
        if (autoSaveOnQuit && dataQueue.Count > 0)
        {
            // Force sync operation on quit
            StartCoroutine(ProcessQueueSync());
        }
    }

    public void RecordChoices(string sceneID, string questionText, string[] allChoices)
    {
        Debug.LogWarning("RecordChoices method is deprecated. Use RecordPlayerChoice instead.");
        
        // Get access code and workshop ID from PlayerPrefs (set by AccessControlPopulator)
        string accessCode = PlayerPrefs.GetString("AccessCode", "");
        string workshopID = PlayerPrefs.GetString("WorkshopId", "");
        string playerID = PlayerPrefs.GetString("PlayerID", "");
        
        // Create player ID if not exists
        if (string.IsNullOrEmpty(playerID))
        {
            playerID = "player_" + UnityEngine.Random.Range(1000, 10000);
            PlayerPrefs.SetString("PlayerID", playerID);
            PlayerPrefs.Save();
        }
        
        // Get session ID
        string sessionID = PlayerPrefs.GetString("SessionID", "");
        if (string.IsNullOrEmpty(sessionID))
        {
            sessionID = System.Guid.NewGuid().ToString();
            PlayerPrefs.SetString("SessionID", sessionID);
            PlayerPrefs.Save();
        }
        
        // Create a basic entry for legacy support
        if (allChoices.Length > 0)
        {
            ChoiceDataEntry entry = new ChoiceDataEntry(
                playerID,
                accessCode,
                workshopID,
                sessionID,
                sceneID,
                questionText,
                allChoices[0],
                0,
                ""
            );
            
            dataQueue.Enqueue(entry);

            // Start processing queue if not already running
            if (!isSending)
            {
                StartCoroutine(ProcessQueue());
            }
        }
        
        // Also log locally
        Debug.Log($"Choices recorded for scene {sceneID}: Question '{questionText}'");
    }
    
    public void RecordPlayerChoice(StorySheetReader.DialogueSO dialogueSO, int choiceIndex)
    {
        if (dialogueSO == null)
        {
            Debug.LogError("DialogueSO is null. Cannot record player choice.");
            return;
        }
        
        // Get the question text and selected answer
        string questionText = dialogueSO.Lines;
        string selectedChoice = "";
        string nextSceneID = "";
        
        // Determine which choice was selected
        switch(choiceIndex) 
        {
            case 0:
                selectedChoice = dialogueSO.A1Answers;
                nextSceneID = dialogueSO.GoToIDA1s;
                break;
            case 1:
                selectedChoice = dialogueSO.A2Answers;
                nextSceneID = dialogueSO.GoToIDA2s;
                break;
            case 2:
                selectedChoice = dialogueSO.A3Answers;
                nextSceneID = dialogueSO.GoToIDA3s;
                break;
        }
        
        // Get player identifiers
        string accessCode = PlayerPrefs.GetString("AccessCode", "");
        string workshopID = PlayerPrefs.GetString("WorkshopId", "");
        string playerID = PlayerPrefs.GetString("PlayerID", "");
        
        // Create player ID if not exists
        if (string.IsNullOrEmpty(playerID))
        {
            playerID = "player_" + UnityEngine.Random.Range(1000, 10000);
            PlayerPrefs.SetString("PlayerID", playerID);
            PlayerPrefs.Save();
        }
        
        // Get session ID
        string sessionID = PlayerPrefs.GetString("SessionID", "");
        if (string.IsNullOrEmpty(sessionID))
        {
            sessionID = System.Guid.NewGuid().ToString();
            PlayerPrefs.SetString("SessionID", sessionID);
            PlayerPrefs.Save();
        }
        
        // Create the choice data entry
        ChoiceDataEntry entry = new ChoiceDataEntry(
            playerID,
            accessCode,
            workshopID,
            sessionID,
            dialogueSO.IDs,
            questionText,
            selectedChoice,
            choiceIndex,
            nextSceneID
        );
        
        dataQueue.Enqueue(entry);

        // Start processing queue if not already running
        if (!isSending)
        {
            StartCoroutine(ProcessQueue());
        }
        
        // Also log locally
        Debug.Log($"Choice recorded - Scene: {dialogueSO.IDs}, Question: '{questionText.Substring(0, Mathf.Min(30, questionText.Length))}...', Choice: {choiceIndex}");
    }

    private IEnumerator ProcessQueue()
    {
        isSending = true;

        while (dataQueue.Count > 0)
        {
            ChoiceDataEntry entry = dataQueue.Dequeue();
            yield return SendDataToGoogleSheet(entry);

            // Add a small delay between requests to avoid rate limiting
            yield return new WaitForSeconds(0.5f);
        }

        isSending = false;
    }

    private IEnumerator ProcessQueueSync()
    {
        while (dataQueue.Count > 0)
        {
            ChoiceDataEntry entry = dataQueue.Dequeue();
            
            UnityWebRequest www = CreateDataRequest(entry);
            www.SendWebRequest();
            
            // Wait for the request to complete
            while (!www.isDone)
            {
                yield return null;
            }
            
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error sending data to Google Sheet: " + www.error);
            }
            
            www.Dispose();
        }
    }

    private UnityWebRequest CreateDataRequest(ChoiceDataEntry entry)
    {
        WWWForm form = new WWWForm();
        
        // Add player and session identifiers
        form.AddField("playerID", entry.playerID);
        form.AddField("accessCode", entry.accessCode);
        form.AddField("workshopID", entry.workshopID);
        form.AddField("timestamp", entry.timestamp);
        form.AddField("sessionID", entry.sessionID);
        
        // Add choice data
        form.AddField("currentSceneID", entry.currentSceneID);
        form.AddField("questionText", entry.questionText);
        form.AddField("selectedChoice", entry.selectedChoice);
        form.AddField("choiceIndex", entry.choiceIndex.ToString());
        form.AddField("nextSceneID", entry.nextSceneID);
        
        return UnityWebRequest.Post(webAppURL, form);
    }

    private IEnumerator SendDataToGoogleSheet(ChoiceDataEntry entry)
    {
        if (string.IsNullOrEmpty(webAppURL))
        {
            Debug.LogError("Google Apps Script Web App URL is not set!");
            yield break;
        }

        UnityWebRequest www = CreateDataRequest(entry);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error sending data to Google Sheet: " + www.error);
            // Re-queue the data for another attempt
            dataQueue.Enqueue(entry);
        }
        else
        {
            Debug.Log("Successfully sent choice data to Google Sheet!");
        }

        www.Dispose();
    }
}