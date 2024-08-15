using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using QAudioManager;

public class GameController : MonoBehaviour
{
    [Header("Game Panels")]
    public GameObject LosePanel;
    public GameObject PlayerController;
    public GameObject wonPanel;
    public GameObject pausePanel;
    public GameObject coinBg;

    private CannonController cannonController;
    private BuildingManager buildingManager;
    private MachineGun machineGun;
    private CurrencyManager currencyManager;

    [Header("Bool Values for Game")]
    public bool gameIsOn = true;
    private bool isTrue = true;
    private bool hasClaimed = false;
    private bool coroutineRunning = false;

    private float previousValue;

    void Start()
    {
        // Initialize game panels and components
        LosePanel.SetActive(false);
        wonPanel.SetActive(false);

        cannonController = PlayerController.GetComponent<CannonController>();
        buildingManager = FindObjectOfType<BuildingManager>();
        machineGun = FindObjectOfType<MachineGun>();
        currencyManager = FindObjectOfType<CurrencyManager>();

        previousValue = (float)Math.Round(buildingManager.destructionSlider.value, 2);
        hasClaimed = false;
    }

    void Update()
    {
        if (gameIsOn)
        {
            CheckHealth();
            CheckBuildingDestruction();
            CheckCannonBallCount();
        }
    }

    private void CheckHealth()
    {
        if (cannonController.HealthBar.value <= 0.2f && isTrue)
        {
            FindObjectOfType<AudioManager>().Play("lose");
            isTrue = false;
            LosePanel.SetActive(true);
            wonPanel.SetActive(false);
            gameIsOn = false;
        }
    }

    private void CheckBuildingDestruction()
    {
        if (buildingManager.destructionSlider.value >= 0.94f)
        {
            gameIsOn = false;
            if (!hasClaimed)
            {
                currencyManager.CountCoins();
                hasClaimed = true;
            }
            FindObjectOfType<AudioManager>().Play("win");
            wonPanel.SetActive(true);
            LosePanel.SetActive(false);
            SceneNumberCheck();
        }
    }

    private void CheckCannonBallCount()
    {
        if (cannonController.cannonBallCount == 0 && !hasClaimed && !coroutineRunning)
        {
            Debug.Log("Worked 1st Time");
            Debug.Log("Condition is : " + (cannonController.cannonBallCount == 0 && !hasClaimed));
            coroutineRunning = true;
            StartCoroutine(CheckDestructionSlider());
        }
    }

    IEnumerator CheckDestructionSlider()
    {
        yield return new WaitForSeconds(2f);

        // Get the current value of the slider
        float currentValue = (float)Math.Round(buildingManager.destructionSlider.value, 2);
        // Check if the value is increasing
        if (currentValue > previousValue)
        {
            Debug.Log("currentValue outside check" + currentValue);
            Debug.Log("previousValue outside check" + previousValue);
            previousValue = (float)Math.Round(currentValue ,2);

            // The value is increasing
            if (currentValue >= 0.94f)
            {
                Debug.Log("currentValue inside check" + currentValue);
                gameIsOn = false;
                if (!hasClaimed)
                {
                    Debug.Log("I Passed");
                    currencyManager.CountCoins();
                    hasClaimed = true;
                }
                FindObjectOfType<AudioManager>().Play("win");
                wonPanel.SetActive(true);
                LosePanel.SetActive(false);
                SceneNumberCheck();
            }
            else if (currentValue < 0.94f)
            {
                StartCoroutine(CheckDestructionSlider());
            }
        }
        else if (currentValue == previousValue)
        {
            FindObjectOfType<AudioManager>().Play("lose");
            Debug.Log("I failed");
            // The value is not increasing
            isTrue = false;
            LosePanel.SetActive(true);
            wonPanel.SetActive(false);
            gameIsOn = false;
        }
    }

    public void SceneNumberCheck()
    {
        if (PlayerPrefs.HasKey("SceneNumber"))
        {
            int index = PlayerPrefs.GetInt("SceneNumber");
            if (index > (SceneManager.GetActiveScene().buildIndex + 1))
            {
                PlayerPrefs.SetInt("SceneNumber", index);
                PlayerPrefs.Save();
            }
            else
            {
                if((SceneManager.GetActiveScene().buildIndex) == 12)
                {
                    PlayerPrefs.SetInt("SceneNumber", (SceneManager.GetActiveScene().buildIndex));
                    PlayerPrefs.Save();
                }
                else
                {
                    PlayerPrefs.SetInt("SceneNumber", (SceneManager.GetActiveScene().buildIndex + 1));
                    PlayerPrefs.Save();
                }
                
            }
        }
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Resume()
    {
        gameIsOn = true;
        pausePanel.transform.DOLocalMove(new Vector3(0, 3084, 0), 1, true);
    }

    public void Home()
    {
        SceneManager.LoadScene(0);
    }

    public void Pause()
    {
        gameIsOn = false;
        pausePanel.transform.DOLocalMove(new Vector3(0, 0, 0), 1, true);
    }
}
