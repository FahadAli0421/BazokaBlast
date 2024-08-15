//this script is attached to enemy
using UnityEngine;

public class EnemyDestroyScript : MonoBehaviour
{
    public GameObject destructionEffect;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("CannonBall"))
        {
            if (destructionEffect != null)
            {
                Instantiate(destructionEffect, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }
}
