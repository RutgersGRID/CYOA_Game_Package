using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Networking;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;

public class UIPopulator : MonoBehaviour
{
    public StorySheetReader SSR;
    public JournalSheetReader JSR;
    public CreditSheetReader CSR;
    int currentIndex = 0;
    private VisualElement sceneElements;
    private VisualElement sceneBackground;
    private VisualElement propImage;
    private VisualElement characterImageLeft;
    private VisualElement characterImageRight;
    private VisualElement rewindUI;
    private Button rewindButton;
    private Button rewindButtonYes;
    private Button rewindButtonNo;
    private Button journalButton;
    ///
    private VisualElement dialogueContainer;
    private TextElement dialogueName;
    private TextElement dialogueText;
    private Button nextDialogueButton;
    ///
    private VisualElement twoOptionContainer;
    private TextElement twoOptionQuestionText;
    //private TextElement textATwo; 
    private Button twoOptionAnswerA;
    //private TextElement textBTwo;
    private Button twoOptionAnswerB;
    ///
    private VisualElement threeOptionContainer;
    private TextElement threeOptionQuestionText;
    //private TextElement textAThree;
    private Button threeOptionAnswerA;
    //private TextElement textBThree;
    private Button threeOptionAnswerB;
    //private TextElement textCThree;
    private Button threeOptionAnswerC;
    ///
    private VisualElement journalUIContainer;
    private TextElement journalEntry;
    private TextElement journalTitle;
    private TextElement journalQuestion;
    private VisualElement doodle;
    private Button journalUICloseButton;
    private Button nextPageButton;
    private Button previousPageButton;
    private Button reflectionPage;
    private Button aboutPage;
    private Button howToPlayPage;
    private Button aboutThisToolPage;
    private Button aboutGRIDPage;
    private VisualElement reflectionPageContainer;
    private VisualElement aboutPageContainer;
    private VisualElement howToPlayPageContainer;
    private VisualElement aboutThisToolContainer;
    private VisualElement aboutGRIDContainer;
    private TextElement aboutText;
    private TextElement htpText;
    private TextElement creditGRIDText;
    private Button bookmarkOne;
    private Button bookmarkTwo;
    private Button bookmarkThree;
    private Button bookmarkFour;
    private Button bookmarkFive;
    ///
    private Button journalEntryPopUpButton;
    private VisualElement journalEntryPopUp;
    private TextElement journalEntryPopUpTitle;
    private TextElement journalEntryPopUpSummaryText;
    private TextElement journalEntryPopUpReflectionQuestion;  
    private TextElement journalEntryPopUpReflectionQuestionText; 
    /// 
    public AudioClip newLogClip;
    public AudioClip checkpointClip;
    public AudioClip pageflipClip;
    public AudioClip dialogueBeepClip;
    public AudioClip keywordSFX;
    AudioSource Audio;
    private int previousCheckPoint;
    string testText;
    int pageNumber = 0;
    List<string> jEventText = new List<string>();
    List<string> jEventTitle = new List<string>();
    List<string> jEventQuestionText = new List<string>();
    List<Sprite> jdoodle = new List<Sprite>();
    List<int> jPages = new List<int>();
    public int jNumber = 0;
    int[] pages;
    string[] pageLog;
    ///
    public float typingSpeed = 0.05f;
    public float fadeDuration = 0.5f;
    ///
    string keywordstring;
    private int QAnum = 1;

    // DEV TOOL
    private VisualElement devToolSkipContainer;
    private ListView devToolSkipListView;
    public List<string> sceneids = new List<string>();
    private TextField devToolSkipSceneID;
    private Button devToolSkipYesButton;
    private Button devToolSkipNoButton;
    
    //
    private Coroutine currentTypeTextCoroutine = null;
    
    
    private Dictionary<string, string> formFields = new Dictionary<string, string>();
    
    async void Start()
    {
        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Signed in anonymously.");
        }

        Audio = GetComponent<AudioSource>();
        var root = GetComponent<UIDocument>().rootVisualElement;
        sceneElements = root.Q<VisualElement>("SceneElementsContainer");
        sceneBackground = root.Q<VisualElement>("SceneBackground");
        characterImageLeft = root.Q<VisualElement>("CharacterLeft");
        characterImageRight = root.Q<VisualElement>("CharacterRight");
        propImage = root.Q<VisualElement>("Props");
        rewindUI = root.Q<VisualElement>("RewindUIContainer");

        dialogueContainer = root.Q<VisualElement>("DialogueContainer");
        dialogueName = root.Q<TextElement>("DialogueName");
        dialogueText = root.Q<TextElement>("DialogueText");

        rewindButton = root.Q<Button>("RewindButton");
        rewindButtonYes = root.Q<Button>("RewindYesButton");
        rewindButtonNo = root.Q<Button>("RewindNoButton");
        nextDialogueButton = root.Q<Button>("NextDialogueButton");

        twoOptionContainer = root.Q<VisualElement>("TwoOptionContainer");
        twoOptionQuestionText = root.Q<TextElement>("TwoOptionQuestionText");
        twoOptionAnswerA = root.Q<Button>("TwoOptionAnswerA");
        twoOptionAnswerB = root.Q<Button>("TwoOptionAnswerB");

