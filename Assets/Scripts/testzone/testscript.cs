using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class testscript : MonoBehaviour
{
    //public CSVtoSOTwo csvToSOTwo;
    public DCSVtoSO dcsvToSO;
    public JCSVtoSO jcsvToSO;
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
    ///
    private VisualElement journalUIContainer;
    private TextElement eventText;
    private Button journalExit;
    private Button nextPage;
    private Button previousPage;

    // private AudioSource newLogSource;
    // private AudioSource checkpointSource;
    // private AudioSource pageflipSource;
    public AudioClip newLogClip;
    public AudioClip checkpointClip;
    public AudioClip pageflipClip;
    public AudioClip dialogueBeepClip;
    AudioSource Audio;

    private int previousCheckPoint;


    string testText;
    int pageNumber = 0;
    List<string> pagesList = new List<string>();
    List<int> eventCheck = new List<int>();

    int[] pages;
    string[] pageLog;
 private void Start()
    {
        Audio = GetComponent<AudioSource>();
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
        textATwo = root.Q<TextElement>("text-a-two");
        aTwo = root.Q<Button>("button-a-two");
        textBTwo = root.Q<TextElement>("text-b-two");
        bTwo = root.Q<Button>("button-b-two");

        threeOptionAnswers = root.Q<VisualElement>("answer-three-options-bg");
        questionThreeAnswers = root.Q<TextElement>("question-text-three");
        textAThree = root.Q<TextElement>("text-a-three");
        aThree = root.Q<Button>("button-a-three");
        textBThree = root.Q<TextElement>("text-b-three");
        bThree = root.Q<Button>("button-b-three");
        textCThree = root.Q<TextElement>("text-c-three");
        cThree = root.Q<Button>("button-c-three");

        journal = root.Q<Button>("journal");
        journalUIContainer = root.Q<VisualElement>("JournalUIContainer");
        eventText = root.Q<TextElement>("journalEntry");
        journalExit = root.Q<Button>("exit-ui-button");
        nextPage = root.Q<Button>("next-page");
        previousPage = root.Q<Button>("back-page");

        nextButton.RegisterCallback<ClickEvent>(NextDialogue);
        aTwo.RegisterCallback<ClickEvent>(NextDialogueA);
        bTwo.RegisterCallback<ClickEvent>(NextDialogueB);
        aThree.RegisterCallback<ClickEvent>(NextDialogueA);
        bThree.RegisterCallback<ClickEvent>(NextDialogueB);
        cThree.RegisterCallback<ClickEvent>(NextDialogueC);

        rewind.RegisterCallback<ClickEvent>(ShowRewind);
        rewindYes.RegisterCallback<ClickEvent>(RewindYes);
        rewindNo.RegisterCallback<ClickEvent>(RewindNo);

        journal.RegisterCallback<ClickEvent>(ShowJournal);
        journalExit.RegisterCallback<ClickEvent>(ExitJournal);
        nextPage.RegisterCallback<ClickEvent>(nextPageB);
        previousPage.RegisterCallback<ClickEvent>(preivousPageB);

        rewindUI.style.display = DisplayStyle.None;
        twoOptionAnswers.style.display = DisplayStyle.None;
        threeOptionAnswers.style.display = DisplayStyle.None;
        journalUIContainer.style.display = DisplayStyle.None;
        previousPage.style.display = DisplayStyle.None;
        PopulateUI();
    }
    private void PopulateUI()
    {
        //var dialogueSO = csvToSOTwo.dialogues[currentIndex];
        var dialogueSO = dcsvToSO.dialogues[currentIndex];
        // if(currentIndex>0)
        // {
        //     var dialogueSOPrev = dcsvToSO.dialogues[currentIndex-1];
        // }
        

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
            //dialogueText.text = dialogueSO.Line;
            testText = dialogueSO.Line;
            ScrollText();
        }
        else if (dialogueSO.Type.Equals("b", StringComparison.CurrentCultureIgnoreCase))
        {
            dlogBG.style.display = DisplayStyle.Flex;
            DBox.style.display = DisplayStyle.None;
            twoOptionAnswers.style.display = DisplayStyle.Flex;
            threeOptionAnswers.style.display = DisplayStyle.None;

            questionTwoAnswers.text = dialogueSO.Line;
            textATwo.text = dialogueSO.A1Answer;
            textBTwo.text = dialogueSO.A2Answer;
        }
        else if (dialogueSO.Type.Equals("c", StringComparison.CurrentCultureIgnoreCase))
        {
            dlogBG.style.display = DisplayStyle.Flex;
            DBox.style.display = DisplayStyle.None;
            twoOptionAnswers.style.display = DisplayStyle.None;
            threeOptionAnswers.style.display = DisplayStyle.Flex;

            questionThreeAnswers.text = dialogueSO.Line;
            textAThree.text = dialogueSO.A1Answer;
            textBThree.text = dialogueSO.A2Answer;
            textCThree.text = dialogueSO.A3Answer;
        }

        if (dialogueSO.Effect != -1)
        {
            var journalSO = jcsvToSO.journals[dialogueSO.Effect];
            foreach (int check in eventCheck)
            {
                if(check == dialogueSO.Effect)
                {
                    break;
                }
            
            }
            pagesList.Add(journalSO.journalEntry);
            Debug.Log(journalSO.journalEntry);
            pageLog = pagesList.ToArray();
            Debug.Log(string.Join(", ", pagesList));
            //newLog.Play();
            Audio.PlayOneShot(newLogClip, 0.7F);
            eventCheck.Add(dialogueSO.Effect);
            pages = eventCheck.ToArray();
            Debug.Log(string.Join(", ", eventCheck));
            //Debug.Log(eventCheck.Count);
            //Debug.Log(pageNumber);
            pageNumber = eventCheck[eventCheck.Count - 1];

        }
        
        // if ( dialogueSO.Checkpoint != dialogueSOPrev.Checkpoint)
        // {
        //     Audio.PlayOneShot(checkpointClip, 0.7F);
        // }
        journalUpdate();
        
        
    }
    public void journalUpdate()
    {
        
        if (pageNumber == eventCheck.Count - 1)
        {
            nextPage.style.display = DisplayStyle.None;
            if (eventCheck.Count > 1)
            {
                previousPage.style.display = DisplayStyle.Flex;
            }
        }
        
        if (pageNumber == 0)
        {
            previousPage.style.display = DisplayStyle.None;
            if (eventCheck.Count > 1)
            {
                nextPage.style.display = DisplayStyle.Flex;
            }
        }

        if (pageNumber > 0 && pageNumber < eventCheck.Count - 1)
        {
            nextPage.style.display = DisplayStyle.Flex;
            previousPage.style.display = DisplayStyle.Flex;
        }

        // else
        // {
        //     nextPage.style.display = DisplayStyle.Flex;
        //     previousPage.style.display = DisplayStyle.Flex;
        // }
        Debug.Log("event count" + eventCheck.Count);
        Debug.Log("page"+ pageNumber);
        eventText.text = pagesList[pageNumber];
    }

    public void ScrollText()
    {
        StopAllCoroutines();
        StartCoroutine(TypeSentence(testText));

    }
    IEnumerator TypeSentence (string sentence) 
    {   //char space = " ";
        dialogueText.text = "";
        int counter = 0;
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            // if (stopAudioSource)
            // {
            //     audioSource.Stop();
            // }
            // if ((letter.Equals(" ") == false) && (counter % charInterval == 0))
            // {
            //     audioSource.PlayOneShot(dialogueTypingSoundClip);
            // }
            if (counter%3 == 0)
            {
                Audio.PlayOneShot(dialogueBeepClip, 0.7F);
            }
            
            yield return new WaitForSeconds(.02f);
            counter++;
        }
    }
    private void NextDialogue(ClickEvent evt)
    {
        //var dialogueSO = csvToSOTwo.dialogues[currentIndex];
        var dialogueSO = dcsvToSO.dialogues[currentIndex];
        currentIndex = dialogueSO.GoToID;
        PopulateUI();
    }
    private void NextDialogueA(ClickEvent evt)
    {
        //var dialogueSO = csvToSOTwo.dialogues[currentIndex];
        var dialogueSO = dcsvToSO.dialogues[currentIndex];
        var journalSO = jcsvToSO.journals[dialogueSO.EffectA1];
        foreach (int check in eventCheck)
            {
                if(check == dialogueSO.EffectA1)
                {
                    break;
                }

            }
        pagesList.Add(journalSO.journalEntry);
        Debug.Log(journalSO.journalEntry);
        Debug.Log(pagesList);
        eventCheck.Add(dialogueSO.EffectA1);
        Debug.Log(eventCheck);

        currentIndex = dialogueSO.GoToIDA1;

        dialogueSO = dcsvToSO.dialogues[currentIndex];

        
        PopulateUI();
    }
    private void NextDialogueB(ClickEvent evt)
    {
        //var dialogueSO = csvToSOTwo.dialogues[currentIndex];
        var dialogueSO = dcsvToSO.dialogues[currentIndex];

        var journalSO = jcsvToSO.journals[dialogueSO.EffectA2];
        foreach (int check in eventCheck)
            {
                if(check == dialogueSO.EffectA2)
                {
                    break;
                }

            }
        pagesList.Add(journalSO.journalEntry);
        Debug.Log(journalSO.journalEntry);
        Debug.Log(pagesList);
        eventCheck.Add(dialogueSO.EffectA2);
        Debug.Log(eventCheck);

        currentIndex = dialogueSO.GoToIDA2;

        dialogueSO = dcsvToSO.dialogues[currentIndex];

        PopulateUI();
    }
    private void NextDialogueC(ClickEvent evt)
    {
        //var dialogueSO = csvToSOTwo.dialogues[currentIndex];
        var dialogueSO = dcsvToSO.dialogues[currentIndex];

        var journalSO = jcsvToSO.journals[dialogueSO.EffectA3];
        foreach (int check in eventCheck)
            {
                if(check == dialogueSO.EffectA3)
                {
                    break;
                }

            }
        pagesList.Add(journalSO.journalEntry);
        Debug.Log(journalSO.journalEntry);
        Debug.Log(pagesList);
        eventCheck.Add(dialogueSO.EffectA3);
        Debug.Log(eventCheck);

        currentIndex = dialogueSO.GoToIDA3;

        dialogueSO = dcsvToSO.dialogues[currentIndex];

        PopulateUI();
    }
    private void ShowRewind(ClickEvent evt)
    {
        rewindUI.style.display = DisplayStyle.Flex;
    }
    private void RewindYes(ClickEvent evt)
    {
        var dialogueSO = dcsvToSO.dialogues[currentIndex];
        currentIndex = dialogueSO.Checkpoint;
        PopulateUI();
        rewindUI.style.display = DisplayStyle.None;
    }
    private void RewindNo(ClickEvent evt)
    {
        rewindUI.style.display = DisplayStyle.None;
    }
    private void ShowJournal(ClickEvent evt)
    {
        journalUIContainer.style.display = DisplayStyle.Flex;
    }
    private void ExitJournal(ClickEvent evt)
    {
        journalUIContainer.style.display = DisplayStyle.None;
    }
    private void nextPageB(ClickEvent evt)
    {
        pageNumber++;
        //pageflip.Play();
        Audio.PlayOneShot(pageflipClip, 0.7F);

        journalUpdate();


    }
    private void preivousPageB(ClickEvent evt)
    {
        pageNumber--;
        //pageflip.Play();
        Audio.PlayOneShot(pageflipClip, 0.7F);

        journalUpdate();

    }
}
