using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMovementScript : MonoBehaviour
{
    [SerializeField] private Rigidbody cloudRb;
    [SerializeField] private float speed; // Change to float for smoother movement
    [SerializeField] private float startPosX;
    [SerializeField] private float endPosX;
    [SerializeField] private Transform cloud;

    private void Start()
{
    cloudRb = GetComponent<Rigidbody>();
    //cloudRb.isKinematic = true; // Make the Rigidbody kinematic
}

    void Update()
    {
        CloudMove();
        CloudMoveCheck();
    }

    private void CloudMove()
    {
        // Use the Rigidbody's velocity to move the cloud
        cloudRb.velocity = Vector3.forward * speed;
    }

    private void CloudMoveCheck()
    {
        if (cloud.transform.localPosition.x <= endPosX)
        {
            Debug.Log("GameObject Name : " + cloud.name + " cloud.transform.position.x" + cloud.transform.position.x);
            // Reset position only on the x-axis
            cloud.transform.localPosition = new Vector3(startPosX, cloud.transform.localPosition.y, cloud.transform.localPosition.z);
        }
    }
}
