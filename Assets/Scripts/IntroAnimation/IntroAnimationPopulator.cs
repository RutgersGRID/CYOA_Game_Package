using System.Collections;
using System.Collections.Generic;
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
        }
    }

    private void StartNextFrameAnimation()
    {
        if (!isAnimating && currentIndex >= 0 && currentIndex < FR.frames.Count)
        {
            nextButton.SetEnabled(false);
            var frameSO = FR.frames[currentIndex];
            var currentframeStill = frameStill.style.backgroundImage;
            frameStill.style.backgroundImage = new StyleBackground(frameSO.frameStills);

            // Check if the current frame is different from the previous one
            if (currentframeStill != frameStill.style.backgroundImage)
            {
                StartFadeIn(frameStill);
            }
        }
        else
        {
            // All frames have been shown; enable the next button
            nextButton.SetEnabled(true);
        }
    }

    public void StartFadeIn(VisualElement visualElement)
    {
        StartCoroutine(FadeInCoroutine(visualElement));
    }

    private IEnumerator FadeInCoroutine(VisualElement visualElement)
    {
        isAnimating = true;
        float targetOpacity = 1f;
        float startOpacity = 0f;
        float duration = 1f; // Duration of the fade-in animation in seconds

        visualElement.style.opacity = new StyleFloat(startOpacity);

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