        threeOptionContainer = root.Q<VisualElement>("ThreeOptionContainer");
        threeOptionQuestionText = root.Q<TextElement>("ThreeOptionQuestionText");
        threeOptionAnswerA = root.Q<Button>("ThreeOptionAnswerA");
        threeOptionAnswerB = root.Q<Button>("ThreeOptionAnswerB");
        threeOptionAnswerC = root.Q<Button>("ThreeOptionAnswerC");

        journalButton = root.Q<Button>("JournalButton");

        journalUIContainer = root.Q<VisualElement>("JournalUIContainer");
        journalEntry = root.Q<TextElement>("JournalSummaryText");
        journalTitle = root.Q<TextElement>("JournalTitle");
        journalQuestion = root.Q<TextElement>("JournalReflectionQuestion");
        doodle = root.Q<VisualElement>("Doodle");
        journalUICloseButton = root.Q<Button>("JournalUICloseButton");
        nextPageButton = root.Q<Button>("NextPageButton");
        previousPageButton = root.Q<Button>("PreviousPageButton");
        reflectionPage = root.Q<Button>("ReflectionEvents");
        aboutPage= root.Q<Button>("About");
        howToPlayPage = root.Q<Button>("HowToPlay");
        aboutThisToolPage = root.Q<Button>("AboutThisTool");
        aboutGRIDPage = root.Q<Button>("AboutGRID");
        reflectionPageContainer = root.Q<VisualElement>("ReflectionEventPage");
        aboutPageContainer = root.Q<VisualElement>("AboutEventPage");
        aboutThisToolContainer = root.Q<VisualElement>("AboutToolEventPage");
        aboutGRIDContainer = root.Q<VisualElement>("AboutGRIDEventPage");
        aboutText = root.Q<TextElement>("AboutText");
        htpText = root.Q<TextElement>("HTPText");
        creditGRIDText = root.Q<TextElement>("AGText");
        howToPlayPageContainer = root.Q<VisualElement>("HowToPlayEventPage");
        bookmarkOne = root.Q<Button>("BookmarkOne");
        bookmarkTwo = root.Q<Button>("BookmarkTwo");
        bookmarkThree = root.Q<Button>("BookmarkThree");
        bookmarkFour = root.Q<Button>("BookmarkFour");
        bookmarkFive = root.Q<Button>("BookmarkFive");

        journalEntryPopUp = root.Q<VisualElement>("JournalEntryPopUpContainer");
        journalEntryPopUpButton = root.Q<Button>("JournalEntryPopUpButton");
        journalEntryPopUpTitle = root.Q<TextElement>("JournalEntryPopUpTitle");
        journalEntryPopUpSummaryText = root.Q<TextElement>("JournalEntryPopUpSummaryText");
        journalEntryPopUpReflectionQuestion = root.Q<TextElement>("JournalEntryPopUpReflectionQuestion");  
        journalEntryPopUpReflectionQuestionText = root.Q<TextElement>("JournalEntryPopUpReflectionQuestionText"); 

        devToolSkipContainer = root.Q<VisualElement>("DevToolSkipContainer");
        devToolSkipListView = root.Q<ListView>("DevtoolSkipListView");
        devToolSkipSceneID = root.Q<TextField>("DevToolSkipSceneID");
        devToolSkipYesButton = root.Q<Button>("DevToolSkipYesButton");
        devToolSkipNoButton = root.Q<Button>("DevToolSkipNoButton");

        SSR.onDataLoaded += DataLoadedCallback;
        SSR.StartCoroutine(SSR.ObtainSheetData());

        JSR.onDataLoaded += DataLoadedCallback;
        JSR.StartCoroutine(JSR.ObtainSheetData());

        CSR.onDataLoaded += DataLoadedCallback;
        CSR.StartCoroutine(CSR.ObtainSheetData());

        nextDialogueButton.RegisterCallback<ClickEvent>(NextDialogue);
        twoOptionAnswerA.RegisterCallback<ClickEvent>(NextDialogueA);
        twoOptionAnswerB.RegisterCallback<ClickEvent>(NextDialogueB);
        threeOptionAnswerA.RegisterCallback<ClickEvent>(NextDialogueA);
        threeOptionAnswerB.RegisterCallback<ClickEvent>(NextDialogueB);
        threeOptionAnswerB.RegisterCallback<ClickEvent>(NextDialogueC);

        rewindButton.RegisterCallback<ClickEvent>(ShowRewind);
        rewindButtonYes.RegisterCallback<ClickEvent>(RewindYes);
        rewindButtonNo.RegisterCallback<ClickEvent>(RewindNo);

        journalButton.RegisterCallback<ClickEvent>(ShowJournal);
        journalUICloseButton.RegisterCallback<ClickEvent>(ExitJournal);
        nextPageButton.RegisterCallback<ClickEvent>(NextPageButton);
        previousPageButton.RegisterCallback<ClickEvent>(PreivousPageButton);
        reflectionPage.RegisterCallback<ClickEvent>(ShowReflection);
        aboutPage.RegisterCallback<ClickEvent>(ShowAbout);
        howToPlayPage.RegisterCallback<ClickEvent>(ShowHowToPlay);
        aboutThisToolPage.RegisterCallback<ClickEvent>(ShowAboutThisTool);
        aboutGRIDPage.RegisterCallback<ClickEvent>(ShowAboutGRID);
        
