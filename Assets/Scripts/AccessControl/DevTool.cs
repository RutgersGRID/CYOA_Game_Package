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
    private AccessControlPopulatorTwo scriptToReload;

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        DevToolUI = root.Q<VisualElement>("DevTool");
        DevToolSheetID = root.Q<TextField>("SheetID");
        DevToolYes = root.Q<Button>("DevToolYes");
        DevToolNo = root.Q<Button>("DevToolNo");

        DevToolYes.RegisterCallback<ClickEvent>(TestSheet);
        DevToolNo.RegisterCallback<ClickEvent>(DisableDevTool);

        DevToolUI.style.display = DisplayStyle.None;
        //PlayerPrefs.SetString("SheetId", "0");

        // Find the AccessControlPopulatorTwo script in the scene
        scriptToReload = FindObjectOfType<AccessControlPopulatorTwo>();
        if (scriptToReload == null)
        {
            Debug.LogError("AccessControlPopulatorTwo script not found in the scene!");
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.G) && 
            Input.GetKey(KeyCode.R) && 
            Input.GetKey(KeyCode.I) && 
            Input.GetKey(KeyCode.D))
        {
            DevToolUI.style.display = DisplayStyle.Flex;
            Debug.Log("DevTool now open");
        }
    }

    private void TestSheet(ClickEvent evt)
    {
        string testsheetIdValue = DevToolSheetID.value;
        PlayerPrefs.SetString("SheetId", testsheetIdValue);
        DevToolUI.style.display = DisplayStyle.None;
        Debug.Log("New SheetID is."  + PlayerPrefs.GetString("SheetId"));
        ReloadScript();
    }

    private void DisableDevTool(ClickEvent evt)
    {
        DevToolUI.style.display = DisplayStyle.None;
        Debug.Log("DevTool now closed");
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
}
