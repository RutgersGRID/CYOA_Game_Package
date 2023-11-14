using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PhotoPopulator : MonoBehaviour
{
    private Button nextButton;
    public string sceneToLoad;
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        nextButton = root.Q<Button>("PhotoButton");
        nextButton.RegisterCallback<ClickEvent>(nextScene);
    }
    private void nextScene(ClickEvent evt)
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
