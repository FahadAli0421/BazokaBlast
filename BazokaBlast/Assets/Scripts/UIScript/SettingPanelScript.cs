using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPanelScript : MonoBehaviour
{
    [SerializeField] private float duration;
    [SerializeField] private bool snapping;
    [SerializeField] private Vector3 transformVectorIn;
    [SerializeField] private Vector3 transformVectorOut;

    public void MovePanelIn()
    {
        transform.DOLocalMove(transformVectorIn, duration, snapping);
    }

    public void MovePanelOut()
    {
        transform.DOLocalMove(transformVectorOut, duration, snapping);
    }
}