        bookmarkOne.RegisterCallback<ClickEvent>(BookmarkOne);
        bookmarkTwo.RegisterCallback<ClickEvent>(BookmarkTwo);
        bookmarkThree.RegisterCallback<ClickEvent>(BookmarkThree);
        bookmarkFour.RegisterCallback<ClickEvent>(BookmarkFour);
        bookmarkFive.RegisterCallback<ClickEvent>(BookmarkFive);

        journalEntryPopUpButton.RegisterCallback<ClickEvent>(JournalEntryButton);

        devToolSkipYesButton.RegisterCallback<ClickEvent>(SkipScene);
        devToolSkipNoButton.RegisterCallback<ClickEvent>(CloseDevTool);

        rewindUI.style.display = DisplayStyle.None;
        twoOptionContainer.style.display = DisplayStyle.None;
        threeOptionContainer.style.display = DisplayStyle.None;
        journalUIContainer.style.display = DisplayStyle.None;
        previousPageButton.style.display = DisplayStyle.None;
        journalEntryPopUp.style.display = DisplayStyle.None;
        howToPlayPageContainer.style.display = DisplayStyle.None;
        aboutPageContainer.style.display = DisplayStyle.None;
        bookmarkOne.style.display = DisplayStyle.None;
        bookmarkTwo.style.display = DisplayStyle.None;
        bookmarkThree.style.display = DisplayStyle.None;
        bookmarkFour.style.display = DisplayStyle.None;
        bookmarkFive.style.display = DisplayStyle.None;
        devToolSkipContainer.style.display = DisplayStyle.None;

        Debug.Log("Size of CSR.credits: " + CSR.credits.Count);

        AddDataToSave("gameStartTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        Debug.Log("Data saved successfully for: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        AddDataToSave("AccessCode", PlayerPrefs.GetString("AccessCode"));
        Debug.Log("Data saved successfully for: " + PlayerPrefs.GetString("AccessCode"));
        AddDataToSave("WorkshopId", PlayerPrefs.GetString("WorkshopId"));
        Debug.Log("Data saved successfully for: " + PlayerPrefs.GetString("WorkshopId"));
        
        foreach (var dialogue in SSR.dialogues)
        {
            sceneids.Add(dialogue.IDs);
        }
        Debug.Log("Scene IDs: " + string.Join(", ", sceneids));
        //populateUI();
    }

    void DataLoadedCallback()
    {
        Debug.Log("Data loaded and list populated.");
        Debug.Log("Size of SSR.dialogues: " + SSR.dialogues.Count);
        Debug.Log("Size of JSR.journals: " + JSR.journals.Count);
        Debug.Log("Size of CSR.credits: " + CSR.credits.Count);
        
        // Print detailed information about SSR, JSR, and CSR
        //PrintSSRData();
       // printJSRData();
        //PrintCSRData();
        
        PreloadTheList();
        if (CSR.credits.Count > 0)
        {
            UpdateAboutAndHtpTexts();
        }
        if (SSR.dialogues.Count > 0) 
        {
            PopulateUI();
        }
    }

    private void PrintSSRData()
    {
        Debug.Log("Printing SSR data:");
        foreach (var dialogue in SSR.dialogues)
        {
            //Debug.Log($"ID: {dialogue.IDs}, Speaker: {dialogue.Speakers}, Line: {dialogue.Lines}, Keywords: {dialogue.Keywords}");
        }
    }

    private void PrintJSRData()
    {
        Debug.Log("Printing JSR data:");
        foreach (var journal in JSR.journals)
        {
            Debug.Log($"ID: {journal.IDs}, Title: {journal.journalTitles}, Entry: {journal.journalEntrys}, Question: {journal.reflectionQuestions}");
        }
    }

    private void PrintCSRData()
    {
        Debug.Log("Printing CSR data:");
        foreach (var credit in CSR.credits)
        {
            //Debug.Log($"Credit Texts: {credit.creditTexts}, HTP Texts: {credit.htpTexts}, Credit GRID Texts: {credit.creditGRIDTexts}");
        }
    }

    private void PreloadTheList()
    {
        foreach (var dialogue in SSR.dialogues)
        {
        }
        foreach (var journal in JSR.journals)
        {
        }
        foreach (var credit in CSR.credits)
        {
        }
    }

    private void UpdateAboutAndHtpTexts()
    {
        var creditSO = CSR.credits[0]; // Assuming you want to display the first CreditSO
        Debug.Log(creditSO.creditTexts);
        Debug.Log(creditSO.htpTexts);
        aboutText.text = creditSO.creditTexts;
        htpText.text = creditSO.htpTexts;
        creditGRIDText.text = creditSO.creditGRIDTexts;
    }

