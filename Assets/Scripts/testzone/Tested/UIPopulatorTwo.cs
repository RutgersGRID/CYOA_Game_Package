// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UIElements;
// using UnityEngine.Networking;

// public class UIPopulatorTwo : MonoBehaviour
// {
//     public StorySheetReaderTwo SSR;
//     public JournalSheetReaderTwo JSR;
//     public CreditSheetReader CSR;
//     int currentIndex = 0;
//     private VisualElement dlogElements;
//     private VisualElement dlogBG;
//     private VisualElement propImage;
//     private VisualElement characterImageLeft;
//     private VisualElement characterImageRight;
//     private VisualElement rewindUI;
//     private Button rewind;
//     private Button rewindYes;
//     private Button rewindNo;
//     private Button journal;
//     ///
//     private VisualElement DBox;
//     private TextElement nameText;
//     private TextElement dialogueText;
//     private Button nextButton;
//     ///
//     private VisualElement twoOptionAnswers;
//     private TextElement questionTwoAnswers;
//     private TextElement textATwo;
//     private Button aTwo;
//     private TextElement textBTwo;
//     private Button bTwo;
//     ///
//     private VisualElement threeOptionAnswers;
//     private TextElement questionThreeAnswers;
//     private TextElement textAThree;
//     private Button aThree;
//     private TextElement textBThree;
//     private Button bThree;
//     private TextElement textCThree;
//     private Button cThree;
//     ///
//     private VisualElement journalUIContainer;
//     private TextElement journalEntry;
//     private TextElement journalTitle;
//     private TextElement journalQuestion;
//     private VisualElement doodle;
//     private Button journalExit;
//     private Button nextPage;
//     private Button previousPage;
//     private Button reflectionPage;
//     private Button aboutPage;
//     private Button howToPlayPage;
//     private Button aboutThisToolPage;
//     private Button aboutGRIDPage;
//     private VisualElement reflectionPageContainer;
//     private VisualElement aboutPageContainer;
//     private VisualElement howToPlayPageContainer;
//     private VisualElement aboutThisToolContainer;
//     private VisualElement aboutGRIDContainer;
//     private TextElement aboutText;
//     private TextElement htpText;
//     private TextElement creditGRIDText;
//     private Button bookmarkOne;
//     private Button bookmarkTwo;
//     private Button bookmarkThree;
//     private Button bookmarkFour;
//     private Button bookmarkFive;
//     ///
//     private Button jEButton;
//     private VisualElement NewJournalEntry;
//     private TextElement Title;
//     private TextElement SummaryText;
//     private TextElement ReflectionQ;  
//     private TextElement Question; 
//     /// 
//     public AudioClip newLogClip;
//     public AudioClip checkpointClip;
//     public AudioClip pageflipClip;
//     public AudioClip dialogueBeepClip;
//     public AudioClip keywordSFX;
//     AudioSource Audio;
//     private int previousCheckPoint;
//     string testText;
//     int pageNumber = 0;
//     List<string> jEventText = new List<string>();
//     List<string> jEventTitle = new List<string>();
//     List<string> jEventQuestionText = new List<string>();
//     List<Sprite> jdoodle = new List<Sprite>();
//     List<int> jPages = new List<int>();
//     public int jNumber = 0;
//     int[] pages;
//     string[] pageLog;
//     ///
//     public float typingSpeed = 0.05f;
//     ///
//     string keywordstring;
//     private string entryPoint;
//     private string answerFillIn;
//     /// <summary>
//     private Coroutine currentTypeTextCoroutine = null;
//     /// </summary>
    
//     private Dictionary<string, string> formFields = new Dictionary<string, string>();
    
//  private void Start()
//     {
//         Audio = GetComponent<AudioSource>();
//         var root = GetComponent<UIDocument>().rootVisualElement;
//         dlogElements = root.Q<VisualElement>("dlog-elements");
//         dlogBG = root.Q<VisualElement>("dlog-bg");
//         characterImageLeft = root.Q<VisualElement>("CharacterLeft");
//         characterImageRight = root.Q<VisualElement>("CharacterRight");
//         propImage = root.Q<VisualElement>("Props");
//         rewindUI = root.Q<VisualElement>("Rewind");

//         DBox = root.Q<VisualElement>("DBox");
//         nameText = root.Q<TextElement>("DialogueName");
//         dialogueText = root.Q<TextElement>("DialogueText");

//         rewind = root.Q<Button>("rewind-button");
//         rewindYes = root.Q<Button>("rewind-yes");
//         rewindNo = root.Q<Button>("rewind-no");
//         nextButton = root.Q<Button>("dlog-button");

