using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class LevelSelectionScript : MonoBehaviour
{
    [SerializeField] private GameObject loadscreen;
    [SerializeField] private LoadingScreenScript loadscreenScript;
    [SerializeField] private List<GameObject> lockimage;
    [SerializeField] private List<GameObject> button;
    [SerializeField] private List<GameObject> button2;
    [SerializeField] private int index;
    private void Start()
    {
        if (PlayerPrefs.HasKey("SceneNumber"))
        {
            index = PlayerPrefs.GetInt("SceneNumber");
        }
        else
        {
            index = 1;
        }

    }
    private void Update()
    {
        CheckforLevelLock(index);
    }
    public void LevelNumber(int index)
    {
        loadscreenScript.sceneIndex = index;
        loadscreen.SetActive(true);
    }
    public void CheckforLevelLock(int index)
    {

        index = Mathf.Clamp(index, 0, button.Count);

        for (int i = 0; i < button.Count; i++)
        {
            if (i < index)
            {
                button[i].GetComponent<Button>().enabled = true;
                button[i].GetComponent<Image>().enabled = true;
                button2[i].GetComponent<Button>().enabled = false;
                button2[i].GetComponent<Image>().enabled = false;
                lockimage[i].GetComponent<Image>().enabled = false;

            }
            else
            {
                button[i].GetComponent<Button>().enabled = false;
                button[i].GetComponent<Image>().enabled = false;
                button2[i].GetComponent<Button>().enabled = true;
                button2[i].GetComponent<Image>().enabled = true;
            }
        }
    }
    public void ShakeLock(int index)
    {
        lockimage[index-1].transform.DOShakePosition(1f, new Vector3(4, 4, 4), 10, 90, false, true);
    }
}