    public async void SaveData(Dictionary<string, object> data)
    {
        try
        {
            // Log the data being saved for debugging purposes
            foreach (var entry in data)
            {
                Debug.Log($"Saving key: {entry.Key}, value: {entry.Value}");
            }

            await CloudSaveService.Instance.Data.Player.SaveAsync(data);
            Debug.Log("Data saved successfully!");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error saving data: {e.Message}\nStack Trace: {e.StackTrace}");
            if (e.InnerException != null)
            {
                Debug.LogError($"Inner Exception: {e.InnerException.Message}");
            }
        }
    }

    private Dictionary<string, object> gameData = new Dictionary<string, object>();

    public void AddDataAndSave(string key, object value)
    {
        gameData[key] = value;
        SaveData(gameData);
    }

    // Simplify the AddDataToSave method
    public void AddDataToSave(string key, string value)
    {
        AddDataAndSave(key, value);
        Debug.Log($"Data type {key} saved with value: {value}");
    }
    private void PopulateUI()
    {
        if (currentIndex >= 0 && currentIndex < SSR.dialogues.Count) 
        {
            var dialogueSO = SSR.dialogues[currentIndex];
        // Ensure no overlapping coroutines
            if (characterImageLeft == null || characterImageRight == null || propImage == null || sceneBackground == null)
        {
            Debug.LogError("One or more VisualElements are not assigned!");
            return;
        }
            ////
            var currentRightImage = characterImageRight.style.backgroundImage;
            var currentLeftImage = characterImageLeft.style.backgroundImage;
            var currentPropImage = propImage.style.backgroundImage;
            var currentsceneBackgroundImage = sceneBackground.style.backgroundImage;
            // Update the background images based on dialogueSO
            characterImageLeft.style.backgroundImage = new StyleBackground(dialogueSO.LeftSideSpeakers);
            characterImageRight.style.backgroundImage = new StyleBackground(dialogueSO.RightSideSpeakers);
            propImage.style.backgroundImage = new StyleBackground(dialogueSO.Props);
            sceneBackground.style.backgroundImage = new StyleBackground(dialogueSO.Backgrounds);
            // Check if characterImageLeft has changed and trigger StartFadeIn if it has
            if (currentRightImage != characterImageRight.style.backgroundImage)
            {
                StartCoroutine(FadeInCoroutine(characterImageRight, currentRightImage, dialogueSO.RightSideSpeakers));
            }
            if (currentLeftImage != characterImageLeft.style.backgroundImage)
            {
                StartCoroutine(FadeInCoroutine(characterImageLeft, currentLeftImage, dialogueSO.LeftSideSpeakers));
            }
            if (currentPropImage != propImage.style.backgroundImage)
            {
                StartCoroutine(FadeInCoroutine(propImage, currentPropImage, dialogueSO.Props));
            }
            if (currentsceneBackgroundImage != sceneBackground.style.backgroundImage)
            {
                StartCoroutine(FadeInCoroutine(sceneBackground, currentsceneBackgroundImage, dialogueSO.Backgrounds));
            }
            ////
            sceneBackground.style.backgroundImage = new StyleBackground(dialogueSO.Backgrounds);
            characterImageLeft.style.backgroundImage = new StyleBackground(dialogueSO.LeftSideSpeakers);
            characterImageRight.style.backgroundImage = new StyleBackground(dialogueSO.RightSideSpeakers);
            propImage.style.backgroundImage = new StyleBackground(dialogueSO.Props);
            Debug.Log("dialogueSO.JournalTriggers" + dialogueSO.JournalTriggers);
            if (dialogueSO.Types.Equals("a", StringComparison.CurrentCultureIgnoreCase))
            {
                sceneBackground.style.display = DisplayStyle.Flex;
                dialogueContainer.style.display = DisplayStyle.Flex;
                twoOptionContainer.style.display = DisplayStyle.None;
                threeOptionContainer.style.display = DisplayStyle.None;
                dialogueName.text = dialogueSO.Speakers;
                testText = dialogueSO.Lines;
                keywordstring = dialogueSO.Keywords;
                keywordSFX = dialogueSO.SoundEFXs;
                ScrollT(testText, keywordstring);
            }
            else if (dialogueSO.Types.Equals("b", StringComparison.CurrentCultureIgnoreCase))
            {
                sceneBackground.style.display = DisplayStyle.Flex;
                dialogueContainer.style.display = DisplayStyle.None;
                twoOptionContainer.style.display = DisplayStyle.Flex;
                threeOptionContainer.style.display = DisplayStyle.None;
                twoOptionQuestionText.text = dialogueSO.Lines;
                twoOptionAnswerA.text = dialogueSO.A1Answers;
                twoOptionAnswerB.text = dialogueSO.A2Answers;
            }
            else if (dialogueSO.Types.Equals("c", StringComparison.CurrentCultureIgnoreCase))
            {
                sceneBackground.style.display = DisplayStyle.Flex;
                dialogueContainer.style.display = DisplayStyle.None;
                twoOptionContainer.style.display = DisplayStyle.None;
                threeOptionContainer.style.display = DisplayStyle.Flex;
                threeOptionQuestionText.text = dialogueSO.Lines;
                threeOptionAnswerA.text = dialogueSO.A1Answers;
                threeOptionAnswerB.text = dialogueSO.A2Answers;
                threeOptionAnswerB.text = dialogueSO.A3Answers;
            }
            if (dialogueSO.JournalTriggers != -1)
            {
                if (!jPages.Contains(dialogueSO.JournalTriggers))
                {
                    jNumber = dialogueSO.JournalTriggers;
                    if (jNumber < 0 || jNumber >= JSR.journals.Count)
                    {
                        Debug.LogError("jNumber out of range!");
                        return;
                    }
                    var journalSO = JSR.journals[jNumber];
                    journalEntryPopUpTitle.text = journalSO.journalTitles;
                    journalEntryPopUpSummaryText.text = journalSO.journalEntrys;
                    journalEntryPopUpReflectionQuestionText.text = journalSO.reflectionQuestions;
                    journalEntryPopUp.style.display = DisplayStyle.Flex;
                }
            }
            JournalUpdate();
        }
        else 
        {
            Debug.LogError("currentIndex out of range! Value: " + currentIndex);
        }
        
    }
   