//         twoOptionAnswers = root.Q<VisualElement>("answer-two-options-bg");
//         questionTwoAnswers = root.Q<TextElement>("question-text-two");
//         aTwo = root.Q<Button>("button-a-two");
//         bTwo = root.Q<Button>("button-b-two");

//         threeOptionAnswers = root.Q<VisualElement>("answer-three-options-bg");
//         questionThreeAnswers = root.Q<TextElement>("question-text-three");
//         aThree = root.Q<Button>("button-a-three");
//         bThree = root.Q<Button>("button-b-three");
//         cThree = root.Q<Button>("button-c-three");

//         journal = root.Q<Button>("journal");

//         journalUIContainer = root.Q<VisualElement>("JournalUIContainerOld");
//         journalEntry = root.Q<TextElement>("journalSummaryText");
//         journalTitle = root.Q<TextElement>("journalTitle");
//         journalQuestion = root.Q<TextElement>("journalreflectionQuestion");
//         doodle = root.Q<VisualElement>("Doodle");
//         journalExit = root.Q<Button>("exit-ui-button");
//         nextPage = root.Q<Button>("next-page");
//         previousPage = root.Q<Button>("back-page");
//         reflectionPage = root.Q<Button>("ReflectionEvents");
//         aboutPage= root.Q<Button>("About");
//         howToPlayPage = root.Q<Button>("HowToPlay");
//         aboutThisToolPage = root.Q<Button>("AboutThisTool");
//         aboutGRIDPage = root.Q<Button>("AboutGRID");
//         reflectionPageContainer = root.Q<VisualElement>("ReflectionEventPage");
//         aboutPageContainer = root.Q<VisualElement>("AboutEventPage");
//         aboutThisToolContainer = root.Q<VisualElement>("AboutToolEventPage");
//         aboutGRIDContainer = root.Q<VisualElement>("AboutGRIDEventPage");
//         aboutText = root.Q<TextElement>("AboutText");
//         htpText = root.Q<TextElement>("HTPText");
//         creditGRIDText = root.Q<TextElement>("AGText");
//         howToPlayPageContainer = root.Q<VisualElement>("HowToPlayEventPage");
//         bookmarkOne = root.Q<Button>("BookmarkOne");
//         bookmarkTwo = root.Q<Button>("BookmarkTwo");
//         bookmarkThree = root.Q<Button>("BookmarkThree");
//         bookmarkFour = root.Q<Button>("BookmarkFour");
//         bookmarkFive = root.Q<Button>("BookmarkFive");

//         NewJournalEntry = root.Q<VisualElement>("NewJournalEntry");
//         jEButton = root.Q<Button>("JEButton");
//         Title = root.Q<TextElement>("Title");
//         SummaryText = root.Q<TextElement>("SummaryText");
//         ReflectionQ = root.Q<TextElement>("reflectionQ");  
//         Question = root.Q<TextElement>("reflectionQuestion"); 

//         SSR.onDataLoaded += DataLoadedCallback;
//         SSR.StartCoroutine(SSR.ObtainSheetData());

//         JSR.onDataLoaded += DataLoadedCallback;
//         JSR.StartCoroutine(JSR.ObtainSheetData());

//         CSR.onDataLoaded += DataLoadedCallback;
//         CSR.StartCoroutine(CSR.ObtainSheetData());

//         nextButton.RegisterCallback<ClickEvent>(NextDialogue);
//         aTwo.RegisterCallback<ClickEvent>(NextDialogueA);
//         bTwo.RegisterCallback<ClickEvent>(NextDialogueB);
//         aThree.RegisterCallback<ClickEvent>(NextDialogueA);
//         bThree.RegisterCallback<ClickEvent>(NextDialogueB);
//         cThree.RegisterCallback<ClickEvent>(NextDialogueC);

//         rewind.RegisterCallback<ClickEvent>(ShowRewind);
//         rewindYes.RegisterCallback<ClickEvent>(RewindYes);
//         rewindNo.RegisterCallback<ClickEvent>(RewindNo);

//         journal.RegisterCallback<ClickEvent>(ShowJournal);
//         journalExit.RegisterCallback<ClickEvent>(ExitJournal);
//         nextPage.RegisterCallback<ClickEvent>(nextPageB);
//         previousPage.RegisterCallback<ClickEvent>(preivousPageB);
//         reflectionPage.RegisterCallback<ClickEvent>(showReflection);
//         aboutPage.RegisterCallback<ClickEvent>(showAbout);
//         howToPlayPage.RegisterCallback<ClickEvent>(showHowToPlay);
//         aboutThisToolPage.RegisterCallback<ClickEvent>(showAboutThisTool);
//         aboutGRIDPage.RegisterCallback<ClickEvent>(showAboutGRID);
        
