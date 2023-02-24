using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DialoguePopulator : MonoBehaviour
{
    //public CSVtoSO csvToSO;
    public CSVtoSOTwo csvToSOTwo;
    private int currentIndex = 0;
    //private int destinationID = 0;
    private VisualElement dlogElements;
    private VisualElement dlogBG;
    private VisualElement propImage;
    private VisualElement characterImageLeft;
    private VisualElement characterImageRight;
    private VisualElement rewindUI;
    private Button rewind;
    private Button rewindYes;
    private Button rewindNo;
    private Button journal;
    ///

    ///
    private VisualElement DBox;
    private TextElement nameText;
    private TextElement dialogueText;
    private Button nextButton;
    ///
    private VisualElement twoOptionAnswers;
    private TextElement questionTwoAnswers;
    private TextElement textATwo;
    private Button aTwo;
    private TextElement textBTwo;
    private Button bTwo;
    ///
    private VisualElement threeOptionAnswers;
    private TextElement questionThreeAnswers;
    private TextElement textAThree;
    private Button aThree;
    private TextElement textBThree;
    private Button bThree;
    private TextElement textCThree;
    private Button cThree;

    private void OnEnable() 
    {
        // var root = GetComponent<UIDocument>().rootVisualElement;
        // //var root = baseUI.rootVisualElement;
        // dlogElements = root.Q<VisualElement>("dlog-elements");
        // dlogBG = root.Q<VisualElement>("dlog-bg");
        // characterImageLeft = root.Q<VisualElement>("CharacterLeft");
        // characterImageRight = root.Q<VisualElement>("CharacterRight");
        // propImage = root.Q<VisualElement>("Props");
        // rewindUI = root.Q<VisualElement>("Rewind");
        // rewindUI.style.display = DisplayStyle.None;

        // rewindYes = root.Q<Button>("rewind-yes");
        // rewindNo = root.Q<Button>("rewind-no");
        // Debug.Log("dlogElements - rewindNo made");
        // ///
        // DBox = root.Q<VisualElement>("DBox");
        // nameText = root.Q<TextElement>("DialogueText");
        // dialogueText = root.Q<TextElement>("DialogueText");
        // nextButton = root.Q<Button>("dlog-button");
        // Debug.Log("DBox - nextButton made");
        // ///
        // twoOptionAnswers = root.Q<VisualElement>("answers-two-options-bg");
        // questionTwoAnswers = root.Q<TextElement>("question-text-two");
        // textATwo = root.Q<TextElement>("text-a-two");
        // aTwo = root.Q<Button>("button-a-two");
        // textBTwo = root.Q<TextElement>("text-b-two");
        // bTwo = root.Q<Button>("button-b-two");
        // Debug.Log("twoOptionAnswers - bTwo made");
        // ///
        // threeOptionAnswers =root.Q<VisualElement>("answers-three-options-bg");
        // questionThreeAnswers = root.Q<TextElement>("question-text-three");
        // textAThree = root.Q<TextElement>("text-a-three");
        // aThree = root.Q<Button>("button-a-three");
        // textBThree = root.Q<TextElement>("text-b-three");
        // bThree = root.Q<Button>("button-c-three");
        // textCThree = root.Q<TextElement>("text-c-three");
        // cThree = root.Q<Button>("button-c-three");
        // Debug.Log("threeOptionAnswers - cThree made");
        // ///
        // nextButton.RegisterCallback<ClickEvent>(NextDialogue);
        // aTwo.RegisterCallback<ClickEvent>(NextDialogueA);
        // bTwo.RegisterCallback<ClickEvent>(NextDialogueB);
        // aThree.RegisterCallback<ClickEvent>(NextDialogueA);
        // bThree.RegisterCallback<ClickEvent>(NextDialogueB);
        // cThree.RegisterCallback<ClickEvent>(NextDialogueC);
        // Debug.Log("nextButton - cThree clickevent made");
    }
    private void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        //var root = baseUI.rootVisualElement;
        dlogElements = root.Q<VisualElement>("dlog-elements");
        dlogBG = root.Q<VisualElement>("dlog-bg");
        characterImageLeft = root.Q<VisualElement>("CharacterLeft");
        characterImageRight = root.Q<VisualElement>("CharacterRight");
        propImage = root.Q<VisualElement>("Props");
        rewindUI = root.Q<VisualElement>("Rewind");

        rewindUI.style.display = DisplayStyle.None;
        rewindYes = root.Q<Button>("rewind-yes");
        rewindNo = root.Q<Button>("rewind-no");
        Debug.Log(root);
        ///
        DBox = root.Q<VisualElement>("DBox");
        nameText = root.Q<TextElement>("DialogueText");
        dialogueText = root.Q<TextElement>("DialogueText");
        nextButton = root.Q<Button>("dlog-button");
        Debug.Log("DBox - nextButton made");
        ///
        twoOptionAnswers = root.Q<VisualElement>("answer-two-options-bg");
        questionTwoAnswers = root.Q<TextElement>("question-text-two");
        textATwo = root.Q<TextElement>("text-a-two");
        aTwo = root.Q<Button>("button-a-two");
        textBTwo = root.Q<TextElement>("text-b-two");
        bTwo = root.Q<Button>("button-b-two");

        twoOptionAnswers.style.display = DisplayStyle.None;
        Debug.Log("twoOptionAnswers - bTwo made");
        ///
        threeOptionAnswers = root.Q<VisualElement>("answer-three-options-bg");
        questionThreeAnswers = root.Q<TextElement>("question-text-three");
        textAThree = root.Q<TextElement>("text-a-three");
        aThree = root.Q<Button>("button-a-three");
        textBThree = root.Q<TextElement>("text-b-three");
        bThree = root.Q<Button>("button-c-three");
        textCThree = root.Q<TextElement>("text-c-three");
        cThree = root.Q<Button>("button-c-three");

        threeOptionAnswers.style.display = DisplayStyle.None;
        Debug.Log("threeOptionAnswers - cThree made");
        ///
        nextButton.RegisterCallback<ClickEvent>(NextDialogue);
        aTwo.RegisterCallback<ClickEvent>(NextDialogueA);
        bTwo.RegisterCallback<ClickEvent>(NextDialogueB);
        aThree.RegisterCallback<ClickEvent>(NextDialogueA);
        bThree.RegisterCallback<ClickEvent>(NextDialogueB);
        cThree.RegisterCallback<ClickEvent>(NextDialogueC);
        PopulateUI();
    }

    private void PopulateUI()
    {
        //var dialogueSO = csvToSO.dialogues[currentIndex];
        var dialogueSO = csvToSOTwo.dialogues[currentIndex];
        if (dialogueSO.Type.Equals("a", StringComparison.CurrentCultureIgnoreCase) == true)
        {
            dlogBG.style.display = DisplayStyle.Flex;
            DBox.style.display = DisplayStyle.Flex;
            twoOptionAnswers.style.display = DisplayStyle.None;
            threeOptionAnswers.style.display = DisplayStyle.None;

            dlogBG.style.backgroundImage = new StyleBackground(dialogueSO.Background);
            characterImageLeft.style.backgroundImage = new StyleBackground(dialogueSO.LeftSideSpeaker);
            characterImageRight.style.backgroundImage = new StyleBackground(dialogueSO.RightSideSpeaker);
            propImage.style.backgroundImage = new StyleBackground(dialogueSO.Prop);

            nameText.text = dialogueSO.Speaker;
            dialogueText.text = dialogueSO.Line;
        }
        else if (dialogueSO.Type.Equals("b", StringComparison.CurrentCultureIgnoreCase) == true)
        {
            dlogBG.style.display = DisplayStyle.Flex;
            DBox.style.display = DisplayStyle.None;
            twoOptionAnswers.style.display = DisplayStyle.Flex;
            threeOptionAnswers.style.display = DisplayStyle.None;

            dlogBG.style.backgroundImage = new StyleBackground(dialogueSO.Background);
            characterImageLeft.style.backgroundImage = new StyleBackground(dialogueSO.LeftSideSpeaker);
            characterImageRight.style.backgroundImage = new StyleBackground(dialogueSO.RightSideSpeaker);
            propImage.style.backgroundImage = new StyleBackground(dialogueSO.Prop);

            questionTwoAnswers.text = dialogueSO.Line;
            textATwo.text = dialogueSO.A1Answer;
            textBTwo.text = dialogueSO.A2Answer;
        }
        else if (dialogueSO.Type.Equals("c", StringComparison.CurrentCultureIgnoreCase) == true)
        {
            dlogBG.style.display = DisplayStyle.Flex;
            DBox.style.display = DisplayStyle.None;
            twoOptionAnswers.style.display = DisplayStyle.None;
            threeOptionAnswers.style.display = DisplayStyle.Flex;

            dlogBG.style.backgroundImage = new StyleBackground(dialogueSO.Background);
            characterImageLeft.style.backgroundImage = new StyleBackground(dialogueSO.LeftSideSpeaker);
            characterImageRight.style.backgroundImage = new StyleBackground(dialogueSO.RightSideSpeaker);
            propImage.style.backgroundImage = new StyleBackground(dialogueSO.Prop);

            questionThreeAnswers.text = dialogueSO.Line;
            textAThree.text = dialogueSO.A1Answer;
            textBThree.text = dialogueSO.A2Answer;
            textCThree.text = dialogueSO.A3Answer;
        }     
    }

    private void NextDialogue(ClickEvent evt)
    {
        var dialogueSO = csvToSOTwo.dialogues[currentIndex];
        currentIndex = dialogueSO.GoToID;
        PopulateUI();
    }
    private void NextDialogueA(ClickEvent evt)
    {
        var dialogueSO = csvToSOTwo.dialogues[currentIndex];
        currentIndex = dialogueSO.GoToIDA1;
        PopulateUI();
    }
    private void NextDialogueB(ClickEvent evt)
    {
        var dialogueSO = csvToSOTwo.dialogues[currentIndex];
        currentIndex = dialogueSO.GoToIDA2;
        PopulateUI();
    }
    private void NextDialogueC(ClickEvent evt)
    {
        var dialogueSO = csvToSOTwo.dialogues[currentIndex];
        currentIndex = dialogueSO.GoToIDA3;
        PopulateUI();
    }
}


