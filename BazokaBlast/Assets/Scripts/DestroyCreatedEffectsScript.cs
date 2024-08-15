using UnityEngine;

public class DestroyCreatedEffectsScript : MonoBehaviour
{
    void Start()
    {
        // Get the duration of the particle system
        float duration = GetComponent<ParticleSystem>().main.duration;

        // Disable the object after the duration of the particle system
        Invoke(nameof(DisableObject), duration);
    }

    void DisableObject()
    {
        gameObject.SetActive(false); // Disable the object after the particle system is done
    }
}
