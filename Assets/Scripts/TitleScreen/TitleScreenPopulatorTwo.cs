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

    public GoogleSheetReaderTwo gsrt;

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        Accesscode = root.Q<TextField>("AccessCodeTF");
        Workshopid = root.Q<TextField>("WorkshopIDTF");
        proceed = root.Q<Button>("Proceed");

        string accessCodeValue = Accesscode.value;
        string workshopIdValue = Workshopid.value;

        proceed.RegisterCallback<ClickEvent>(nextScene);
    }

    private void nextScene(ClickEvent evt)
    {   
        string accessCodeValue = Accesscode.value;
        string workshopIdValue = Workshopid.value;
        string sceneToLoad = "TestZone";
        Boolean match = false;

        for(int currentIndex = 0; currentIndex < gsrt.logins.Count; currentIndex++)
        {
            var LoginSO = gsrt.logins[currentIndex];
            
            Debug.Log(currentIndex);
            Debug.Log("Accesscode value: " + accessCodeValue);
            Debug.Log("Workshopid value: " + workshopIdValue);
            Debug.Log("LoginSO.AccessCodes: " + LoginSO.AccessCodes);
            Debug.Log("LoginSO.WorkshopIDCodes: " + LoginSO.WorkshopIDCodes);
            
            if (accessCodeValue.Equals(LoginSO.AccessCodes) && workshopIdValue.Equals(LoginSO.WorkshopIDCodes))
            {
                Debug.Log("Codes match!");
                match = true;
                //SceneManager.LoadScene(sceneToLoad);
                break;
            }
        }

        if (!match)
        {
            Debug.Log("Codes do not match!");
        }
    }

    void Update()
    {
        
    }
}
