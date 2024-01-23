using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class IntroAnimationPopulator : MonoBehaviour
{
    public FrameReader FR;
    int currentIndex = 0;
    private VisualElement frameStill;
    private Button nextButton;
    public string sceneToLoad;
    private bool isAnimating = false;

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        Debug.Log("Root VisualElement: " + root);

        frameStill = root.Q<VisualElement>("FrameContainer");
        Debug.Log("FrameStill VisualElement: " + frameStill);

        nextButton = root.Q<Button>("AImageButton");
        Debug.Log("NextButton: " + nextButton);
        nextButton.RegisterCallback<ClickEvent>(nextScene);
        nextButton.SetEnabled(false);

        // Check if FR is not null before subscribing to the event
        if (FR != null)
        {
            FR.onDataLoaded += DataLoadedCallback;
            FR.StartCoroutine(FR.ObtainSheetData());
        }
        else
        {
            Debug.LogError("FR is null. Make sure it's properly initialized.");
        }
    }

    private void nextScene(ClickEvent evt)
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    void DataLoadedCallback()
    {
        Debug.Log("Size of FR.frames: " + FR.frames.Count);
        preloadTheList();
        if (FR.frames.Count > 0)
        {
            StartNextFrameAnimation();
        }
    }

    private void preloadTheList()
    {
        foreach (var frame in FR.frames)
        {
            // Preload frames if needed
        }
    }

    private void StartNextFrameAnimation()
    {
        if (!isAnimating && currentIndex >= 0 && currentIndex < FR.frames.Count)
        {
            nextButton.SetEnabled(false);
            var frameSO = FR.frames[currentIndex];
            var currentframeStill = frameStill.style.backgroundImage;

            StartCoroutine(FadeInOutCoroutine(frameStill, currentframeStill, frameSO.frameStills));
        }
        else
        {
            // All frames have been shown; enable the next button
            nextButton.SetEnabled(true);
        }
    }

    private IEnumerator FadeInOutCoroutine(VisualElement visualElement, StyleBackground currentBackground, Sprite nextSprite)
{
    isAnimating = true;
    float startOpacity = 1f;
    float targetOpacity = 0f;
    float duration = 1f; // Duration of the fade animation in seconds

    float startTime = Time.time;
    while (Time.time - startTime < duration)
    {
        float progress = (Time.time - startTime) / duration;
        float newOpacity = Mathf.Lerp(startOpacity, targetOpacity, progress);
        visualElement.style.opacity = new StyleFloat(newOpacity);
        yield return null;
    }

    visualElement.style.opacity = new StyleFloat(targetOpacity);

    // Set the next frame image using the nextSprite
    var nextBackground = new StyleBackground(nextSprite.texture);

    frameStill.style.backgroundImage = nextBackground;

    startOpacity = 0f;
    targetOpacity = 1f;

    startTime = Time.time;
    while (Time.time - startTime < duration)
    {
        float progress = (Time.time - startTime) / duration;
        float newOpacity = Mathf.Lerp(startOpacity, targetOpacity, progress);
        visualElement.style.opacity = new StyleFloat(newOpacity);
        yield return null;
    }

    visualElement.style.opacity = new StyleFloat(targetOpacity);
    currentIndex++;
    isAnimating = false;

    // Start the animation for the next frame
    StartNextFrameAnimation();
}
}
