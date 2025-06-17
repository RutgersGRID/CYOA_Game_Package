using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class TWPopulator : MonoBehaviour
{
    public TriggerWarningReader TWR; // Reference to TriggerWarningReader
    private Button nextButton;
    private TextElement contentWarningBody; // Reference to the text element
    public string sceneToLoad;
    
    void Start()
    {
        try
        {
            // Check for UIDocument component
            var uiDocument = GetComponent<UIDocument>();
            if (uiDocument == null)
            {
                Debug.LogError("UIDocument component not found on the GameObject");
                return;
            }
            
            var root = uiDocument.rootVisualElement;
            if (root == null)
            {
                Debug.LogError("Root VisualElement is null");
                return;
            }
            
            // Make sure TWR reference exists
            if (TWR == null)
            {
                Debug.LogError("TriggerWarningReader (TWR) reference is not set");
                return;
            }
            
            // Find UI elements with null checks
            nextButton = root.Q<Button>("Content_Warning_Button");
            if (nextButton == null) Debug.LogError("Content_Warning_Button not found in UI Document");
            
            contentWarningBody = root.Q<TextElement>("Content_Warning_Body");
            if (contentWarningBody == null) Debug.LogError("Content_Warning_Body not found in UI Document");
            
            // Only register callback if button was found
            if (nextButton != null)
            {
                nextButton.RegisterCallback<ClickEvent>(nextScene);
            }
            
            // Subscribe to the data loaded event
            TWR.onDataLoaded += OnDataLoaded;
            
            // If data is already loaded, update UI immediately
            if (TWR.triggerWarnings.Count > 0)
            {
                LoadTriggerWarningText();
            }
            
            Debug.Log("TWPopulator initialization completed successfully");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error in TWPopulator.Start(): {e.Message}\n{e.StackTrace}");
        }
    }
    
    private void OnDataLoaded()
    {
        // This will be called when the data is loaded from Google Sheets
        Debug.Log("Trigger warning data loaded. Loading UI elements...");
        LoadTriggerWarningText();
    }
    
    private void nextScene(ClickEvent evt)
    {
        SceneManager.LoadScene(sceneToLoad);
    }
    
    private void LoadTriggerWarningText()
    {
        try
        {
            if (TWR == null)
            {
                Debug.LogError("TriggerWarningReader (TWR) reference is null");
                return;
            }
            
            if (TWR.triggerWarnings.Count > 0)
            {
                // Use the first trigger warning text (index 0)
                var triggerWarningSO = TWR.triggerWarnings[0];
                
                // Check if UI element exists before setting text
                if (contentWarningBody != null)
                {
                    contentWarningBody.text = triggerWarningSO.TWText;
                    Debug.Log($"Loaded trigger warning text: {triggerWarningSO.TWText.Substring(0, Mathf.Min(100, triggerWarningSO.TWText.Length))}...");
                }
                else
                {
                    Debug.LogError("contentWarningBody element is null");
                }
            }
            else
            {
                Debug.LogError($"No trigger warnings available to load. Count: {TWR.triggerWarnings.Count}");
                
                // Set fallback text if no data is loaded
                if (contentWarningBody != null)
                {
                    contentWarningBody.text = "This game may include content that some participants may find difficult. Please proceed with caution.";
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error in LoadTriggerWarningText(): {e.Message}\n{e.StackTrace}");
        }
    }
    
    private void OnDestroy()
    {
        // Unsubscribe from the event when component is destroyed
        if (TWR != null)
        {
            TWR.onDataLoaded -= OnDataLoaded;
        }
    }
}