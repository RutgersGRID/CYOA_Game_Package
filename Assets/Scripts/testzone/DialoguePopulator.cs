using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DialoguePopulator : MonoBehaviour
{
    public CSVtoSO csvToSO;
    private int currentIndex = 0;
    //private int destinationID = 0;
    private VisualElement dlogElements;
    private VisualElement dlogBG;
    private VisualElement propImage;
    private VisualElement characterImageLeft;
    private VisualElement characterImageRight;
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

    private void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        //var root = baseUI.rootVisualElement;
        dlogElements = root.Q<VisualElement>("dlog-elements");
        dlogBG = root.Q<VisualElement>("dlog-bg");
        characterImageLeft = root.Q<VisualElement>("CharacterLeft");
        characterImageRight = root.Q<VisualElement>("CharacterRight");
        propImage = root.Q<VisualElement>("Props");
        rewindYes = root.Q<Button>("rewind-yes");
        rewindNo = root.Q<Button>("rewind-no");
        Debug.Log("dlogElements - rewindNo made");
        ///
        DBox = root.Q<VisualElement>("DBox");
        nameText = root.Q<TextElement>("DialogueText");
        dialogueText = root.Q<TextElement>("DialogueText");
        nextButton = root.Q<Button>("dlog-button");
        Debug.Log("DBox - nextButton made");
        ///
        twoOptionAnswers = root.Q<VisualElement>("answers-two-options-bg");
        questionTwoAnswers = root.Q<TextElement>("question-text-two");
        textATwo = root.Q<TextElement>("text-a-two");
        aTwo = root.Q<Button>("button-a-two");
        textBTwo = root.Q<TextElement>("text-b-two");
        bTwo = root.Q<Button>("button-b-two");
        Debug.Log("twoOptionAnswers - bTwo made");
        ///
        threeOptionAnswers =root.Q<VisualElement>("answers-three-options-bg");
        questionThreeAnswers = root.Q<TextElement>("question-text-three");
        textAThree = root.Q<TextElement>("text-a-three");
        aThree = root.Q<Button>("button-a-three");
        textBThree = root.Q<TextElement>("text-b-three");
        bThree = root.Q<Button>("button-c-three");
        textCThree = root.Q<TextElement>("text-c-three");
        cThree = root.Q<Button>("button-c-three");
        Debug.Log("threeOptionAnswers - cThree made");
        ///
        nextButton.RegisterCallback<ClickEvent>(NextDialogue);
        aTwo.RegisterCallback<ClickEvent>(NextDialogueA);
        bTwo.RegisterCallback<ClickEvent>(NextDialogueB);
        aThree.RegisterCallback<ClickEvent>(NextDialogueA);
        bThree.RegisterCallback<ClickEvent>(NextDialogueB);
        cThree.RegisterCallback<ClickEvent>(NextDialogueC);
        Debug.Log("nextButton - cThree clickevent made");
        //var root2 = rewindDocument.rootVisualElement;
        ///
        // nameText = root.Q<TextElement>("DialogueName");
        // if (nameText == null)
        // {
        //     Debug.LogError("Could not find nameText element");
        //     return;
        // }

        // dialogueText = root.Q<TextElement>("DialogueText");
        // if (dialogueText == null)
        // {
        //     Debug.LogError("Could not find dialogueText element");
        //     return;
        // }

        // propImage = root.Q<VisualElement>("Props");
        // if (propImage == null)
        // {
        //     Debug.LogError("Could not find propImage element");
        //     return;
        // }

        // characterImage = root.Q<VisualElement>("CharacterRight");
        // if (characterImage == null)
        // {
        //     Debug.LogError("Could not find characterImage element");
        //     return;
        // }

        // nextButton = root.Q<Button>("dlog-button");
        // if (nextButton == null)
        // {
        //     Debug.LogError("Could not find nextButton element");
        //     return;
        // }

        // nextButton.RegisterCallback<ClickEvent>(NextCharacter);
        PopulateUI();
    }

    private void PopulateUI()
    {
        var dialogueSO = csvToSO.dialogues[currentIndex];
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
        // var character = csvToSO.characters[currentIndex];
        // nameText.text = character.Character_Name;
        // dialogueText.text = character.Character_Dialogue;
        // Debug.Log("Loading prop sprite: " + character.Prop_Sprite);
        // Debug.Log("Loading character sprite: " + character.Character_Sprite);
        // propImage.style.backgroundImage = new StyleBackground(character.Prop_Sprite);
        // characterImage.style.backgroundImage = new StyleBackground(character.Character_Sprite);       
    }

    private void NextDialogue(ClickEvent evt)
    {
        var dialogueSO = csvToSO.dialogues[currentIndex];
        // currentIndex = (currentIndex + 1) % csvToSO.characters.Count;
        currentIndex = dialogueSO.GoToID;
        // change currentIndex to be the currentIndex GoToID
        PopulateUI();
    }
    private void NextDialogueA(ClickEvent evt)
    {
        var dialogueSO = csvToSO.dialogues[currentIndex];
        // currentIndex = (currentIndex + 1) % csvToSO.characters.Count;
        currentIndex = dialogueSO.GoToIDA1;
        // change currentIndex to be the currentIndex GoToID
        PopulateUI();
    }
    private void NextDialogueB(ClickEvent evt)
    {
        var dialogueSO = csvToSO.dialogues[currentIndex];
        // currentIndex = (currentIndex + 1) % csvToSO.characters.Count;
        currentIndex = dialogueSO.GoToIDA2;
        // change currentIndex to be the currentIndex GoToID
        PopulateUI();
    }
    private void NextDialogueC(ClickEvent evt)
    {
        var dialogueSO = csvToSO.dialogues[currentIndex];
        // currentIndex = (currentIndex + 1) % csvToSO.characters.Count;
        currentIndex = dialogueSO.GoToIDA3;
        // change currentIndex to be the currentIndex GoToID
        PopulateUI();
    }

    // private Sprite LoadSprite(string spriteName)
    // {
    //     return Resources.Load<Sprite>(spriteName);
    // }
}
