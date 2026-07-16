using UnityEngine;
using System.Collections;
public class SuitcaseDropZone : MonoBehaviour
{
    [Header("Suitcase")]
    public SuitcasePickup suitcase;
    public Transform placePoint;
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip womanVoice;
    [Header("Story")]
    public DoorbellEvent doorbellEvent;
    private bool hasTriggered = false;
    // 给 EZPZ Interactable 调用
    public void TryPlaceSuitcase()
    {
        if (hasTriggered)
        {
            Debug.Log("The suitcase has already been placed.");
            return;
        }
        if (suitcase == null)
        {
            Debug.LogWarning("Suitcase is not assigned.");
            return;
        }
        if (!suitcase.IsPickedUp())
        {
            Debug.Log("The player has not picked up the suitcase.");
            return;
        }
        if (placePoint == null)
        {
            Debug.LogWarning("Place Point is not assigned.");
            return;
        }
        hasTriggered = true;
        StartCoroutine(PlaceSuitcase());
    }
    private IEnumerator PlaceSuitcase()
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
        Debug.Log("Now the player should go to the study.");
    }
}