//         bookmarkOne.RegisterCallback<ClickEvent>(bMarkOne);
//         bookmarkTwo.RegisterCallback<ClickEvent>(bMarkTwo);
//         bookmarkThree.RegisterCallback<ClickEvent>(bMarkThree);
//         bookmarkFour.RegisterCallback<ClickEvent>(bMarkFour);
//         bookmarkFive.RegisterCallback<ClickEvent>(bMarkFive);

//         jEButton.RegisterCallback<ClickEvent>(JournalEntryButton);

//         rewindUI.style.display = DisplayStyle.None;
//         twoOptionAnswers.style.display = DisplayStyle.None;
//         threeOptionAnswers.style.display = DisplayStyle.None;
//         journalUIContainer.style.display = DisplayStyle.None;
//         previousPage.style.display = DisplayStyle.None;
//         NewJournalEntry.style.display = DisplayStyle.None;
//         howToPlayPageContainer.style.display = DisplayStyle.None;
//         aboutPageContainer.style.display = DisplayStyle.None;
//         bookmarkOne.style.display = DisplayStyle.None;
//         bookmarkTwo.style.display = DisplayStyle.None;
//         bookmarkThree.style.display = DisplayStyle.None;
//         bookmarkFour.style.display = DisplayStyle.None;
//         bookmarkFive.style.display = DisplayStyle.None;

//         Debug.Log("Size of CSR.credits: " + CSR.credits.Count);
        
//         //PopulateUI();
//     }
//     void DataLoadedCallback()
//     {
//         Debug.Log("Data loaded and list populated.");
//         Debug.Log("Size of SSR.dialogues: " + SSR.dialogues.Count);
//         Debug.Log("Size of JSR.journals: " + JSR.journals.Count);
//         Debug.Log("Size of CSR.credits: " + CSR.credits.Count);
//         preloadTheList();
//         if (CSR.credits.Count > 0)
//         {
//             UpdateAboutAndHtpTexts();
//         }
//         if (SSR.dialogues.Count > 0) 
//         {
//             PopulateUI();
//         }
//     }
//     private void UpdateAboutAndHtpTexts()
//     {
//         var creditSO = CSR.credits[0]; // Assuming you want to display the first CreditSO
//         Debug.Log(creditSO.creditTexts);
//         Debug.Log(creditSO.htpTexts);
//         aboutText.text = creditSO.creditTexts;
//         htpText.text = creditSO.htpTexts;
//         creditGRIDText.text = creditSO.creditGRIDTexts;
//     }
//         private void preloadTheList()
//     {
//         foreach (var dialogue in SSR.dialogues)
//         {
//         }
//         foreach (var journal in JSR.journals)
//         {
//         }
//         foreach (var credit in CSR.credits)
//         {
//         }
//     }

// private void AddFormField(string fieldName, string fieldValue)
// {
//     // Add the field to the dictionary
//     formFields[fieldName] = fieldValue;
// }

// // Create a method to submit form data
// private void SubmitFormData()
// {
//     string googleFormURL = "https://docs.google.com/forms/u/0/d/e/1FAIpQLScxj2t25l3MuFxqj7M6uSNGLRX7UQGrgu5mYn4wd0JWzuaqeA/formResponse";

//     // Create a WWWForm and add form data
//     WWWForm form = new WWWForm();
//     form.AddField("entry.1360379984", PlayerPrefs.GetString("AccessCode"));
//     form.AddField("entry.1388253662", PlayerPrefs.GetString("WorkshopId"));

//     // Add form fields from the dictionary
//     foreach (var kvp in formFields)
//     {
//         form.AddField(kvp.Key, kvp.Value);
//     }

//     // Debug.Log to check form field data before submission
//     Debug.Log("Form Data Before Submission:");
//     Debug.Log("AccessCode: " + PlayerPrefs.GetString("AccessCode"));
//     Debug.Log("WorkshopId: " + PlayerPrefs.GetString("WorkshopId"));
//     foreach (var kvp in formFields)
//     {
//         Debug.Log(kvp.Key + ": " + kvp.Value);
//     }

//     // Send the form data to the Google Form
//     StartCoroutine(SendFormResponse(googleFormURL, form));
// }

// // Create a method to send the form data
// private IEnumerator SendFormResponse(string url, WWWForm form)
// {
//     using (UnityWebRequest www = UnityWebRequest.Post(url, form))
//     {
//         yield return www.SendWebRequest();

