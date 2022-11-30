using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]

    public Dialogue dialogue;
    public void TriggerDialogue (){

        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    private UIDocument _doc;
    private Button _rewindButton;
    private Button _journalButton;
    private Button _dialogueButton;
    private VisualElement _dialogueVE;
    private VisualElement _characterVE;
    private VisualElement _propVE;
    private Label _nameLabel;
    private Label _dialogueLabel;
    private Button _choiceselectButton;

    private Button _pickA;
    private Button _pickB;

    private Queue<string> sentences;
    private Queue<string> names;
    private Queue<Sprite> characters;
    private Queue<Sprite> props;
    

    private void Awake ()
    {
        _doc = GetComponent<UIDocument>();
        _rewindButton = _doc.rootVisualElement.Q<Button>("RewindButton");
        _journalButton = _doc.rootVisualElement.Q<Button>("JournalButton");
        _dialogueButton = _doc.rootVisualElement.Q<Button>("dlog-button");
        _choiceselectButton = _doc.rootVisualElement.Q<Button>("ChoiceSelectButton");
        _dialogueVE = _doc.rootVisualElement.Q<VisualElement>("DialogueBox");
        _characterVE = _doc.rootVisualElement.Q<VisualElement>("Characters");
        _propVE = _doc.rootVisualElement.Q<VisualElement>("Props");
        _nameLabel = _doc.rootVisualElement.Q<Label>("dlog-name");
        _dialogueLabel = _doc.rootVisualElement.Q<Label>("dlog-text");

       
        //_pickA = _doc.rootVisualElement.Q<Button>("ChoiceAButton");
        //_pickB = _doc.rootVisualElement.Q<Button>("ChoiceBButton");

        _dialogueButton.RegisterCallback<ClickEvent>(ev => dialogueLabel());

        _rewindButton.clicked += Rewind;
        //_pickA.clicked += GoToA;
        //_pickB.clicked += GoToB;


    }

    private void Rewind()
    {
        SceneManager.LoadScene("PrototypeUITools");
    }
    // private void GoToA()
    // {
    //     SceneManager.LoadScene("PickA");
    // }
    // private void GoToB()
    // {
    //     SceneManager.LoadScene("PickB");
    // }



    public void StartDialogue (Dialogue dialogue)
    {   
        // Clears queue to remove any lingering sentences.
        sentences.Clear();
        names.Clear();
        characters.Clear();
        props.Clear();

        // Forloops that queue up elements from the array
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        foreach (string name in dialogue.names)
        {
            names.Enqueue(name);
        }
        foreach (Sprite character in dialogue.characters)
        {
            characters.Enqueue(character);
        }
        foreach (Sprite prop in dialogue.props)
        {
            props.Enqueue(prop);
        }

        dialogueLabel();
    }

    private void dialogueLabel()
    {
        string name = names.Dequeue();
        string sentence = sentences.Dequeue();
        Sprite character = characters.Dequeue();
        Sprite prop = props.Dequeue();

        _nameLabel.text = name;
        _dialogueLabel.text = sentence;
        //_characterVE.image = character;
        //_propVE.image = prop;
    }
    
    void start()
    {
        sentences = new Queue<string>();
        names = new Queue<string>();
        characters = new Queue<Sprite>();
        props = new Queue<Sprite>();
    }

}
