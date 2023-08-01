using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class TestScriptFade : MonoBehaviour
{
    public DCSVtoSO dcsvToSO;
    public JCSVtoSO jcsvToSO;
    int currentIndex = 0;
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
    string testText;
    public float typingSpeed = 0.05f;
    ///
    
 private void Start()
    {
        
        var root = GetComponent<UIDocument>().rootVisualElement;
        dlogElements = root.Q<VisualElement>("dlog-elements");
        dlogBG = root.Q<VisualElement>("dlog-bg");
        characterImageLeft = root.Q<VisualElement>("CharacterLeft");
        characterImageRight = root.Q<VisualElement>("CharacterRight");
        propImage = root.Q<VisualElement>("Props");
        rewindUI = root.Q<VisualElement>("Rewind");

        DBox = root.Q<VisualElement>("DBox");
        nameText = root.Q<TextElement>("DialogueName");
        dialogueText = root.Q<TextElement>("DialogueText");

        rewind = root.Q<Button>("rewind-button");
        rewindYes = root.Q<Button>("rewind-yes");
        rewindNo = root.Q<Button>("rewind-no");
        nextButton = root.Q<Button>("dlog-button");

        twoOptionAnswers = root.Q<VisualElement>("answer-two-options-bg");
        questionTwoAnswers = root.Q<TextElement>("question-text-two");
        aTwo = root.Q<Button>("button-a-two");
        bTwo = root.Q<Button>("button-b-two");

        threeOptionAnswers = root.Q<VisualElement>("answer-three-options-bg");
        questionThreeAnswers = root.Q<TextElement>("question-text-three");
        aThree = root.Q<Button>("button-a-three");
        bThree = root.Q<Button>("button-b-three");
        cThree = root.Q<Button>("button-c-three");

        journal = root.Q<Button>("journal");

        nextButton.RegisterCallback<ClickEvent>(NextDialogue);
        aTwo.RegisterCallback<ClickEvent>(NextDialogueA);
        bTwo.RegisterCallback<ClickEvent>(NextDialogueB);
        aThree.RegisterCallback<ClickEvent>(NextDialogueA);
        bThree.RegisterCallback<ClickEvent>(NextDialogueB);
        cThree.RegisterCallback<ClickEvent>(NextDialogueC);

        rewindUI.style.display = DisplayStyle.None;
        twoOptionAnswers.style.display = DisplayStyle.None;
        threeOptionAnswers.style.display = DisplayStyle.None;
    }
    private void PopulateUI()
{
    var dialogueSO = dcsvToSO.dialogues[currentIndex];

    
    dlogBG.style.backgroundImage = new StyleBackground(dialogueSO.Background);
    characterImageLeft.style.backgroundImage = new StyleBackground(dialogueSO.LeftSideSpeaker);
    characterImageRight.style.backgroundImage = new StyleBackground(dialogueSO.RightSideSpeaker);
    propImage.style.backgroundImage = new StyleBackground(dialogueSO.Prop);

    
    if (dialogueSO.Type.Equals("a", StringComparison.CurrentCultureIgnoreCase))
    {
        dlogBG.style.display = DisplayStyle.Flex;
        DBox.style.display = DisplayStyle.Flex;
        twoOptionAnswers.style.display = DisplayStyle.None;
        threeOptionAnswers.style.display = DisplayStyle.None;

        nameText.text = dialogueSO.Speaker;
        testText = dialogueSO.Line;
        ScrollT(testText);
    }
    else if (dialogueSO.Type.Equals("b", StringComparison.CurrentCultureIgnoreCase))
    {
        dlogBG.style.display = DisplayStyle.Flex;
        DBox.style.display = DisplayStyle.None;
        twoOptionAnswers.style.display = DisplayStyle.Flex;
        threeOptionAnswers.style.display = DisplayStyle.None;

        questionTwoAnswers.text = dialogueSO.Line;
        aTwo.text = dialogueSO.A1Answer;
        bTwo.text = dialogueSO.A2Answer;
    }
    else if (dialogueSO.Type.Equals("c", StringComparison.CurrentCultureIgnoreCase))
    {
        dlogBG.style.display = DisplayStyle.Flex;
        DBox.style.display = DisplayStyle.None;
        twoOptionAnswers.style.display = DisplayStyle.None;
        threeOptionAnswers.style.display = DisplayStyle.Flex;

        questionThreeAnswers.text = dialogueSO.Line;
        aThree.text = dialogueSO.A1Answer;
        bThree.text = dialogueSO.A2Answer;
        cThree.text = dialogueSO.A3Answer;
    }
    StartFadeIn(characterImageRight); 
    
}

public void StartFadeIn(VisualElement visualElement)
{
    StartCoroutine(FadeInCoroutine(visualElement));
}

private IEnumerator FadeInCoroutine(VisualElement visualElement)
{
    float targetOpacity = 1f;
    float startOpacity = 0f;
    float timeStep = 1f / 1f; // Calculate timeStep based on desired duration (0.5 seconds)

    visualElement.style.opacity = new StyleFloat(startOpacity);

    while (visualElement.style.opacity.value < targetOpacity)
    {
        float newOpacity = Mathf.MoveTowards(visualElement.style.opacity.value, targetOpacity, timeStep * Time.deltaTime);
        visualElement.style.opacity = new StyleFloat(newOpacity);
        yield return null;
    }

    visualElement.style.opacity = new StyleFloat(targetOpacity);
}


public void ScrollT(string sentence)
{
    StartCoroutine(TypeText(sentence));
}

private IEnumerator TypeText(string sentence)
{
    dialogueText.text = "";

    bool insideTag = false;
    bool insideRichTextTag = false;
    foreach (char c in sentence)
    {
        if (c == '<')
        {
            insideTag = true;
        }

        if (insideTag)
        {
            dialogueText.text += c;
        }
        else
        {
            if (c == '>')
            {
                insideRichTextTag = false;
            }

            if (!insideRichTextTag)
            {
                dialogueText.text += c;
                yield return new WaitForSeconds(typingSpeed);
            }

            if (c == '/')
            {
                insideRichTextTag = true;
            }
        }

        if (c == '>')
        {
            insideTag = false;
        }
    }
}


    private void NextDialogue(ClickEvent evt)
    {
        var dialogueSO = dcsvToSO.dialogues[currentIndex];
        currentIndex = dialogueSO.GoToID;
        PopulateUI();
    }
    private void NextDialogueA(ClickEvent evt)
    {
        var dialogueSO = dcsvToSO.dialogues[currentIndex];
        var journalSO = jcsvToSO.journals[dialogueSO.EffectA1];

        currentIndex = dialogueSO.GoToIDA1;
        dialogueSO = dcsvToSO.dialogues[currentIndex];
        PopulateUI();
    }
    private void NextDialogueB(ClickEvent evt)
    {
        var dialogueSO = dcsvToSO.dialogues[currentIndex];
        var journalSO = jcsvToSO.journals[dialogueSO.EffectA2];

        currentIndex = dialogueSO.GoToIDA2;
        dialogueSO = dcsvToSO.dialogues[currentIndex];
        PopulateUI();
    }
    private void NextDialogueC(ClickEvent evt)
    {
        var dialogueSO = dcsvToSO.dialogues[currentIndex];
        var journalSO = jcsvToSO.journals[dialogueSO.EffectA3];

        currentIndex = dialogueSO.GoToIDA3;
        dialogueSO = dcsvToSO.dialogues[currentIndex];
        PopulateUI();
    }
  
}

