using UnityEngine;

public class TargetCollision : MonoBehaviour
{
    private TargetManager targetManager;

    void Start()
    {
        // Find the TargetManager in the scene
        targetManager = FindObjectOfType<TargetManager>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the object that collided with the target is a bullet
        if (collision.gameObject.CompareTag("CannonBall"))
        {
            // Call the OnTargetHit method on the targetManager
            targetManager.OnTargetHit(gameObject);

            // Optionally, destroy the bullet upon impact
            Destroy(collision.gameObject);
        }
    }
}
