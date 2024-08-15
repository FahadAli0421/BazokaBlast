using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class TypingScript : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] sentences;
    public AudioSource audioSource;
    public AudioClip typeSound;
    public float typingSpeed = 0.1f;
    public UnityEvent onSentenceComplete;

    private int currentSentenceIndex = 0;
    private Coroutine typingCoroutine;
    private bool isTyping = false;

    private void Start()
    {
        StartTyping();
    }

    public void OnTextBoxClick() // Call this method when the text box is clicked
    {
        if (isTyping)
        {
            CompleteSentence();
        }
    }

    public void StartTyping()
    {
        if (currentSentenceIndex < sentences.Length)
        {
            typingCoroutine = StartCoroutine(TypeSentence(sentences[currentSentenceIndex]));
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        textComponent.text = "";
        isTyping = true;

        foreach (char letter in sentence)
        {
            textComponent.text += letter;
            PlayTypeSoundEffect();
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        currentSentenceIndex++;
        yield return new WaitForSeconds(1f);
        StartTyping();
    }

    void PlayTypeSoundEffect()
    {
        if (audioSource != null && typeSound != null)
        {
            audioSource.PlayOneShot(typeSound);
        }
    }

    void CompleteSentence()
    {
        textComponent.text = sentences[currentSentenceIndex];
        isTyping = false;

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        if (onSentenceComplete != null)
        {
            onSentenceComplete.Invoke();
        }

        currentSentenceIndex++;
        StartTyping();
    }
}
