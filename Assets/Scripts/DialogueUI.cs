using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class DialogueUI : MonoBehaviour
{
/// Scenes
    public GameObject backgroundBase;

    public string SceneBase;
    public string SceneA;
    public string SceneB;
    public string SceneC;

    public int SceneQuestions;
///

/// Dialogue section   
    
    private VisualElement _dBox;

    public string[] names;
    public string[] sentences;
    public Sprite[] characters;
    public Sprite[] props;

    private Button _dialogueButton;
    private Label _nameLabel;
    private Label _dialogueLabel;

    private VisualElement _characterVE;
    private VisualElement _propVE;
///

/// Journal Button
    private Button _journalButton;
    private VisualElement _journalUIVE;
    private Button _exitJournalButton;

    public string[] journalLogs;
    private Button _nextPage;
    private Button _previousPage;
    private Label _eventText;

///

/// Rewind 
    private Label _checkpoint;
    private Button _rewindButton;
    private VisualElement _rewind;
    private Button _rewindYes;
    private Button _rewindNo;
    
///

/// Two answers
    private VisualElement _twoAnswers;
    private Button _taanswerA;
    private Button _taanswerB;
///

/// Three answers
    private VisualElement _threeAnswers;
    private Button _answerA;
    private Button _answerB;
    private Button _answerC;
///

/// Array Counter                       
    private int count = 0;
    private int countJournal = 0;
///


    // Start is called before the first frame update
    private void OnEnable()
    {
        var rootVisualElement = GetComponent<UIDocument>().rootVisualElement;

        /// Dialogue
        _dBox = rootVisualElement.Q<VisualElement>("DBox");
        _nameLabel = rootVisualElement.Q<Label>("dlog-name");
        _dialogueLabel = rootVisualElement.Q<Label>("dlog-text");
        _dialogueButton = rootVisualElement.Q<Button>("dlog-button");
        _characterVE = rootVisualElement.Q<VisualElement>("Characters");
        _propVE = rootVisualElement.Q<VisualElement>("Props");
        ///

        /// Journal
        _journalButton = rootVisualElement.Q<Button>("journal-button");
        _journalUIVE = rootVisualElement.Q<VisualElement>("JournalUIContainer");
        _exitJournalButton = rootVisualElement.Q<Button>("exit-ui-button");
        _nextPage = rootVisualElement.Q<Button>("next-page");
        _previousPage = rootVisualElement.Q<Button>("back-page");
        _eventText = rootVisualElement.Q<Label>("event-text");

        _journalUIVE.style.display = DisplayStyle.None;
        ///

        /// Rewind
        _rewindButton = rootVisualElement.Q<Button>("rewind-button");
        _rewind = rootVisualElement.Q<VisualElement>("Rewind");
        _rewindYes = rootVisualElement.Q<Button>("rewind-yes");
        _rewindNo = rootVisualElement.Q<Button>("rewind-no");
        _checkpoint = rootVisualElement.Q<Label>("checkpoint");
        _rewind.style.display = DisplayStyle.None;
        ///

        /// Two answers
        _twoAnswers = rootVisualElement.Q<VisualElement>("answer-two-options");
        _taanswerA = rootVisualElement.Q<Button>("button-a-two");
        _taanswerB = rootVisualElement.Q<Button>("button-b-two");
        _twoAnswers.style.display = DisplayStyle.None;
        ///

        /// Three answers
        _threeAnswers = rootVisualElement.Q<VisualElement>("answer-three-options");
        _answerA = rootVisualElement.Q<Button>("button-a-three");
        _answerB = rootVisualElement.Q<Button>("button-b-three");
        _answerC = rootVisualElement.Q<Button>("button-c-three");
        _threeAnswers.style.display = DisplayStyle.None;
        ///

        /// Button click events
        _dialogueButton.RegisterCallback<ClickEvent>(ev => dialogueLabel());

        _journalButton.RegisterCallback<ClickEvent>(ev => OpenJournal());
        _exitJournalButton.RegisterCallback<ClickEvent>(ev => CloseJournal());

        _nextPage.RegisterCallback<ClickEvent>(ev => nextPage());
        _previousPage.RegisterCallback<ClickEvent>(ev => preivousPage());

        
        _rewindButton.RegisterCallback<ClickEvent>(ev => RewindOpen());
        _rewindYes.RegisterCallback<ClickEvent>(ev => Rewind());
        _rewindNo.RegisterCallback<ClickEvent>(ev => RewindClose());

        _taanswerA.RegisterCallback<ClickEvent>(ev => GoToA());
        _taanswerB.RegisterCallback<ClickEvent>(ev => GoToB());

        _answerA.RegisterCallback<ClickEvent>(ev => GoToA());
        _answerB.RegisterCallback<ClickEvent>(ev => GoToB());
        _answerC.RegisterCallback<ClickEvent>(ev => GoToC());

        ///
    }

    /// Dialogue system method
    private void dialogueLabel()
    {
        _checkpoint.style.display = DisplayStyle.None;
        if(count >= names.Length)
        {
            _dBox.style.display = DisplayStyle.None;

            if(SceneQuestions == 2)
            {
                _twoAnswers.style.display = DisplayStyle.Flex;
            }
            if(SceneQuestions == 3)
            {
                _threeAnswers.style.display = DisplayStyle.Flex;
            }
        }
        if(count < names.Length)
        {
            _nameLabel.text = names[count];
            _dialogueLabel.text = sentences[count];
            _characterVE.style.backgroundImage = new StyleBackground(characters[count]);
            _propVE.style.backgroundImage = new StyleBackground(props[count]);
        }
        
        count++;
    }
    ///

    /// Rewind Method
    private void Rewind()
    {
        //SceneManager.LoadScene("PrototypeUITools");
        SceneManager.LoadScene(SceneBase);
    }
    private void RewindOpen()
    {
       _rewind.style.display = DisplayStyle.Flex;
    }
    private void RewindClose()
    {
       _rewind.style.display = DisplayStyle.None;
    }
    ///

    /// Journal Methods
    private void OpenJournal()
    {
        _journalUIVE.style.display = DisplayStyle.Flex;
    }
    private void CloseJournal()
    {
        _journalUIVE.style.display = DisplayStyle.None;
    }


    private void nextPage()
    {
        //_nextPage.style.display = DisplayStyle.Flex;
        if(countJournal == 0)
        {
            _previousPage.style.display = DisplayStyle.None;
        }

        if(countJournal < journalLogs.Length)
        {
            _eventText.text = journalLogs[countJournal];
        }
        countJournal++;
    }


    private void preivousPage()
    {
        //_previousPage.style.display = DisplayStyle.Flex;
        if(countJournal == journalLogs.Length)
        {
            _nextPage.style.display = DisplayStyle.None;
        }

        if(countJournal > 0)
        {
            _eventText.text = journalLogs[countJournal];
        }
        countJournal--;
    }

    ///

    /// Answers method

    private void GoToA()
    {
        SceneManager.LoadScene(SceneA);
        bgkiller();
    }
    private void GoToB()
    {
        SceneManager.LoadScene(SceneB);
        bgkiller();
    }
    private void GoToC()
    {
        SceneManager.LoadScene(SceneC);
        bgkiller();
    }
    ///

    /// Destroy bg
    private void bgkiller()
    {
        Destroy(backgroundBase);
    }
    ///

    void start()
    {

    }
    void update()
    {

    }
}