    private IEnumerator FadeInCoroutine(VisualElement background, StyleBackground currentBackground, Sprite nextSprite)
    {
        // Check if nextSprite is null and do nothing if true
        if (nextSprite == null)
        {
            yield break;  // Do nothing and exit the coroutine
        }

        float startOpacity = 0f; // Start fully transparent
        float targetOpacity = 1f; // End fully opaque
        float duration = fadeDuration; // Duration of the fade animation in seconds

        var nextBackground = new StyleBackground(nextSprite.texture);
        background.style.backgroundImage = nextBackground;

        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            float progress = (Time.time - startTime) / duration;
            float newOpacity = Mathf.Lerp(startOpacity, targetOpacity, progress);
            background.style.opacity = new StyleFloat(newOpacity);

            // Print sprite name and opacity progress
            Debug.Log($"Fading {background.name}: Sprite Name: {nextSprite.name}, Opacity: {newOpacity}");

            yield return null;
        }

        background.style.opacity = new StyleFloat(targetOpacity); // Ensure it's fully visible at the end
        float elapsedTime = Time.time - startTime;
        Debug.Log($"Fade-in complete for: {background.name}, Sprite Name: {nextSprite.name}, Duration: {elapsedTime} seconds");
    }

    private void ShowReflection(ClickEvent evt)
    {
        reflectionPageContainer.style.display = DisplayStyle.Flex;
        howToPlayPageContainer.style.display = DisplayStyle.None;
        aboutPageContainer.style.display = DisplayStyle.None;
        aboutThisToolContainer.style.display = DisplayStyle.None;
        aboutGRIDContainer.style.display = DisplayStyle.None;
    }
    private void ShowAbout(ClickEvent evt)
    {
        reflectionPageContainer.style.display = DisplayStyle.None;
        howToPlayPageContainer.style.display = DisplayStyle.None;
        aboutPageContainer.style.display = DisplayStyle.Flex;
        aboutThisToolContainer.style.display = DisplayStyle.None;
        aboutGRIDContainer.style.display = DisplayStyle.None;
    }
    private void ShowHowToPlay(ClickEvent evt)
    {
        reflectionPageContainer.style.display = DisplayStyle.None;
        howToPlayPageContainer.style.display = DisplayStyle.Flex;
        aboutPageContainer.style.display = DisplayStyle.None;
        aboutThisToolContainer.style.display = DisplayStyle.None;
        aboutGRIDContainer.style.display = DisplayStyle.None;
    }
    private void ShowAboutThisTool(ClickEvent evt)
    {
        reflectionPageContainer.style.display = DisplayStyle.None;
        howToPlayPageContainer.style.display = DisplayStyle.None;
        aboutPageContainer.style.display = DisplayStyle.None;
        aboutThisToolContainer.style.display = DisplayStyle.Flex;
        aboutGRIDContainer.style.display = DisplayStyle.None;
    }
    private void ShowAboutGRID(ClickEvent evt)
    {
        reflectionPageContainer.style.display = DisplayStyle.None;
        howToPlayPageContainer.style.display = DisplayStyle.None;
        aboutPageContainer.style.display = DisplayStyle.None;
        aboutThisToolContainer.style.display = DisplayStyle.None;
        aboutGRIDContainer.style.display = DisplayStyle.Flex;
    }
    public void ScrollT(string sentence, string keywordstring)
    {
    //     StartCoroutine(TypeText(sentence, keywordstring));
    // }
        if (currentTypeTextCoroutine != null)
        {
            StopCoroutine(currentTypeTextCoroutine);
        }

        currentTypeTextCoroutine = StartCoroutine(TypeText(sentence, keywordstring));
    }
    private bool instantCompleteRequested = false;
    private void Update()
    {
        // Check for input in Update to catch it more reliably
        if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space))
        {
            instantCompleteRequested = true;
        }
        if (
            Input.GetKey(KeyCode.U) && 
            Input.GetKey(KeyCode.I))
        {
            // devtoolskipListView.itemsSource = sceneids;
            // devtoolskipListView.makeItem = () => new Label();
            // devtoolskipListView.bindItem = (element, i) => (element as Label).text = sceneids[i];
            // devtoolskipListView.Rebuild();
            devToolSkipContainer.style.display = DisplayStyle.Flex;
            Debug.Log("DevTool now open");
        }
    }

    
    // private IEnumerator TypeText(string sentence, string keywordstring)
    // {
    //     instantCompleteRequested = false;  // Reset the flag at the start of each new line
    //     Debug.Log("TypeText coroutine started with new line");
    //     dialogueText.text = "";
    //     int characterCounter = 0;

    //     for (int i = 0; i < sentence.Length; i++)
    //     {
    //         if (instantCompleteRequested)
    //         {
    //             Debug.Log("Instant completion requested");
    //             dialogueText.text = sentence;
    //             break; // Exit the loop immediately
    //         }

    //         char c = sentence[i];
    //         dialogueText.text += c;
    //         characterCounter++;

    //         if (characterCounter % 4 == 0)
    //         {
    //             Audio.PlayOneShot(dialogueBeepClip, 0.7F);
    //         }

    //         yield return new WaitForSeconds(typingSpeed);
    //     }

    //     nextDialogueButton.SetEnabled(true);
    //     Debug.Log("Coroutine ended");
    // }