//         if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
//         {
//             Debug.LogError("Error submitting form: " + www.error);
//         }
//         else
//         {
//             Debug.Log("Form submitted successfully!");
//         }
//     }
// }

// private void OnApplicationQuit()
// {
//     // Debug.Log to check if OnApplicationQuit is being called
//     Debug.Log("Application is quitting. Submitting form data...");
    
//     // Submit the form data when the application quits
//     SubmitFormData();
// }

//     private void PopulateUI()
//     {
//         if (currentIndex >= 0 && currentIndex < SSR.dialogues.Count) 
//         {
//             var dialogueSO = SSR.dialogues[currentIndex];

//             ////
//             var currentRightImage = characterImageRight.style.backgroundImage;
//             var currentLeftImage = characterImageLeft.style.backgroundImage;
//             var currentPropImage = propImage.style.backgroundImage;
//             var currentDlogBGImage = dlogBG.style.backgroundImage;

//             // Update the background images based on dialogueSO
//             characterImageLeft.style.backgroundImage = new StyleBackground(dialogueSO.LeftSideSpeakers);
//             characterImageRight.style.backgroundImage = new StyleBackground(dialogueSO.RightSideSpeakers);
//             propImage.style.backgroundImage = new StyleBackground(dialogueSO.Props);
//             dlogBG.style.backgroundImage = new StyleBackground(dialogueSO.Backgrounds);

//             /// Populate the About and How to Play page
            

//             // Check if characterImageRight has changed and trigger StartFadeIn if it has
//             if (currentRightImage != characterImageRight.style.backgroundImage)
//             {
//                 StartCoroutine(FadeInOutCoroutine(characterImageRight, currentRightImage, dialogueSO.RightSideSpeakers));
//             }

//             // Check if characterImageLeft has changed and trigger StartFadeIn if it has
//             if (currentLeftImage != characterImageLeft.style.backgroundImage)
//             {
                
//             }

//             // Check if propImage has changed and trigger StartFadeIn if it has
//             if (currentPropImage != propImage.style.backgroundImage)
//             {
                
//             }

//             // Check if dlogBG has changed and trigger StartFadeIn if it has
//             if (currentDlogBGImage != dlogBG.style.backgroundImage)
//             {
                
//             }
//             ////

//             dlogBG.style.backgroundImage = new StyleBackground(dialogueSO.Backgrounds);
//             characterImageLeft.style.backgroundImage = new StyleBackground(dialogueSO.LeftSideSpeakers);
//             characterImageRight.style.backgroundImage = new StyleBackground(dialogueSO.RightSideSpeakers);
//             propImage.style.backgroundImage = new StyleBackground(dialogueSO.Props);

//             Debug.Log("dialogueSO.Effects" + dialogueSO.Effects);
//             if (dialogueSO.Types.Equals("a", StringComparison.CurrentCultureIgnoreCase))
//             {
//                 dlogBG.style.display = DisplayStyle.Flex;
//                 DBox.style.display = DisplayStyle.Flex;
//                 twoOptionAnswers.style.display = DisplayStyle.None;
//                 threeOptionAnswers.style.display = DisplayStyle.None;

//                 nameText.text = dialogueSO.Speakers;
//                 testText = dialogueSO.Lines;
//                 keywordstring = dialogueSO.Keywords;
//                 keywordSFX = dialogueSO.SoundEFXs;
//                 ScrollT(testText, keywordstring);
//             }

//             else if (dialogueSO.Types.Equals("b", StringComparison.CurrentCultureIgnoreCase))
//             {
//                 dlogBG.style.display = DisplayStyle.Flex;
//                 DBox.style.display = DisplayStyle.None;
//                 twoOptionAnswers.style.display = DisplayStyle.Flex;
//                 threeOptionAnswers.style.display = DisplayStyle.None;

//                 questionTwoAnswers.text = dialogueSO.Lines;
//                 aTwo.text = dialogueSO.A1Answers;
//                 bTwo.text = dialogueSO.A2Answers;
//             }
//             else if (dialogueSO.Types.Equals("c", StringComparison.CurrentCultureIgnoreCase))
//             {
//                 dlogBG.style.display = DisplayStyle.Flex;
//                 DBox.style.display = DisplayStyle.None;
//                 twoOptionAnswers.style.display = DisplayStyle.None;
//                 threeOptionAnswers.style.display = DisplayStyle.Flex;

