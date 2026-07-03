using UnityEngine;

public class SuitcaseDropZone : MonoBehaviour
{
    public SuitcasePickup suitcase;
    public Transform placePoint;

    public AudioSource audioSource;
    public AudioClip womanVoice;

    private bool playerInside = false;
    private bool hasTriggered = false;

    void Update()
    {
        if (!playerInside) return;
        if (hasTriggered) return;
        if (suitcase == null || !suitcase.IsPickedUp()) return;

        if (Input.GetMouseButtonDown(0))
        {
            hasTriggered = true;

            suitcase.PutDown(placePoint);

            if (audioSource != null && womanVoice != null)
            {
                audioSource.clip = womanVoice;
                audioSource.Play();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
        }
    }
}