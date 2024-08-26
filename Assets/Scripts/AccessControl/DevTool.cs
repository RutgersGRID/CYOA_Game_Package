using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class DevTool : MonoBehaviour
{
    private VisualElement DevToolUI;
    private TextField DevToolSheetID;
    private Button DevToolYes;
    private Button DevToolNo;
    private AccessControlPopulator scriptToReload;

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        DevToolUI = root.Q<VisualElement>("DevToolContainer");
        DevToolSheetID = root.Q<TextField>("SheetID");
        DevToolYes = root.Q<Button>("DevToolYes");
        DevToolNo = root.Q<Button>("DevToolNo");

        DevToolYes.RegisterCallback<ClickEvent>(TestSheet);
        DevToolNo.RegisterCallback<ClickEvent>(DisableDevTool);

        DevToolUI.style.display = DisplayStyle.None;

        scriptToReload = FindObjectOfType<AccessControlPopulator>();
        if (scriptToReload == null)
        {
            Debug.LogError("AccessControlPopulator script not found in the scene!");
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.U) && 
            Input.GetKey(KeyCode.I))
        {
            DevToolUI.style.display = DisplayStyle.Flex;
            Debug.Log("DevTool now open");
        }
    }

    private void TestSheet(ClickEvent evt)
{
    string testsheetIdValue = DevToolSheetID.value;
    PlayerPrefs.SetString("SheetId", testsheetIdValue);
    PlayerPrefs.Save(); // Ensure PlayerPrefs are saved immediately
    DevToolUI.style.display = DisplayStyle.None;
    Debug.Log("New SheetID is " + PlayerPrefs.GetString("SheetId"));
    ReloadScript();
}

void ReloadScript()
{
    if (scriptToReload != null)
    {
        scriptToReload.Reload();
        Debug.Log("Reloaded!");
    }
    else
    {
        Debug.LogError("AccessControlPopulatorTwo script reference is null!");
    }
}

        private void DisableDevTool(ClickEvent evt)
    {
        DevToolUI.style.display = DisplayStyle.None;
        Debug.Log("DevTool now closed");
    }
}
