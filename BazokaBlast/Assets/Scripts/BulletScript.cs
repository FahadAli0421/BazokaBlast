using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float delay = 2f;
    public Transform firePoint;
    private Vector3 startPos;
    private Quaternion startRotation;
    private CannonController cannonController;

    private void Awake()
    {
        cannonController = FindAnyObjectByType<CannonController>();

        firePoint = cannonController.firePoint;
    }

    private void Start()
    {
        /*startPos = cannonController.cannonBallStartPos;
        startRotation = cannonController.cannonBallStartRotation;*/
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Ground" )
        {
            StartCoroutine(DeactivateAfterDelay());
        }
    }
    private IEnumerator DeactivateAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
        transform.position = firePoint.position;
        transform.rotation = firePoint.rotation;
    }
}
