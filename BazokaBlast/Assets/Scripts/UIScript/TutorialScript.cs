using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TutorialScript : MonoBehaviour
{
    public List<GameObject> panel; // List of panels to be managed
    public GameObject tutorial;    // The tutorial GameObject to show/hide
    public CannonController cannonController;
    public CannonControllerTutorial cannonControllerTut;
    public GameObject healthBar, nextLevel, coinBg, bombBg, pauseButton;
    public UnityEvent unityEvent;

    private void Start()
    {

        // Use consistent key for PlayerPrefs
        if (PlayerPrefs.HasKey("Played"))
        {
            Debug.Log("Played");
            tutorial.SetActive(false); // Hide the tutorial if the key exists
            cannonController.enabled = true;
            cannonControllerTut.enabled = false;
            healthBar.SetActive(true);
            nextLevel.SetActive(true);
            coinBg.SetActive(true);
            bombBg.SetActive(true);
            pauseButton.SetActive(true);
            unityEvent.Invoke();
            
        }
    }

    public void PanelViewer(int index)
    {
        if (index < 0 || index >= panel.Count)
        {
            Debug.LogWarning("Index out of bounds: " + index);
            return;
        }

        // Hide all panels
        for (int i = 0; i < panel.Count; i++)
        {
            panel[i].SetActive(false);
        }

        // Show the selected panel
        panel[index].SetActive(true);

        // Check if the current panel is the last one
        if (panel[index] == panel[panel.Count - 1])
        {
            PlayerPrefs.SetInt("Played", 1);
            PlayerPrefs.Save(); // Ensure changes are saved immediately
        }
    }
}