private IEnumerator TypeText(string sentence, string keywordstring)
{
    instantCompleteRequested = false;  // Reset the flag at the start of each new line
    Debug.Log("TypeText coroutine started with new line");
    dialogueText.text = "";
    int characterCounter = 0;

    string[] keywords = keywordstring.Split(','); // Assuming keywords are comma-separated

    for (int i = 0; i < sentence.Length; i++)
    {
        if (instantCompleteRequested)
        {
            Debug.Log("Instant completion requested");
            dialogueText.text = sentence;
            break; // Exit the loop immediately
        }

        char c = sentence[i];
        dialogueText.text += c;
        characterCounter++;

        foreach (var keyword in keywords)
        {
            if (sentence.Substring(i).StartsWith(keyword))
            {
                dialogueText.text += keyword.Substring(1); 

                // Play the sound effect associated with the keyword
                Audio.PlayOneShot(keywordSFX);

                // Move the index forward to skip the rest of the keyword
                i += keyword.Length - 1; 
                break;
            }
        }

        // Play the dialogue beep sound at every 4th character as before
        if (characterCounter % 4 == 0)
        {
            Audio.PlayOneShot(dialogueBeepClip, 0.7F);
        }

        yield return new WaitForSeconds(typingSpeed);
    }

    nextDialogueButton.SetEnabled(true);
    Debug.Log("Coroutine ended");
}




    private void NextDialogue(ClickEvent evt)
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

    PopulateUI();
}
    private void NextDialogueA(ClickEvent evt)
{
    var dialogueSO = SSR.dialogues[currentIndex];

    string Qnum = "Question_"+ QAnum.ToString();
    string Anum = "Answer_"+ QAnum.ToString();
    AddDataAndSave(Qnum, dialogueSO.Lines);
    AddDataAndSave(Anum, dialogueSO.A1Answers);
    Debug.Log(dialogueSO.Lines + dialogueSO.A1Answers);
    QAnum++;

    if (dialogueSO.JournalTriggerA1s >= 0 )
    {
        var journalSO = JSR.journals[dialogueSO.JournalTriggerA1s];
        jNumber = dialogueSO.JournalTriggerA1s;

        journalEntryPopUpTitle.text = journalSO.journalTitles;
        journalEntryPopUpSummaryText.text = journalSO.journalEntrys;
        journalEntryPopUpReflectionQuestionText.text = journalSO.reflectionQuestions;
        doodle.style.backgroundImage = new StyleBackground(journalSO.doodles);
    }
    else
    {
        journalEntryPopUpTitle.text = "";
        journalEntryPopUpSummaryText.text = "";
        journalEntryPopUpReflectionQuestionText.text = "";
        doodle.style.backgroundImage = new StyleBackground();
    }

    // Find the index of the next dialogue using the string ID
    string nextID = dialogueSO.GoToIDA1s;
    currentIndex = SSR.dialogues.FindIndex(d => d.IDs == nextID);

    if (currentIndex == -1)
    {
        Debug.LogError($"GoToIDA1 {nextID} not found!");
        return;
    }

    PopulateUI();
}

    private void NextDialogueB(ClickEvent evt)
{
    var dialogueSO = SSR.dialogues[currentIndex];

    string Qnum = "Question_"+ QAnum.ToString();
    string Anum = "Answer_"+ QAnum.ToString();
    AddDataAndSave(Qnum, dialogueSO.Lines);
    AddDataAndSave(Anum, dialogueSO.A2Answers);
    Debug.Log(dialogueSO.Lines + dialogueSO.A2Answers);
    QAnum++;

    if (dialogueSO.JournalTriggerA2s >= 0 && dialogueSO.JournalTriggerA2s < JSR.journals.Count)
    {
        var journalSO = JSR.journals[dialogueSO.JournalTriggerA2s];
        jNumber = dialogueSO.JournalTriggerA2s;

        journalEntryPopUpTitle.text = journalSO.journalTitles;
        journalEntryPopUpSummaryText.text = journalSO.journalEntrys;
        journalEntryPopUpReflectionQuestionText.text = journalSO.reflectionQuestions;
        doodle.style.backgroundImage = new StyleBackground(journalSO.doodles);
    }
    else
    {
        journalEntryPopUpTitle.text = "";
        journalEntryPopUpSummaryText.text = "";
        journalEntryPopUpReflectionQuestionText.text = "";
        doodle.style.backgroundImage = new StyleBackground();
    }

    // Find the index of the next dialogue using the string ID
    string nextID = dialogueSO.GoToIDA2s;
    currentIndex = SSR.dialogues.FindIndex(d => d.IDs == nextID);

    if (currentIndex == -1)
    {
        Debug.LogError($"GoToIDA2 {nextID} not found!");
        return;
    }

    PopulateUI();
}

    private void NextDialogueC(ClickEvent evt)
{
    var dialogueSO = SSR.dialogues[currentIndex];

    string Qnum = "Question_"+ QAnum.ToString();
    string Anum = "Answer_"+ QAnum.ToString();
    AddDataAndSave(Qnum, dialogueSO.Lines);
    AddDataAndSave(Anum, dialogueSO.A3Answers);
    Debug.Log(dialogueSO.Lines + dialogueSO.A3Answers);
    QAnum++;

    if (dialogueSO.JournalTriggerA3s >= 0 )
    {
        var journalSO = JSR.journals[dialogueSO.JournalTriggerA3s];
        jNumber = dialogueSO.JournalTriggerA3s;

        journalEntryPopUpTitle.text = journalSO.journalTitles;
        journalEntryPopUpSummaryText.text = journalSO.journalEntrys;
        journalEntryPopUpReflectionQuestionText.text = journalSO.reflectionQuestions;
        doodle.style.backgroundImage = new StyleBackground(journalSO.doodles);
    }
    else
    {
        journalEntryPopUpTitle.text = "";
        journalEntryPopUpSummaryText.text = "";
        journalEntryPopUpReflectionQuestionText.text = "";
        doodle.style.backgroundImage = new StyleBackground();
    }

    // Find the index of the next dialogue using the string ID
    string nextID = dialogueSO.GoToIDA3s;
    currentIndex = SSR.dialogues.FindIndex(d => d.IDs == nextID);

    if (currentIndex == -1)
    {
        Debug.LogError($"GoToIDA3 {nextID} not found!");
        return;
    }

    PopulateUI();
}

    private void ShowRewind(ClickEvent evt)
    {
        rewindUI.style.display = DisplayStyle.Flex;
    }

    private void RewindYes(ClickEvent evt)
{
    var dialogueSO = SSR.dialogues[currentIndex];
    string checkpointID = dialogueSO.Checkpoints;

    // Find the index of the checkpoint dialogue using the string ID
    currentIndex = SSR.dialogues.FindIndex(d => d.IDs == checkpointID);

    // Check if the index was found
    if (currentIndex == -1)
    {
        Debug.LogError($"Checkpoint ID {checkpointID} not found!");
        return; // Exit function early
    }

    rewindUI.style.display = DisplayStyle.None;
    PopulateUI();
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
    private void NextPageButton(ClickEvent evt)
    {
        pageNumber++;
        Audio.PlayOneShot(pageflipClip, 0.7F);
        Debug.Log("Clicked Next");
        JournalUpdate();
    }
    private void PreivousPageButton(ClickEvent evt)
    {
        pageNumber--;
        Audio.PlayOneShot(pageflipClip, 0.7F);
        Debug.Log("Clicked Previous");
        JournalUpdate();

    }
    private void BookmarkOne(ClickEvent evt)
    {
        pageNumber = 0;
        Audio.PlayOneShot(pageflipClip, 0.7F);
        JournalUpdate();
    }
    private void BookmarkTwo(ClickEvent evt)
    {
        pageNumber = 1;
        Audio.PlayOneShot(pageflipClip, 0.7F);
        JournalUpdate();
    }
    private void BookmarkThree(ClickEvent evt)
    {
        pageNumber = 2;
        Audio.PlayOneShot(pageflipClip, 0.7F);
        JournalUpdate();
    }
    private void BookmarkFour(ClickEvent evt)
    {
        pageNumber = 3;
        Audio.PlayOneShot(pageflipClip, 0.7F);
        JournalUpdate();
    }
    private void BookmarkFive(ClickEvent evt)
    {
        pageNumber = 4;
        Audio.PlayOneShot(pageflipClip, 0.7F);
        JournalUpdate();
    }

    public void JournalUpdate()
{
    if (jPages.Count <= 0)
    {
        nextPageButton.style.display = DisplayStyle.None;
        previousPageButton.style.display = DisplayStyle.None;
    }
    else if (pageNumber >= 0 && pageNumber < jPages.Count)
    {
        if (pageNumber == jPages.Count - 1)
        {
            nextPageButton.style.display = DisplayStyle.None;
            if (jPages.Count > 1)
            {
                previousPageButton.style.display = DisplayStyle.Flex;
            }
        }
        else if (pageNumber == 0)
        {
            previousPageButton.style.display = DisplayStyle.None;
            if (jPages.Count > 1)
            {
                nextPageButton.style.display = DisplayStyle.Flex;
            }
        }
        else
        {
            nextPageButton.style.display = DisplayStyle.Flex;
            previousPageButton.style.display = DisplayStyle.Flex;
        }
        if (pageNumber < jEventText.Count)
        {
            journalEntry.text = jEventText[pageNumber];
            journalTitle.text = jEventTitle[pageNumber];
            journalQuestion.text = jEventQuestionText[pageNumber];
            doodle.style.backgroundImage = new StyleBackground(jdoodle[pageNumber]);
        }
        else
        {
            Debug.LogError("pageNumber is out of range! jEventText count: " + jEventText.Count);
        }
    }
    else
    {
        Debug.LogError("pageNumber is out of range! jPages count: " + jPages.Count);
    }
}

    private void JournalEntryButton(ClickEvent evt)
        {
            //var dialogueSO = csvToSOTwo.dialogues[currentIndex];
            var dialogueSO = SSR.dialogues[currentIndex];
            var journalSO = JSR.journals[jNumber];
            //var journalSO = jcsvToSO.journals[jNumber];

            Debug.Log("jNumber " + jNumber);
            Debug.Log("jEventText" + journalSO.journalEntrys);
            Debug.Log("Page Number " + pageNumber);

            Audio.PlayOneShot(newLogClip, 0.7F);
            
            if (dialogueSO.JournalTriggers != -1 && jPages.Count < 0)
            {
                //if (!jPages.Contains(dialogueSO.JournalTrigger))
                if (!jPages.Contains(jNumber))
                {
                    jPages.Add(jNumber);
                    //jPages.Add(pageNumber);
                    jEventText.Add(journalSO.journalEntrys);
                    jEventTitle.Add(journalSO.journalTitles);
                    jEventQuestionText.Add(journalSO.reflectionQuestions);
                    jdoodle.Add(journalSO.doodles);
                    journalEntry.text = jEventText[pageNumber];
                    journalTitle.text = jEventTitle[pageNumber];
                    journalQuestion.text = jEventQuestionText[pageNumber];
                    
                }
            }
            else if (!jPages.Contains(jNumber))
            {
                jEventText.Add(journalSO.journalEntrys);
                jEventTitle.Add(journalSO.journalTitles);
                jEventQuestionText.Add(journalSO.reflectionQuestions);
                jdoodle.Add(journalSO.doodles);
                jPages.Add(jNumber);
                journalEntry.text = jEventText[jPages.Count -1];
                journalTitle.text = jEventTitle[jPages.Count -1];
                journalQuestion.text = jEventQuestionText[jPages.Count -1];
                doodle.style.backgroundImage = new StyleBackground(jdoodle[jPages.Count -1]);

            } 
            Debug.Log("jNumber " + jNumber);
            Debug.Log("jEventText" + journalSO.journalEntrys);
            Debug.Log("Page Number " + pageNumber);

            Debug.Log("Page Number " + pageNumber);
            Debug.Log("jPages" + string.Join(", ", jPages));

            if(jPages.Count >= 5)
            {
            bookmarkOne.style.display = DisplayStyle.Flex;
            bookmarkTwo.style.display = DisplayStyle.Flex;
            bookmarkThree.style.display = DisplayStyle.Flex;
            bookmarkFour.style.display = DisplayStyle.Flex;
            bookmarkFive.style.display = DisplayStyle.Flex;
            }
            if(jPages.Count >= 4)
            {
            bookmarkOne.style.display = DisplayStyle.Flex;
            bookmarkTwo.style.display = DisplayStyle.Flex;
            bookmarkThree.style.display = DisplayStyle.Flex;
            bookmarkFour.style.display = DisplayStyle.Flex;
            }
            if(jPages.Count >= 3)
            {
            bookmarkOne.style.display = DisplayStyle.Flex;
            bookmarkTwo.style.display = DisplayStyle.Flex;
            bookmarkThree.style.display = DisplayStyle.Flex;
            }
            if(jPages.Count >= 2)
            {
                bookmarkOne.style.display = DisplayStyle.Flex;
                bookmarkTwo.style.display = DisplayStyle.Flex;
            }
            if(jPages.Count >= 1)
            {
                bookmarkOne.style.display = DisplayStyle.Flex;
            }
            JournalUpdate();
            journalEntryPopUp.style.display = DisplayStyle.None;
        }
    private void SkipScene(ClickEvent evt)
    {
        string sceneID = devToolSkipSceneID.value;
    
        // Find the index of the scene using the string ID
        currentIndex = SSR.dialogues.FindIndex(d => d.IDs == sceneID);

        // Check if the index was found
        if (currentIndex == -1)
        {
            Debug.LogError($"Scene ID {sceneID} not found!");
            return; // Exit function early if the ID is not found
        }

    // Populate the UI with the new current index
        PopulateUI();
        devToolSkipContainer.style.display = DisplayStyle.None;
    }
    private void CloseDevTool(ClickEvent evt)
    {
        devToolSkipContainer.style.display = DisplayStyle.None;
    }
}