//                 questionThreeAnswers.text = dialogueSO.Lines;
//                 aThree.text = dialogueSO.A1Answers;
//                 bThree.text = dialogueSO.A2Answers;
//                 cThree.text = dialogueSO.A3Answers;
//             }
//             if (dialogueSO.Effects != -1 && jNumber != 0)
//             {
//                 if (!jPages.Contains(jNumber))
//                 {
//                     NewJournalEntry.style.display = DisplayStyle.Flex;
//                 }
//             }
//             else if (dialogueSO.Effects != -1)
//             {
//                 if (!jPages.Contains(dialogueSO.Effects))
//                 {
//                     jNumber = dialogueSO.Effects;

//                     if (jNumber < 0 || jNumber >= JSR.journals.Count) // Check added here
//                     {
//                         Debug.LogError("jNumber out of range!");
//                         return;
//                     }

//                     var journalSO = JSR.journals[jNumber];
//                     Title.text = journalSO.journalTitles;
//                     SummaryText.text = journalSO.journalEntrys;
//                     Question.text = journalSO.reflectionQuestions;
//                     NewJournalEntry.style.display = DisplayStyle.Flex;

                    
//                 }
//             }
//             else if (!jPages.Contains(jNumber) && jPages.Count > 0)
//             {
//                 NewJournalEntry.style.display = DisplayStyle.Flex;
//             }
        
//             journalUpdate();
//         }
//         else 
//         {
//             Debug.LogError("currentIndex out of range! Value: " + currentIndex);
//         }
        
//     }

//     private void showReflection(ClickEvent evt)
//     {
//         reflectionPageContainer.style.display = DisplayStyle.Flex;
//         howToPlayPageContainer.style.display = DisplayStyle.None;
//         aboutPageContainer.style.display = DisplayStyle.None;
//         aboutThisToolContainer.style.display = DisplayStyle.None;
//         aboutGRIDContainer.style.display = DisplayStyle.None;
//     }
//     private void showAbout(ClickEvent evt)
//     {
//         reflectionPageContainer.style.display = DisplayStyle.None;
//         howToPlayPageContainer.style.display = DisplayStyle.None;
//         aboutPageContainer.style.display = DisplayStyle.Flex;
//         aboutThisToolContainer.style.display = DisplayStyle.None;
//         aboutGRIDContainer.style.display = DisplayStyle.None;
//     }
//     private void showHowToPlay(ClickEvent evt)
//     {
//         reflectionPageContainer.style.display = DisplayStyle.None;
//         howToPlayPageContainer.style.display = DisplayStyle.Flex;
//         aboutPageContainer.style.display = DisplayStyle.None;
//         aboutThisToolContainer.style.display = DisplayStyle.None;
//         aboutGRIDContainer.style.display = DisplayStyle.None;
//     }
//     private void showAboutThisTool(ClickEvent evt)
//     {
//         reflectionPageContainer.style.display = DisplayStyle.None;
//         howToPlayPageContainer.style.display = DisplayStyle.None;
//         aboutPageContainer.style.display = DisplayStyle.None;
//         aboutThisToolContainer.style.display = DisplayStyle.Flex;
//         aboutGRIDContainer.style.display = DisplayStyle.None;
//     }
//     private void showAboutGRID(ClickEvent evt)
//     {
//         reflectionPageContainer.style.display = DisplayStyle.None;
//         howToPlayPageContainer.style.display = DisplayStyle.None;
//         aboutPageContainer.style.display = DisplayStyle.None;
//         aboutThisToolContainer.style.display = DisplayStyle.None;
//         aboutGRIDContainer.style.display = DisplayStyle.Flex;
//     }

// private IEnumerator FadeInOutCoroutine(VisualElement background, StyleBackground currentBackground, Sprite nextSprite)
// {
//     float startOpacity = 1f;
//     float targetOpacity = 0f;
//     float duration = 1f; // Duration of the fade animation in seconds

//     float startTime = Time.time;
//     while (Time.time - startTime < duration)
//     {
//         float progress = (Time.time - startTime) / duration;
//         float newOpacity = Mathf.Lerp(startOpacity, targetOpacity, progress);
//         background.style.opacity = new StyleFloat(newOpacity);
//         yield return null;
//     }

//     background.style.opacity = new StyleFloat(targetOpacity);

//     // Set the next frame image using the nextSprite
//     var nextBackground = new StyleBackground(nextSprite.texture);

//     background.style.backgroundImage = nextBackground;

//     startOpacity = 0f;
//     targetOpacity = 1f;

//     startTime = Time.time;
//     while (Time.time - startTime < duration)
//     {
//         float progress = (Time.time - startTime) / duration;
//         float newOpacity = Mathf.Lerp(startOpacity, targetOpacity, progress);
//         background.style.opacity = new StyleFloat(newOpacity);
//         yield return null;
//     }

