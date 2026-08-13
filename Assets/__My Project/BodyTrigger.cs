using UnityEngine;
using System.Collections;

public class BodyTrigger : MonoBehaviour
{
    public AudioSource voiceSource;
    public AudioClip femaleVoice;

    private bool playerInside = false;
    private bool hasPlayed = false;

    private Coroutine triggerCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (hasPlayed) return;

        if (other.CompareTag("Player"))
        {
            playerInside = true;
            triggerCoroutine = StartCoroutine(WaitAndPlay());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;

            if (triggerCoroutine != null)
            {
                StopCoroutine(triggerCoroutine);
            }
        }
    }

    IEnumerator WaitAndPlay()
    {
        yield return new WaitForSeconds(10f);

        if (playerInside && !hasPlayed)
        {
            hasPlayed = true;

            if (voiceSource != null && femaleVoice != null)
            {
                voiceSource.clip = femaleVoice;
                voiceSource.Play();
            }
        }
    }
}