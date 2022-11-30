using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class DialogueUI : MonoBehaviour
{
    public string[] names;
    //[TextArea(1, 10)]
    public string[] sentences;

    private Button _dialogueButton;
    private Label _nameLabel;
    private Label _dialogueLabel;

    private VisualElement _characterVE;
    private VisualElement _propVE;

    private int count;
    // Start is called before the first frame update
    private void OnEnable()
    {
        var rootVisualElement = GetComponent<UIDocument>().rootVisualElement;
        _nameLabel = rootVisualElement.Q<Label>("dlog-name");
        _dialogueLabel = rootVisualElement.Q<Label>("dlog-text");
        _dialogueButton = rootVisualElement.Q<Button>("dlog-button");

        _characterVE = rootVisualElement.Q<VisualElement>("Characters");
        _propVE = rootVisualElement.Q<VisualElement>("Props");

        _dialogueButton.RegisterCallback<ClickEvent>(ev => dialogueLabel());
    }

    private void dialogueLabel()
    {
        _nameLabel.text = names[count];
        _dialogueLabel.text = sentences[count];
        count++;

    }


    

    void start()
    {

    }
}
