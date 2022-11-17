using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private UIDocument _doc;
    private Button _rewindButton;
    private Button _journalButton;
    private Button _dialogueButton;
    private VisualElement _dialogueVE;
    private VisualElement _characterVE;
    private VisualElement _propVE;
    private Label _nameLabel;
    private Label _dialogueLabel;


    private void Awake ()
    {
        _doc = GetComponent<UIDocument>();
        _rewindButton = _doc.rootVisualElement.Q<Button>("RewindButton");
        _journalButton = _doc.rootVisualElement.Q<Button>("JournalButton");
        _dialogueButton = _doc.rootVisualElement.Q<Button>("DialogueButton");
        _dialogueVE = _doc.rootVisualElement.Q<Button>("DialogueBox");
        _characterVE = _doc.rootVisualElement.Q<Button>("Characters");
        _propVE = _doc.rootVisualElement.Q<Button>("Props");
        _nameLabel = _doc.rootVisualElement.Q<Button>("Name");
        _dialogueLabel = _doc.rootVisualElement.Q<Button>("Dialogue");


        _rewindButton.clicked += Rewind;



    }

    private void Rewind()
    {
        SceneManager.LoadScene("PrototypeUITools");
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
