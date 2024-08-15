using DG.Tweening;
using UnityEngine;

public class DOTweenInitializer : MonoBehaviour
{
    private void Awake()
    {
        // Initialize DOTween once for the entire project
        DOTween.Init();
    }
}