//     background.style.opacity = new StyleFloat(targetOpacity);
//     currentIndex++;
// }
//         public void ScrollT(string sentence, string keywordstring)
//     {
//     //     StartCoroutine(TypeText(sentence, keywordstring));
//     // }
//         if (currentTypeTextCoroutine != null)
//         {
//             StopCoroutine(currentTypeTextCoroutine);
//         }

//         currentTypeTextCoroutine = StartCoroutine(TypeText(sentence, keywordstring));
//     }

// private bool instantCompleteRequested = false;
// private void Update()
// {
//     // Check for input in Update to catch it more reliably
//     if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space))
//     {
//         instantCompleteRequested = true;
//     }
// }
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

//     nextButton.SetEnabled(true);
//     Debug.Log("Coroutine ended");
// }
// /// 
//     private void NextDialogue(ClickEvent evt)
// {
//     Debug.Log($"CurrentIndex before change: {currentIndex}");
//     if (currentIndex < 0 || currentIndex >= SSR.dialogues.Count)
//     {
//         Debug.LogError("Current index is out of range!");
//         return; 
//     }

//     var dialogueSO = SSR.dialogues[currentIndex];
//     int nextIndex = dialogueSO.GoToIDs;

//     // Check if the next index is within range
//     if (nextIndex < 0 || nextIndex >= SSR.dialogues.Count)
//     {
//         Debug.LogError($"GoToID {nextIndex} is out of range!");
//         return; // Exit function early
//     }

//     currentIndex = nextIndex;  // Update current index to the valid next index
//     PopulateUI();
// }
//     private void NextDialogueA(ClickEvent evt)
//     {
//         var dialogueSO = SSR.dialogues[currentIndex];

//         // Debugging statement 1: Print the value of dialogueSO.EffectA1s.
//         Debug.Log("dialogueSO.EffectA1s: " + dialogueSO.EffectA1s);

//         if (dialogueSO.EffectA1s >= 0 && dialogueSO.EffectA1s < JSR.journals.Count)
//         //if (dialogueSO.EffectA1s >= 0 && dialogueSO.EffectA1s)
//         {
//             var journalSO = JSR.journals[dialogueSO.EffectA1s];
//             jNumber = dialogueSO.EffectA1s;

//             Debug.Log("JSR.journals Count: " + JSR.journals.Count);

//             Title.text = journalSO.journalTitles;
//             SummaryText.text = journalSO.journalEntrys;
//             Question.text = journalSO.reflectionQuestions;
//             doodle.style.backgroundImage = new StyleBackground(journalSO.doodles);
//         }
//         else
//         {
//             Title.text = "";
//             SummaryText.text = "";
//             Question.text = "";
//             doodle.style.backgroundImage = new StyleBackground();
//         }

//         AddFormField(dialogueSO.EntryPoints, dialogueSO.A1Answers);
//         Debug.Log(dialogueSO.EntryPoints + dialogueSO.A1Answers);
//         currentIndex = dialogueSO.GoToIDA1s;

//         if (currentIndex >= 0 && currentIndex < SSR.dialogues.Count)
//         {
//             dialogueSO = SSR.dialogues[currentIndex];
//             Debug.Log("jNumber " + jNumber);
//             PopulateUI();
//         }
//         else
//         {
//             Debug.LogError("currentIndex is out of bounds.");
//         }
//     }
//     private void NextDialogueB(ClickEvent evt)
//     {
//         var dialogueSO = SSR.dialogues[currentIndex];

//         // Debugging statement 1: Print the value of dialogueSO.EffectA1s.
//         Debug.Log("dialogueSO.EffectA1s: " + dialogueSO.EffectA2s);

//         if (dialogueSO.EffectA2s >= 0 && dialogueSO.EffectA2s < JSR.journals.Count)
//         //if (dialogueSO.EffectA1s >= 0 && dialogueSO.EffectA1s)
//         {
//             var journalSO = JSR.journals[dialogueSO.EffectA2s];
//             jNumber = dialogueSO.EffectA2s;

//             Debug.Log("JSR.journals Count: " + JSR.journals.Count);

//             Title.text = journalSO.journalTitles;
//             SummaryText.text = journalSO.journalEntrys;
//             Question.text = journalSO.reflectionQuestions;
//             doodle.style.backgroundImage = new StyleBackground(journalSO.doodles);
//         }
//         else
//         {
//             Title.text = "";
//             SummaryText.text = "";
//             Question.text = "";
//             doodle.style.backgroundImage = new StyleBackground();
//         }

//         AddFormField(dialogueSO.EntryPoints, dialogueSO.A2Answers);
//         Debug.Log(dialogueSO.EntryPoints + dialogueSO.A2Answers);

//         currentIndex = dialogueSO.GoToIDA2s;

