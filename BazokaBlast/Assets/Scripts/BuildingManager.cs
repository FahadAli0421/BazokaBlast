using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening; // Import DOTween namespace

public class BuildingManager : MonoBehaviour
{
    [Header("Building Manager Variables & Slider")]
    [SerializeField] public Slider destructionSlider; // Reference to the UI Slider
    [SerializeField] public TextMeshProUGUI currentLevel, nextLevel;
    [SerializeField] public TextMeshProUGUI percentage;
    [SerializeField] private int totalBlocks; // Total number of blocks in the building
    [SerializeField] private int destroyedBlocks; // Number of blocks destroyed

    void Start()
    {
        Debug.Log("GameObject Name : " + gameObject.name);
        Debug.Log("GameObject Length : " + GameObject.FindGameObjectsWithTag("BuildingBlock").Length);
        Debug.Log("GameObject Length : " + GameObject.FindGameObjectsWithTag("BuildingBlock"));
        // Find all building blocks by tag and initialize total blocks
        totalBlocks = GameObject.FindGameObjectsWithTag("BuildingBlock").Length;
        destructionSlider.value = 0;
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        currentLevel.text = currentIndex.ToString();
        nextLevel.text = (currentIndex + 1).ToString();
        percentage.text = destructionSlider.value.ToString();
    }

    // Method to call when a block is destroyed
    public void BlockDestroyed()
    {
        destroyedBlocks++;
        UpdateSlider();
    }

    private void UpdateSlider()
    {
        if (totalBlocks > 0)
        {
            float targetValue = (float)destroyedBlocks / totalBlocks;
            destructionSlider.DOValue(targetValue, 0.5f); // Smoothly animate the slider value over 0.5 seconds
            percentage.text = ((int)(targetValue * 100)).ToString() + "%";
        }
    }
}
