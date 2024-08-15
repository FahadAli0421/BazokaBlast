using UnityEngine;
using DG.Tweening; // Import DOTween
using System.Collections;
using TMPro; // Import TMP
using UnityEngine.UI;

public class ShopScript : MonoBehaviour
{
    [SerializeField] private GameObject[] cannons; // Array of all cannons in the shop
    [SerializeField] private float transitionDuration = 0.5f; // Duration of the transition
    [SerializeField] private Transform leftPosition; // Position to transition out to the left
    [SerializeField] private Transform rightPosition; // Position to transition out to the right
    [SerializeField] private Transform centerPosition; // Position for the active cannon
    [SerializeField] private TextMeshProUGUI coinCounter; // TextMeshProUGUI for coin count
    [SerializeField] private int cannonPrice = 100; // Price of each cannon, excluding the default one

    private int currentIndex = 0; // Index of the currently displayed cannon
    private bool isTransitioning = false; // Is a transition currently happening?

    [SerializeField] private TextMeshProUGUI counter;
    [SerializeField] private Button buyButton;
    [SerializeField] private GameObject icon;
    [SerializeField] private GameObject price;
    [SerializeField] private GameObject boughtIcon;

    void Start()
    {
        counter.text = PlayerPrefs.GetInt("CountDollar").ToString();
        // Ensure only the first cannon is active at the start
        DisplayCannon(currentIndex);
        UpdateCoinCounter();
        UpdateBuyButtonState();
    }

    private void Update()
    {
        UpdateBuyButtonState();
    }

    private void UpdateBuyButtonState()
    {
        // The default cannon (index 0) can always be selected
        bool isDefaultCannon = currentIndex == 0;
        bool isBought = IsCannonBought(currentIndex);

        buyButton.enabled = !isDefaultCannon && !isBought;
        icon.SetActive(!isDefaultCannon && !isBought);
        price.SetActive(!isDefaultCannon && !isBought);
        boughtIcon.SetActive(isDefaultCannon || isBought);
    }

    public void ShowNextCannon()
    {
        if (isTransitioning) return; // Prevent new transitions while one is in progress
        StartCoroutine(TransitionToNextCannon());
    }

    public void ShowPreviousCannon()
    {
        if (isTransitioning) return; // Prevent new transitions while one is in progress
        StartCoroutine(TransitionToPreviousCannon());
    }

    private IEnumerator TransitionToNextCannon()
    {
        isTransitioning = true;
        // Move the current cannon out to the left
        cannons[currentIndex].transform.DOMove(leftPosition.position, transitionDuration).SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(transitionDuration);
        cannons[currentIndex].SetActive(false);
        // Update index and set the next cannon active
        currentIndex = (currentIndex + 1) % cannons.Length;
        cannons[currentIndex].SetActive(true);
        cannons[currentIndex].transform.position = rightPosition.position;
        // Move the new cannon to the center position
        cannons[currentIndex].transform.DOMove(centerPosition.position, transitionDuration).SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(transitionDuration);
        isTransitioning = false;
    }

    private IEnumerator TransitionToPreviousCannon()
    {
        isTransitioning = true;
        // Move the current cannon out to the right
        cannons[currentIndex].transform.DOMove(rightPosition.position, transitionDuration).SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(transitionDuration);
        cannons[currentIndex].SetActive(false);
        // Update index and set the previous cannon active
        currentIndex = (currentIndex - 1 + cannons.Length) % cannons.Length;
        cannons[currentIndex].SetActive(true);
        cannons[currentIndex].transform.position = leftPosition.position;
        // Move the new cannon to the center position
        cannons[currentIndex].transform.DOMove(centerPosition.position, transitionDuration).SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(transitionDuration);
        isTransitioning = false;
    }

    private void DisplayCannon(int index)
    {
        cannons[index].transform.position = centerPosition.position;
        cannons[index].SetActive(true);
    }

    public void SelectCannon()
    {
        if (currentIndex == 0 || IsCannonBought(currentIndex))
        {
            PlayerPrefs.SetInt("SelectedCannonIndex", currentIndex);
            PlayerPrefs.Save();
        }
        else
        {
            Debug.Log("This cannon cannot be selected because it has not been bought.");
        }
    }

    public void BuyCannon()
    {
        // Ensure the default cannon (index 0) is not buyable
        if (currentIndex == 0)
        {
            SelectCannon();
            Debug.Log("Default cannon cannot be bought.");
            return;
        }

        int currentCoins = PlayerPrefs.GetInt("CountDollar");

        if (currentCoins >= cannonPrice)
        {
            // Deduct coins and mark the cannon as bought
            PlayerPrefs.SetInt("CountDollar", currentCoins - cannonPrice);
            PlayerPrefs.SetInt(GetCannonBoughtKey(currentIndex), 1);
            PlayerPrefs.Save();
            UpdateCoinCounter();
            UpdateBuyButtonState(); // Update button state after purchase
            SelectCannon();
        }
        else
        {
            Debug.Log("Not enough coins to buy this cannon.");
        }
    }

    private void UpdateCoinCounter()
    {
        int startValue = int.Parse(coinCounter.text); // Get the current displayed value
        int endValue = PlayerPrefs.GetInt("CountDollar"); // The value to reach

        // Animate the change in the coin counter text
        DOTween.To(() => startValue, x => {
            startValue = x;
            coinCounter.text = startValue.ToString();
            counter.text = startValue.ToString();
        }, endValue, 1f) // 1f is the duration of the animation, adjust as needed
        .SetEase(Ease.OutCubic); // Smooth easing for the animation
    }


    private bool IsCannonBought(int index)
    {
        return PlayerPrefs.GetInt(GetCannonBoughtKey(index), 0) == 1;
    }

    private string GetCannonBoughtKey(int index)
    {
        return "CannonBought_" + index;
    }

}
