using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintDialogueSO : MonoBehaviour
{
    public StorySheetReaderTwo storySheetReader; // Reference to your StorySheetReaderTwo script

    void Start()
    {
        if (storySheetReader != null)
        {
            storySheetReader.onDataLoaded += PrintDialogues;
            storySheetReader.StartCoroutine(storySheetReader.ObtainSheetData());
        }
        else
        {
            Debug.LogError("StorySheetReaderTwo is not assigned.");
        }
    }

    void PrintDialogues()
    {
        if (storySheetReader.dialogues != null && storySheetReader.dialogues.Count > 0)
        {
            foreach (var dialogue in storySheetReader.dialogues)
            {
                Debug.Log($"ID: {dialogue.IDs}, Speaker: {dialogue.Speakers}, Line: {dialogue.Lines}, Keywords: {dialogue.Keywords}");
            }
        }
        else
        {
            Debug.LogError("Dialogues list is empty or null.");
        }
    }
}