using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
public class LoadingScreenScript : MonoBehaviour
{

    public Slider loadingSlider; // Reference to the slider
    public TextMeshProUGUI loadingText; // Reference to the loading text
    public TextMeshProUGUI percentageText; // Reference to the percentage text
    public float duration = 4f; // Duration of the loading animation (increased for slower animation)
    public Ease easeType = Ease.Linear; // Ease type for the loading animation
    public string targetSceneName; // Name of the scene to open after loading
    public int sceneIndex;

    private AsyncOperation asyncLoad;

    private void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex > 0 || !PlayerPrefs.HasKey("SceneNumber"))
        {
            if(SceneManager.GetActiveScene().buildIndex < 12)
            {
                sceneIndex = (SceneManager.GetActiveScene().buildIndex + 1);
                AnimateLoadingScreen(sceneIndex);
            }
            else if (SceneManager.GetActiveScene().buildIndex >= 12)
            {
                AnimateLoadingScreen(0);
            }
            
        }
        else 
        { 
            AnimateLoadingScreen(sceneIndex);
        }

    }

    public void AnimateLoadingScreen(int index)
    {
        Debug.Log("Scene Index : " + index);
        float targetSliderValue = 1f; // Target value for the slider
        string loadingTextPrefix = "Loading"; // Text prefix for the loading text

        loadingSlider.value = 0f; // Set the initial slider value to zero
        loadingText.text = loadingTextPrefix; // Set the initial loading text
        percentageText.text = "0%"; // Set the initial percentage text

        asyncLoad = SceneManager.LoadSceneAsync(index);
        asyncLoad.allowSceneActivation = false;
        // Animate the slider value
        loadingSlider.DOValue(targetSliderValue, duration).SetEase(easeType);

        // Animate the loading text
        loadingText.DOText(loadingTextPrefix + "...", 5)
            .SetEase(easeType)
            .SetLoops(-1, LoopType.Restart); // Loop the animation

        // Animate the percentage text
        DOTween.To(() => int.Parse(percentageText.text.Replace("%", "")), x => percentageText.text = x + "%", 100, duration)
            .SetEase(easeType)
            .OnComplete(() => LoadTargetScene());
    }

    private void LoadTargetScene()
    {
        asyncLoad.allowSceneActivation = true;
    }
}
