using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEngine.SceneManagement;

public class TitleScreenPopulator : MonoBehaviour
{
    private TextField Accesscode;
    private TextField Workshopid;
    private Button proceed;
    
    public List<LoginSO> logins;

    public CCSVtoSO ccsvToSO;

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        Accesscode = root.Q<TextField>("AccessCodeTF");
        Workshopid = root.Q<TextField>("WorkshopIDTF");
        proceed = root.Q<Button>("Proceed");

        string accessCodeValue = Accesscode.value;
        string workshopIdValue = Workshopid.value;

        proceed.RegisterCallback<ClickEvent>(nextScene);

        

        //int counter = ccsvToSO.logins.Count;
        //Debug.Log(counter);
    }

   private void nextScene(ClickEvent evt)
   
{   
    int currentIndex = 0;
    string accessCodeValue = Accesscode.value;
    string workshopIdValue = Workshopid.value;
    string sceneToLoad = "TestZone";
    Boolean match = false;

    while (currentIndex < ccsvToSO.logins.Count)
    {
        var LoginSO = ccsvToSO.logins[currentIndex];
        Debug.Log(currentIndex);
        Debug.Log("Accesscode value: " + accessCodeValue);
        Debug.Log("Workshopid value: " + workshopIdValue);
        Debug.Log("LoginSO.AccessCodes: " + LoginSO.AccessCodes);
        Debug.Log("LoginSO.WorkshopIDCodes: " + LoginSO.WorkshopIDCodes);
        if  (accessCodeValue.Equals(LoginSO.AccessCodes) && workshopIdValue.Equals(LoginSO.WorkshopIDCodes))
        {
            Debug.Log("Codes match!");
            match = true;
            SceneManager.LoadScene(sceneToLoad);
            break;
        }
        else
        {
           currentIndex++; 
        }
    }

    if  (match == false)
    {
        Debug.Log("Codes do not match!");
    }
    
}
    void Update()
    {
        
    }
}
