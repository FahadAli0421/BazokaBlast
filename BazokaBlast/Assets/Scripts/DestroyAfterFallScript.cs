using UnityEngine;

public class DestroyAfterFallScript : MonoBehaviour
{
    bool initiallyGrounded = false;
    public GameObject destructionEffect;
    private bool isCollidingWithGround = false;
    private BuildingManager buildManager;

    void Start()
    {
        CheckInitialGrounded();
        buildManager = FindAnyObjectByType<BuildingManager>();
    }

    void Update()
    {
        if (transform.position.y <= -10)
        {
            TriggerDestruction();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isCollidingWithGround = true;

            if (!initiallyGrounded)
            {
                TriggerDestruction();
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (initiallyGrounded && collision.gameObject.CompareTag("Ground"))
        {
            isCollidingWithGround = false;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (initiallyGrounded && isCollidingWithGround && collision.gameObject.CompareTag("Ground") && collision.contacts.Length == 1)
        {
            bool onlyGroundContact = true;

            foreach (ContactPoint contact in collision.contacts)
            {
                if (!contact.otherCollider.CompareTag("Ground"))
                {
                    onlyGroundContact = false;
                    break;
                }
            }

            if (onlyGroundContact)
            {
                TriggerDestruction();
            }
        }
    }

    void CheckInitialGrounded()
    {
        RaycastHit hit;
        Vector3 raycastOrigin = transform.position + Vector3.up * 0.2f;
        if (Physics.Raycast(raycastOrigin, Vector3.down, out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject.CompareTag("Ground"))
            {
                initiallyGrounded = true;
            }
        }
    }

    void TriggerDestruction()
    {
        if (destructionEffect != null)
        {
            Instantiate(destructionEffect, transform.position, Quaternion.identity);
        }

        // Disable the object instead of destroying it
        gameObject.SetActive(false);

        // Call the existing method in the BuildingManager to handle logic after the block is "destroyed"
        buildManager.BlockDestroyed();
    }
}
