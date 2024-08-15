using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGBullet : MonoBehaviour
{
    public float BulletSpeed;
    [SerializeField]
    private Rigidbody rb;

    void Start()
    {
       
    }

    void Update()
    {
        
        
    }
    private void FixedUpdate()
    {
        rb.AddForce(transform.forward * BulletSpeed * Time.deltaTime, ForceMode.Impulse);
    }
    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject, 4f);
        //if (other.gameObject.CompareTag("Player"))
        //{
        //    Debug.Log("Hit on Player");
        //    Destroy(gameObject);
        //}
        //else if(other.gameObject.CompareTag("Line"))
        //{
        //    Destroy(other.gameObject);
        //    Destroy(gameObject);
        //}
        //else
        //{
            
            
        //}
    }
}
