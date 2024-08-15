using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainmenuManager : MonoBehaviour
{
    public TextMeshProUGUI coinText;
    [SerializeField] public int sceneIndex;
    [SerializeField] private LoadingScreenScript loadscript;
    [SerializeField] private GameObject MainmenuPanel;
    [SerializeField] private GameObject ShopPanel;
    [SerializeField] private GameObject levelSelectPanel;

    private void Start()
    {
        coinText.text = PlayerPrefs.GetInt("CountDollar").ToString();
    }

    public void SceneIndex()
    {
        if (PlayerPrefs.HasKey("SceneNumber"))
        {
            loadscript.sceneIndex = PlayerPrefs.GetInt("SceneNumber");
        }
        else
        {
            loadscript.sceneIndex = (SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    public void PanelMoveIn()
    {
        levelSelectPanel.transform.DOLocalMove(new Vector2(0, 0), 1, false);
    }
    public void PanelMoveOut()
    {
        levelSelectPanel.transform.DOLocalMove(new Vector3(1807, 0, 0), 1, false);
    }

    public void MenuPanelMoveIn()
    {
        MainmenuPanel.transform.DOLocalMove(new Vector2(0, 0), 2, false);
    }
    public void MenuPanelMoveOut()
    {
        MainmenuPanel.transform.DOLocalMove(new Vector3(0, 3040, 0), 1, false);
    }

    public void ShopPanelMoveIn()
    {
        ShopPanel.transform.DOLocalMove(new Vector2(0, 0), 1, false);
    }
    public void ShopPanelMoveOut()
    {
        ShopPanel.transform.DOLocalMove(new Vector3(-2081, 0, 0), 2, false);
    }

    

}