//         if (currentIndex >= 0 && currentIndex < SSR.dialogues.Count)
//         {
//             dialogueSO = SSR.dialogues[currentIndex];
//             Debug.Log("jNumber " + jNumber);
//             PopulateUI();
//         }
//         else
//         {
//             Debug.LogError("currentIndex is out of bounds.");
//         }
//     }
//     private void NextDialogueC(ClickEvent evt)
//     {
//         var dialogueSO = SSR.dialogues[currentIndex];
//         var journalSO = JSR.journals[dialogueSO.EffectA3s];
//         jNumber = dialogueSO.EffectA3s;

//         Title.text = journalSO.journalTitles;
//         SummaryText.text = journalSO.journalEntrys;
//         Question.text = journalSO.reflectionQuestions;
//         doodle.style.backgroundImage = new StyleBackground(journalSO.doodles);
        
//         currentIndex = dialogueSO.GoToIDA3s;
//         dialogueSO = SSR.dialogues[currentIndex];
//         Debug.Log("jNumber " + jNumber);
//         PopulateUI();
//     }
//     private void ShowRewind(ClickEvent evt)
//     {
//         rewindUI.style.display = DisplayStyle.Flex;
//     }
//     private void RewindYes(ClickEvent evt)
//     {
//         var dialogueSO = SSR.dialogues[currentIndex];
//         currentIndex = dialogueSO.Checkpoints;
//         rewindUI.style.display = DisplayStyle.None;
//         PopulateUI();
        
//     }
//     private void RewindNo(ClickEvent evt)
//     {
//         rewindUI.style.display = DisplayStyle.None;
//     }
//     private void ShowJournal(ClickEvent evt)
//     {
//         journalUIContainer.style.display = DisplayStyle.Flex;
        
//     }
//     private void ExitJournal(ClickEvent evt)
//     {
//         journalUIContainer.style.display = DisplayStyle.None;
//     }
//     private void nextPageB(ClickEvent evt)
//     {
//         pageNumber++;
//         Audio.PlayOneShot(pageflipClip, 0.7F);
//         Debug.Log("Clicked Next");
//         journalUpdate();
//     }
//     private void preivousPageB(ClickEvent evt)
//     {
//         pageNumber--;
//         Audio.PlayOneShot(pageflipClip, 0.7F);
//         Debug.Log("Clicked Previous");
//         journalUpdate();

//     }
//     private void bMarkOne(ClickEvent evt)
//     {
//         pageNumber = 0;
//         Audio.PlayOneShot(pageflipClip, 0.7F);
//         journalUpdate();
//     }
//     private void bMarkTwo(ClickEvent evt)
//     {
//         pageNumber = 1;
//         Audio.PlayOneShot(pageflipClip, 0.7F);
//         journalUpdate();
//     }
//     private void bMarkThree(ClickEvent evt)
//     {
//         pageNumber = 2;
//         Audio.PlayOneShot(pageflipClip, 0.7F);
//         journalUpdate();
//     }
//     private void bMarkFour(ClickEvent evt)
//     {
//         pageNumber = 3;
//         Audio.PlayOneShot(pageflipClip, 0.7F);
//         journalUpdate();
//     }
//     private void bMarkFive(ClickEvent evt)
//     {
//         pageNumber = 4;
//         Audio.PlayOneShot(pageflipClip, 0.7F);
//         journalUpdate();
//     }
//     public void journalUpdate()
//     {
//         if (jPages.Count <= 0)
//         {
//             nextPage.style.display = DisplayStyle.None;
//             previousPage.style.display = DisplayStyle.None;
//         }
        
//         else if (pageNumber >= 0 && pageNumber < jPages.Count)
//         {
//             if (pageNumber == jPages.Count - 1)
//             {
//                 nextPage.style.display = DisplayStyle.None;
//                 if (jPages.Count > 1)
//                 {
//                     previousPage.style.display = DisplayStyle.Flex;
//                 }
//             }
//             else if (pageNumber == 0)
//             {
//                 previousPage.style.display = DisplayStyle.None;
//                 if (jPages.Count > 1)
//                 {
//                     nextPage.style.display = DisplayStyle.Flex;
//                 }
//             }
//             else
//             {
//                 nextPage.style.display = DisplayStyle.Flex;
//                 previousPage.style.display = DisplayStyle.Flex;
//             }
//             if (pageNumber < jEventText.Count)
//             {
//                 journalEntry.text = jEventText[pageNumber];
//                 journalTitle.text = jEventTitle[pageNumber];
//                 journalQuestion.text = jEventQuestionText[pageNumber];
//                 doodle.style.backgroundImage = new StyleBackground(jdoodle[pageNumber]);

