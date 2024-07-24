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
    public float duration = 1f;

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

            StartCoroutine(FadeInCoroutine(frameStill, currentframeStill, frameSO.frameStills));
        }
        else
        {
            // All frames have been shown; enable the next button
            nextButton.SetEnabled(true);
        }
    }

    private IEnumerator FadeInCoroutine(VisualElement visualElement, StyleBackground currentBackground, Sprite nextSprite)
    {
        if (nextSprite == null)
        {
            yield break;
        }

        float startOpacity = 0f;
        float targetOpacity = 1f;
        //float duration = 1f; // Duration of the fade animation in seconds

        var nextBackground = new StyleBackground(nextSprite.texture);
        visualElement.style.backgroundImage = nextBackground;

        float startTime = Time.time;
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
