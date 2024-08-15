using UnityEngine;
using DG.Tweening;

public class TargetManager : MonoBehaviour
{
    public GameObject[] targets; // Array of target GameObjects
    public GameObject[] barrels; // Array of barrel GameObjects
    public float spawnDuration = 4f; // Time before target despawns
    public float despawnDuration = 8f; // Time before spawning a new target

    // Reference to the CannonController
    public CannonController cannonController;
    private CurrencyManager currency;

    private Vector3 targetScale = new Vector3(1.5f, 0.2357201f, 1.5f); // Target's desired scale
    private Quaternion targetRotation = Quaternion.Euler(0, 0, 90); // Target's desired rotation

    void Start()
    {
        // Ensure that CannonController is set up
        if (cannonController == null)
        {
            cannonController = FindObjectOfType<CannonController>();
        }

        if (currency == null)
        {
            currency = FindObjectOfType<CurrencyManager>();
        }

        SpawnTarget();
    }

    void SpawnTarget()
    {
        // Select a random target to spawn
        GameObject selectedTarget = targets[Random.Range(0, targets.Length)];
        selectedTarget.SetActive(true);

        // Set target's initial rotation
        selectedTarget.transform.rotation = targetRotation;

        // Animate target: spin from scale 0 to the specified scale
        selectedTarget.transform.localScale = Vector3.zero;
        selectedTarget.transform.DOScale(targetScale, 1f).SetEase(Ease.OutBack);
        selectedTarget.transform.DORotate(new Vector3(0, 0, 450), 1f, RotateMode.FastBeyond360);

        // Schedule target despawn
        Invoke(nameof(DespawnTarget), spawnDuration);
    }

    void DespawnTarget()
    {
        foreach (var target in targets)
        {
            if (target.activeSelf)
            {
                // Animate target despawn: spin and scale down to 0
                target.transform.DOScale(Vector3.zero, 1f).SetEase(Ease.InBack)
                    .OnComplete(() => target.SetActive(false));
                target.transform.DORotate(new Vector3(0, 0, 360), 1f, RotateMode.FastBeyond360);

                break;
            }
        }

        // Schedule the next target spawn after some delay
        Invoke(nameof(SpawnTarget), despawnDuration);
    }

    public void OnTargetHit(GameObject target)
    {
        // Handle random reward logic here (coins, ammo, or barrel)
        GiveRandomReward();

        // Immediately despawn the target on hit
        target.transform.DOScale(Vector3.zero, 1f).SetEase(Ease.InBack)
            .OnComplete(() => target.SetActive(false));
        target.transform.DORotate(new Vector3(0, 0, 360), 1f, RotateMode.FastBeyond360);
    }

    void GiveRandomReward()
    {
        int rewardType = Random.Range(0, 3);
        switch (rewardType)
        {
            case 0: // Reward coins
                int coinAmount = Random.Range(1, 100); // Random amount
                int currentCoins = PlayerPrefs.GetInt("CountDollar", 0);
                PlayerPrefs.SetInt("CountDollar", currentCoins + coinAmount);
                PlayerPrefs.Save();
                currency.UpdateCoins();
                break;
            case 1: // Reward ammo
                int ammoAmount = Random.Range(1, 10); // Random amount
                if (cannonController != null)
                {
                    cannonController.cannonBallCount += ammoAmount;
                }
                cannonController.CannonBallCountUpdate();
                break;
            case 2: // Spawn a random barrel as a reward
                SpawnBarrel();
                break;
        }
    }

    void SpawnBarrel()
    {
        // Select a random barrel to spawn
        GameObject selectedBarrel = barrels[Random.Range(0, barrels.Length)];
        selectedBarrel.SetActive(true);

        // Animate barrel: scale up with bounce effect
        selectedBarrel.transform.localScale = Vector3.zero;
        selectedBarrel.transform.DOScale(Vector3.one, 1f).SetEase(Ease.OutElastic);

        // Schedule barrel despawn
        //Invoke(nameof(DespawnBarrel), despawnDuration);
    }

    void DespawnBarrel()
    {
        foreach (var barrel in barrels)
        {
            if (barrel.activeSelf)
            {
                // Animate barrel despawn: scale down with bounce effect
                barrel.transform.DOScale(Vector3.zero, 1f).SetEase(Ease.InElastic)
                    .OnComplete(() => barrel.SetActive(false));

                break;
            }
        }
    }
}