//                 Debug.Log("pageNumber: " + pageNumber + ", eventText: " + journalEntry.text);
//                 Debug.Log("jPages" + string.Join(", ", jPages));
//                 Debug.Log("jEventText" + string.Join(", ", jEventText));
//             }
//             else
//             {
//                 Debug.LogError("pageNumber is out of range! jEventText count: " + jEventText.Count);
//                 // Handle the error, e.g., set eventText.text to an error message or disable UI elements
//             }
//         }
//         else
//         {
//             Debug.LogError("pageNumber is out of range! jPages count: " + jPages.Count);
//             Debug.LogError("pageNumber is out of range! pageNumber count: " + pageNumber);
//             // Handle the error, e.g., set eventText.text to an error message or disable UI elements
//         }
//     }
//     private void JournalEntryButton(ClickEvent evt)
//         {
//             //var dialogueSO = csvToSOTwo.dialogues[currentIndex];
//             var dialogueSO = SSR.dialogues[currentIndex];
//             var journalSO = JSR.journals[jNumber];
//             //var journalSO = jcsvToSO.journals[jNumber];

//             Debug.Log("jNumber " + jNumber);
//             Debug.Log("jEventText" + journalSO.journalEntrys);
//             Debug.Log("Page Number " + pageNumber);

//             Audio.PlayOneShot(newLogClip, 0.7F);
            
//             if (dialogueSO.Effects != -1 && jPages.Count < 0)
//             {
//                 //if (!jPages.Contains(dialogueSO.Effect))
//                 if (!jPages.Contains(jNumber))
//                 {
//                     jPages.Add(jNumber);
//                     //jPages.Add(pageNumber);
//                     jEventText.Add(journalSO.journalEntrys);
//                     jEventTitle.Add(journalSO.journalTitles);
//                     jEventQuestionText.Add(journalSO.reflectionQuestions);
//                     jdoodle.Add(journalSO.doodles);
//                     journalEntry.text = jEventText[pageNumber];
//                     journalTitle.text = jEventTitle[pageNumber];
//                     journalQuestion.text = jEventQuestionText[pageNumber];
                    
//                 }
//             }
//             else if (!jPages.Contains(jNumber))
//             {
//                 jEventText.Add(journalSO.journalEntrys);
//                 jEventTitle.Add(journalSO.journalTitles);
//                 jEventQuestionText.Add(journalSO.reflectionQuestions);
//                 jdoodle.Add(journalSO.doodles);
//                 jPages.Add(jNumber);
//                 journalEntry.text = jEventText[jPages.Count -1];
//                 journalTitle.text = jEventTitle[jPages.Count -1];
//                 journalQuestion.text = jEventQuestionText[jPages.Count -1];
//                 doodle.style.backgroundImage = new StyleBackground(jdoodle[jPages.Count -1]);

//             } 
//             Debug.Log("jNumber " + jNumber);
//             Debug.Log("jEventText" + journalSO.journalEntrys);
//             Debug.Log("Page Number " + pageNumber);

//             Debug.Log("Page Number " + pageNumber);
//             Debug.Log("jPages" + string.Join(", ", jPages));

//             if(jPages.Count >= 5)
//             {
//             bookmarkOne.style.display = DisplayStyle.Flex;
//             bookmarkTwo.style.display = DisplayStyle.Flex;
//             bookmarkThree.style.display = DisplayStyle.Flex;
//             bookmarkFour.style.display = DisplayStyle.Flex;
//             bookmarkFive.style.display = DisplayStyle.Flex;
//             }
//             if(jPages.Count >= 4)
//             {
//             bookmarkOne.style.display = DisplayStyle.Flex;
//             bookmarkTwo.style.display = DisplayStyle.Flex;
//             bookmarkThree.style.display = DisplayStyle.Flex;
//             bookmarkFour.style.display = DisplayStyle.Flex;
//             }
//             if(jPages.Count >= 3)
//             {
//             bookmarkOne.style.display = DisplayStyle.Flex;
//             bookmarkTwo.style.display = DisplayStyle.Flex;
//             bookmarkThree.style.display = DisplayStyle.Flex;
//             }
//             if(jPages.Count >= 2)
//             {
//                 bookmarkOne.style.display = DisplayStyle.Flex;
//                 bookmarkTwo.style.display = DisplayStyle.Flex;
//             }
//             if(jPages.Count >= 1)
//             {
//                 bookmarkOne.style.display = DisplayStyle.Flex;
//             }
//             journalUpdate();
//             NewJournalEntry.style.display = DisplayStyle.None;
//         }
// }
