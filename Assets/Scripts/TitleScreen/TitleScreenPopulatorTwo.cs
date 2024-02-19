using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class TitleScreenPopulatorTwo : MonoBehaviour
{
    private TextField Accesscode;
    private TextField Workshopid;
    private Button proceed;
    public string sceneToLoad;
    public GoogleSheetReaderTwo gsrt;

    void Start()
    {
        Debug.Log("TitleScreenPopulatorTwo started. Subscribing to event...");
        var root = GetComponent<UIDocument>().rootVisualElement;
        Accesscode = root.Q<TextField>("AccessCodeTF");
        Workshopid = root.Q<TextField>("WorkshopIDTF");
        proceed = root.Q<Button>("Proceed");

        // Subscribe to the onDataLoaded event
        proceed.RegisterCallback<ClickEvent>(nextScene);
        gsrt.onDataLoaded += DataLoadedCallback;
        gsrt.StartCoroutine(gsrt.ObtainSheetData());
    }

    void DataLoadedCallback()
    {
        Debug.Log("Data loaded and list populated.");
        Debug.Log("Size of gsrt.dialogues: " + gsrt.logins.Count);
        preloadTheList();
    }

        private void preloadTheList()
    {
        foreach (var login in gsrt.logins)
        {

        }
    }
    private void nextScene(ClickEvent evt)
    {   
        Debug.Log("Size of gsrt.dialogues: " + gsrt.logins.Count);
        string accessCodeValue = Accesscode.value;
        string workshopIdValue = Workshopid.value;
        //string sceneToLoad = "TestZone";
        Boolean match = false;

        for(int currentIndex = 0; currentIndex < gsrt.logins.Count; currentIndex++)
        {
            var LoginSO = gsrt.logins[currentIndex];
            
            Debug.Log(currentIndex);
            // Debug.Log("Accesscode value: " + accessCodeValue);
            // Debug.Log("Workshopid value: " + workshopIdValue);
            Debug.Log("LoginSO.AccessCodes: " + LoginSO.AccessCodes);
            Debug.Log("LoginSO.WorkshopIDCodes: " + LoginSO.WorkshopIDCodes);
            // Debug.Log("LoginSO.TestSpaces: " + LoginSO.TestSpaces);

            Debug.Log("Size of gsrt.dialogues: " + gsrt.logins.Count);
            
            if (accessCodeValue.Equals(LoginSO.AccessCodes) && workshopIdValue.Equals(LoginSO.WorkshopIDCodes))
            {
                Debug.Log("Codes match!");
                match = true;
                PlayerPrefs.SetString("AccessCode", accessCodeValue);
                PlayerPrefs.SetString("WorkshopId", workshopIdValue);
                SceneManager.LoadScene(sceneToLoad);
                break;
            }
            else
            {
                Debug.Log("Codes did not match: " + accessCodeValue + " and " + workshopIdValue);
            }
        }

        if (!match)
        {
            Debug.Log("Codes do not match!");
        }
    }

    void OnDestroy()
    {
        gsrt.onDataLoaded -= DataLoadedCallback;
    }
}
