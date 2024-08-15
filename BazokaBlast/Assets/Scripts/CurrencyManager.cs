using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using QAudioManager;

public class CurrencyManager : MonoBehaviour
{
    [Header("Currency Manager for Game")]
    [SerializeField] private GameObject pileOfCoins;
    [SerializeField] private TextMeshProUGUI counter;
    [SerializeField] private Vector2[] initialPos;
    [SerializeField] private Quaternion[] initialRotation;
    [SerializeField] private int coinsAmount;
    [SerializeField] private int amount;
    [SerializeField] private Transform coinIcon;
    [SerializeField] private Vector2 startPos;

    void Start()
    {
        counter.text = PlayerPrefs.GetInt("CountDollar").ToString();
        coinIcon = GameObject.FindWithTag("CoinIcon")?.transform;

        if (coinIcon == null)
        {
            Debug.LogError("CoinIcon with the tag 'CoinIcon' not found. Please check the tag assignment.");
            return;
        }

        startPos = coinIcon.position;

        if (coinsAmount == 0)
            coinsAmount = 10; // you need to change this value based on the number of coins in the inspector

        initialPos = new Vector2[coinsAmount];
        initialRotation = new Quaternion[coinsAmount];

        for (int i = 0; i < pileOfCoins.transform.childCount; i++)
        {
            initialPos[i] = pileOfCoins.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition;
            initialRotation[i] = pileOfCoins.transform.GetChild(i).GetComponent<RectTransform>().rotation;
        }
    }

    private void Update()
    {

        if (coinIcon == null)
        {
            coinIcon = GameObject.FindWithTag("CoinIcon")?.transform;
            startPos = coinIcon.position;
            Debug.LogError("CoinIcon with the tag 'CoinIcon' not found. Please check the tag assignment.");
            return;
        }
    }


    public void CountCoins()
    {
        pileOfCoins.SetActive(true);
        var delay = 0f;

        for (int i = 0; i < pileOfCoins.transform.childCount; i++)
        {
            pileOfCoins.transform.GetChild(i).DOScale(1f, 0.3f).SetDelay(delay).SetEase(Ease.OutBack);
            pileOfCoins.transform.GetChild(i).GetComponent<RectTransform>().DOMove(startPos, 0.8f)
                .SetDelay(delay + 0.5f).SetEase(Ease.InBack);
            pileOfCoins.transform.GetChild(i).DORotate(Vector3.zero, 0.5f).SetDelay(delay + 0.5f).SetEase(Ease.Flash);
            pileOfCoins.transform.GetChild(i).DOScale(0f, 0.3f).SetDelay(delay + 1.5f).SetEase(Ease.OutBack);

            // Play the coin sound for each coin animation
            //FindObjectOfType<AudioManager>().Play("coin");
            StartCoroutine(CoinSound());

            delay += 0.1f;

            counter.transform.parent.GetChild(0).transform.DOScale(0.7f, 0.1f).SetLoops(10, LoopType.Yoyo).SetEase(Ease.InOutSine).SetDelay(1.2f);
        }

        StartCoroutine(CountDollars());
    }


    IEnumerator CountDollars()
    {
        yield return new WaitForSecondsRealtime(1f);
        
        int startValue = PlayerPrefs.GetInt("CountDollar");
        int endValue = startValue + amount + PlayerPrefs.GetInt("BPrize");
        float duration = 1.5f; // Duration for the counter animation

        PlayerPrefs.SetInt("CountDollar", endValue);
        PlayerPrefs.SetInt("BPrize", 0);

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            int currentValue = Mathf.RoundToInt(Mathf.Lerp(startValue, endValue, t));
            counter.text = currentValue.ToString();
            yield return null;
        }

        counter.text = endValue.ToString();
    }

    IEnumerator CoinSound()
    {
        yield return new WaitForSecondsRealtime(1f);
        FindObjectOfType<AudioManager>().Play("coin");
    }

    public void UpdateCoins()
    {
        int currentCoins = PlayerPrefs.GetInt("CountDollar", 0);
        int displayedCoins = int.Parse(counter.text);

        // Animate the coin counter from the current displayed value to the new value
        DOTween.To(() => displayedCoins, x =>
        {
            displayedCoins = x;
            counter.text = displayedCoins.ToString();
        }, currentCoins, 0.5f) // 0.5f is the duration of the animation
        .SetEase(Ease.OutQuad);
    }

}
