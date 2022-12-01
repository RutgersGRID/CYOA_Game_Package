using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class DialogueUI : MonoBehaviour
{
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
///

/// Rewind Button
    private Button _rewindButton;
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
        _journalUIVE.style.display = DisplayStyle.None;
        ///

        /// Rewind
        _rewindButton = rootVisualElement.Q<Button>("rewind-button");
        ///

        /// Two answers
        _twoAnswers = rootVisualElement.Q<VisualElement>("answer-two-options");
        _twoAnswers.style.display = DisplayStyle.None;
        _taanswerA = rootVisualElement.Q<Button>("answer-a");
        _taanswerB = rootVisualElement.Q<Button>("answer-b");
        ///

        /// Three answers
        _threeAnswers = rootVisualElement.Q<VisualElement>("answer-three-options");
        _threeAnswers.style.display = DisplayStyle.None;
        _answerA = rootVisualElement.Q<Button>("answer-a-t");
        _answerB = rootVisualElement.Q<Button>("answer-b-t");
        _answerC = rootVisualElement.Q<Button>("answer-c-t");
        ///

        /// Button click events
        _dialogueButton.RegisterCallback<ClickEvent>(ev => dialogueLabel());

        _journalButton.RegisterCallback<ClickEvent>(ev => OpenJournal());
        _exitJournalButton.RegisterCallback<ClickEvent>(ev => CloseJournal());

        _rewindButton.RegisterCallback<ClickEvent>(ev => Rewind());

        //_taanswerA.RegisterCallback<ClickEvent>(ev => GoToA());
        //_taanswerB.RegisterCallback<ClickEvent>(ev => GoToB());

        _answerA.RegisterCallback<ClickEvent>(ev => GoToA());
        _answerB.RegisterCallback<ClickEvent>(ev => GoToB());
        _answerC.RegisterCallback<ClickEvent>(ev => GoToC());
        // _answerA.RegisterCallback<ClickEvent>(ev => OpenJournal());
        // _answerB.RegisterCallback<ClickEvent>(ev => OpenJournal());
        // _answerC.RegisterCallback<ClickEvent>(ev => OpenJournal());
        ///
    }

    /// Dialogue system method
    private void dialogueLabel()
    {
        if(count >= names.Length)
        {
            _dBox.style.display = DisplayStyle.None;

            //_twoAnswers.style.display = DisplayStyle.None;
            _threeAnswers.style.display = DisplayStyle.Flex;

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
        SceneManager.LoadScene("PrototypeUITools");
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
    ///

    /// Answers method
    private void GoToA()
    {
        SceneManager.LoadScene("PickA");
    }
    private void GoToB()
    {
        SceneManager.LoadScene("PickB");
    }
    private void GoToC()
    {
        SceneManager.LoadScene("PickC");
    }
    ///

    void start()
    {

    }
}
