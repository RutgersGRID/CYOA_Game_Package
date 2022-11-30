using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class DialogueUI : MonoBehaviour
{
    public Dialogue dialogue;
    public void TriggerDialogue (){

        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    private Queue<string> sentences;
    private Queue<string> names;
    private Queue<Sprite> characters;
    private Queue<Sprite> props;

    private Button _dialogueButton;
    private Label _nameLabel;
    private Label _dialogueLabel;

    private VisualElement _characterVE;
    private VisualElement _propVE;
    // Start is called before the first frame update
    private void OnEnable()
    {
        var rootVisualElement = GetComponent<UIDocument>().rootVisualElement;
        _nameLabel = rootVisualElement.Q<Label>("dlog-name");
        _dialogueLabel = rootVisualElement.Q<Label>("dlog-name");
        _dialogueButton = rootVisualElement.Q<Button>("dlog-button");

        _characterVE = rootVisualElement.Q<VisualElement>("Characters");
        _propVE = rootVisualElement.Q<VisualElement>("Props");

        _dialogueButton.RegisterCallback<ClickEvent>(ev => dialogueLabel());
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

    void start()
    {
        sentences = new Queue<string>();
        names = new Queue<string>();
        characters = new Queue<Sprite>();
        props = new Queue<Sprite>();
    }
}
