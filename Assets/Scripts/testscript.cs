using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class testscript : MonoBehaviour
{

    private Button _journalB;
    private VisualElement _journalUI;
    private Button _exitJournalB;

    public string[] journalL;
    private Button _nextPg;
    private Button _previousPg;
    private Label _eventTextLog;

    private int countJournalNum = 0;
    // Start is called before the first frame update
    

    private void OnEnable()
    {
        var rootVisualElement = GetComponent<UIDocument>().rootVisualElement;
        _journalB = rootVisualElement.Q<Button>("journal-button");
        _journalUI = rootVisualElement.Q<VisualElement>("JournalUIContainer");
        _exitJournalB = rootVisualElement.Q<Button>("exit-ui-button");
        _nextPg = rootVisualElement.Q<Button>("next-page");
        _previousPg = rootVisualElement.Q<Button>("back-page");
        _eventTextLog = rootVisualElement.Q<Label>("event-text");
        
        _journalUI.style.display = DisplayStyle.None;

        _journalB.RegisterCallback<ClickEvent>(ev => OpenJ());
        _exitJournalB.RegisterCallback<ClickEvent>(ev => CloseJ());

        _nextPg.RegisterCallback<ClickEvent>(ev => nextPg());
        _previousPg.RegisterCallback<ClickEvent>(ev => preivousPg());

        _eventTextLog.text = journalL[0];
    }

     private void OpenJ()
    {
        _journalUI.style.display = DisplayStyle.Flex;
    }
    private void CloseJ()
    {
        _journalUI.style.display = DisplayStyle.None;
    }


    private void nextPg()
    {
        //_nextPage.style.display = DisplayStyle.Flex;
        if(countJournalNum == 0)
        {
            _previousPg.style.display = DisplayStyle.None;
        }

        if(countJournalNum < journalL.Length)
        {
            _eventTextLog.text = journalL[countJournalNum];
        }
        countJournalNum++;
    }


    private void preivousPg()
    {
        //_previousPage.style.display = DisplayStyle.Flex;
        if(countJournalNum == journalL.Length)
        {
            _nextPg.style.display = DisplayStyle.None;
        }

        if(countJournalNum == 0)
        {
            _eventTextLog.text = journalL[countJournalNum];
        }
        countJournalNum--;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
