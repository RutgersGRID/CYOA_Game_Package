using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class TitleScreenPopulator : MonoBehaviour
{
    public TitleScreenReader TSR;
    int currentIndex = 0;
    private Button nextButton;
    private VisualElement gameTitleImage;
    private TextElement gameTitle;
    private TextElement presentsTitle;
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
            
            // Make sure TSR reference exists
            if (TSR == null)
            {
                Debug.LogError("TitleScreenReader (TSR) reference is not set");
                return;
            }
            
            // Find UI elements with null checks
            nextButton = root.Q<Button>("TTButton");
            if (nextButton == null) Debug.LogError("TTButton not found in UI Document");
            
            gameTitleImage = root.Q<VisualElement>("Titlescreen_Image");
            if (gameTitleImage == null) Debug.LogError("Titlescreen_Image not found in UI Document");
            
            gameTitle = root.Q<TextElement>("Titlescreen_Title");
            if (gameTitle == null) Debug.LogError("Titlescreen_Title not found in UI Document");
            
            presentsTitle = root.Q<TextElement>("Titlescreen_Presents");
            if (presentsTitle == null) Debug.LogError("Titlescreen_Presents not found in UI Document");
            
            // Only register callback if button was found
            if (nextButton != null)
            {
                nextButton.RegisterCallback<ClickEvent>(nextScene);
            }
            
            // Subscribe to the data loaded event
            TSR.onDataLoaded += OnDataLoaded;
            
            // If data is already loaded, update UI immediately
            if (TSR.titlescreens.Count > 0)
            {
                loadTitleScreen();
            }
            
            Debug.Log("TitleScreenPopulator initialization completed successfully");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error in TitleScreenPopulator.Start(): {e.Message}\n{e.StackTrace}");
        }
    }
    
    private void OnDataLoaded()
    {
        // This will be called when the data is loaded from Google Sheets
        Debug.Log("Title screen data loaded. Loading UI elements...");
        loadTitleScreen();
    }
    
    private void nextScene(ClickEvent evt)
    {
        SceneManager.LoadScene(sceneToLoad);
    }
    
    private void preloadTheList()
    {
        foreach (var titlescreen in TSR.titlescreens)
        {
            // Preload frames if needed
        }
    }
    
    private void loadTitleScreen()
    {
        try
        {
            if (TSR == null)
            {
                Debug.LogError("TitleScreenReader (TSR) reference is null");
                return;
            }
            
            if (TSR.titlescreens.Count > 0 && currentIndex < TSR.titlescreens.Count)
            {
                var TitleScreenSO = TSR.titlescreens[currentIndex];
                
                // Check if UI elements exist before setting properties
                if (gameTitle != null)
                    gameTitle.text = TitleScreenSO.Titles;
                else
                    Debug.LogError("gameTitle element is null");
                    
                if (presentsTitle != null)
                    presentsTitle.text = TitleScreenSO.Presents;
                else
                    Debug.LogError("presentsTitle element is null");
                
                if (gameTitleImage != null && TitleScreenSO.TitleImages != null)
                    gameTitleImage.style.backgroundImage = new StyleBackground(TitleScreenSO.TitleImages);
                else
                    Debug.LogError($"gameTitleImage is null: {gameTitleImage == null}, TitleImages is null: {TitleScreenSO.TitleImages == null}");
                
                Debug.Log($"Loaded title screen: {TitleScreenSO.Titles}, {TitleScreenSO.Presents}");
            }
            else
            {
                Debug.LogError($"No title screens available to load or index out of range. Count: {TSR.titlescreens.Count}, Index: {currentIndex}");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error in loadTitleScreen(): {e.Message}\n{e.StackTrace}");
        }
    }
    
    private void OnDestroy()
    {
        // Unsubscribe from the event when component is destroyed
        if (TSR != null)
        {
            TSR.onDataLoaded -= OnDataLoaded;
        }
    }
}