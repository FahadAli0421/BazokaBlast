using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using QAudioManager;

public class EnemyAnimations : MonoBehaviour
{
    private Animator animator;
    private bool isFalling = false;
    private Vector3 initialPosition;

    private const float FallOffset = 0.1f;
    private int currentAnimationIndex = 0;
    private string[] animations = { "laughing", "pointing", "silly_dance" };

    private Dictionary<string, float> animationLengths = new Dictionary<string, float>();

    public AudioClip deathSound;
    public GameObject deathEffect;

    void Start()
    {
        animator = GetComponent<Animator>();
        initialPosition = transform.position;

        CacheAnimationLengths();
        SetRandomAnimationOrder();
        PlayCurrentAnimation();
    }

    void Update()
    {
        if (!isFalling)
        {
            HandleFalling();
        }
    }

    private void CacheAnimationLengths()
    {
        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            animationLengths[clip.name] = clip.length;
        }
    }

    private void HandleFalling()
    {
        if (transform.position.y < initialPosition.y - FallOffset)
        {
            SetFalling(true);
        }
        else if (transform.position.y < -10)
        {
            HandleDeath();
        }
        else
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.normalizedTime >= 1)
            {
                currentAnimationIndex = (currentAnimationIndex + 1) % animations.Length;
                PlayCurrentAnimation();
            }
        }
    }

    private void SetRandomAnimationOrder()
    {
        Shuffle(animations);
    }

    private void Shuffle<T>(T[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            T temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }

    private void PlayCurrentAnimation()
    {
        string currentAnimation = animations[currentAnimationIndex];
        animator.Play(currentAnimation);
    }

    public void SetFalling(bool falling)
    {
        isFalling = falling;
        if (falling)
        {
            PlayFallAnimation();
            FindObjectOfType<AudioManager>().Play("scream");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("CannonBall") || collision.gameObject.CompareTag("Ground"))
        {
            HandleDeath();
        }
    }

    private void HandleDeath()
    {
        if (deathSound != null || deathEffect != null)
        {
            StartCoroutine(HandleDeathCoroutine());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator HandleDeathCoroutine()
    {
        if (deathSound != null)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
        }

        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        yield return new WaitForSeconds(1.0f);

        Destroy(gameObject);
    }

    private void PlayFallAnimation()
    {
        animator.Play("fall");
    }
}
