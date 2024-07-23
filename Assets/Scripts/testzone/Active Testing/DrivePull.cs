using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Networking;

public class DrivePull : MonoBehaviour
{
    public StorySheetReaderTester SSR;
    public string googleDriveURL = "https://drive.google.com/uc?export=download&id=FILE_ID";
    public VisualElement imageSprite;
    public Button nextSprite;
    int currentIndex = 0;

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        imageSprite = root.Q<VisualElement>("TestSprite");
        nextSprite = root.Q<Button>("NextPic");

        nextSprite.RegisterCallback<ClickEvent>(ShowNext);

       
    }

    private void PopulateTestUI()
    {
        if (currentIndex >= 0 && currentIndex < SSR.dialogues.Count) 
        {
            var dialogueSO = SSR.dialogues[currentIndex];

            ////
            var currentimageSprite = imageSprite.style.backgroundImage;

            // Update the background images based on dialogueSO
            imageSprite.style.backgroundImage = new StyleBackground(dialogueSO.LeftSideSpeakers);

        }
    }
    private void ShowNext(ClickEvent evt)
    {
        Debug.Log($"CurrentIndex before change: {currentIndex}");
        if (currentIndex < 0 || currentIndex >= SSR.dialogues.Count)
        {
            Debug.LogError("Current index is out of range!");
            return;
        }

        var dialogueSO = SSR.dialogues[currentIndex];
        string nextID = dialogueSO.GoToIDs;

        // Find the index of the next dialogue using the string ID
        currentIndex = SSR.dialogues.FindIndex(d => d.IDs == nextID);

        // Check if the index was found
        if (currentIndex == -1)
        {
            Debug.LogError($"GoToID {nextID} not found!");
            return; // Exit function early
        }

        PopulateTestUI();
    }
}
