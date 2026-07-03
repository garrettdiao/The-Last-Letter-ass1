using UnityEngine;
using System.Collections;

public class WomanTrigger : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip womanVoice;

    private bool hasPlayed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasPlayed) return;

        if (other.CompareTag("Player"))
        {
            hasPlayed = true;

            if (audioSource != null && womanVoice != null)
            {
                audioSource.clip = womanVoice;
                audioSource.Play();
            }
        }
    }
}
