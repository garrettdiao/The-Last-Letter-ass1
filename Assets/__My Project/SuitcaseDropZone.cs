using UnityEngine;
using System.Collections;

public class SuitcaseDropZone : MonoBehaviour
{
    public SuitcasePickup suitcase;
    public Transform placePoint;

    public AudioSource audioSource;
    public AudioClip womanVoice;

    public DoorbellEvent doorbellEvent;

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
            StartCoroutine(PlaceSuitcase());
        }
    }

    IEnumerator PlaceSuitcase()
    {
        suitcase.PutDown(placePoint);

        if (doorbellEvent != null)
        {
            doorbellEvent.StopSuspenseMusic();
        }

        if (audioSource != null && womanVoice != null)
        {
            audioSource.clip = womanVoice;
            audioSource.Play();
            yield return new WaitWhile(() => audioSource.isPlaying);
        }

        Debug.Log("Now player should go to the study.");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInside = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInside = false;
    }
}
