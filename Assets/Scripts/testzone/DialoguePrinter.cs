using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class DialoguePrinter : MonoBehaviour
{
    public StorySheetReader SSR;

    private void Start()
    {
        PrintDialogues();
    }

    private void Awake()
    {
        PrintDialogues();
    }

    public void PrintDialogues()
    {
        if (SSR == null) 
        {
            Debug.LogError("SSR is not set in DialoguePrinter!");
            return;
        }

        if (SSR.dialogues == null || SSR.dialogues.Count == 0) 
        {
            Debug.LogError("Dialogues list is null or empty!");
            return;
        }

        foreach (var dialogue in SSR.dialogues)
        {
            Debug.Log("ID: " + dialogue.ID);
            Debug.Log("Speaker: " + dialogue.Speaker);
            Debug.Log("Line: " + dialogue.Line);
            Debug.Log("Left Side Speaker: " + (dialogue.LeftSideSpeaker ? dialogue.LeftSideSpeaker.name : "None"));
            Debug.Log("Right Side Speaker: " + (dialogue.RightSideSpeaker ? dialogue.RightSideSpeaker.name : "None"));
            Debug.Log("Prop: " + (dialogue.Prop ? dialogue.Prop.name : "None"));
            Debug.Log("Background: " + (dialogue.Background ? dialogue.Background.name : "None"));
            Debug.Log("Checkpoint: " + dialogue.Checkpoint);
            Debug.Log("Type: " + dialogue.Type);
            Debug.Log("GoToID: " + dialogue.GoToID);
            Debug.Log("Effect: " + dialogue.Effect);
            Debug.Log("A1 Answer: " + dialogue.A1Answer);
            Debug.Log("GoToID A1: " + dialogue.GoToIDA1);
            Debug.Log("Effect A1: " + dialogue.EffectA1);
            Debug.Log("A2 Answer: " + dialogue.A2Answer);
            Debug.Log("GoToID A2: " + dialogue.GoToIDA2);
            Debug.Log("Effect A2: " + dialogue.EffectA2);
            Debug.Log("A3 Answer: " + dialogue.A3Answer);
            Debug.Log("GoToID A3: " + dialogue.GoToIDA3);
            Debug.Log("Effect A3: " + dialogue.EffectA3);

            Debug.Log("-----------------------------------------"); // For better separation between dialogues
        }
    }
}