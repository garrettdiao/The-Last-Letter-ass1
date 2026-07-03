using UnityEngine;
using System.Collections;

public class StudyVoiceTrigger : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip jamesStudyVoice;

    public float delayBeforeVoice = 1f;

    private bool hasPlayed = false;

    void OnTriggerEnter(Collider other)
    {
        if (hasPlayed) return;

        if (other.CompareTag("Player"))
        {
            hasPlayed = true;
            StartCoroutine(PlayVoiceAfterDelay());
        }
    }

    IEnumerator PlayVoiceAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeVoice);

        if (audioSource != null && jamesStudyVoice != null)
        {
            audioSource.clip = jamesStudyVoice;
            audioSource.Play();
        }
    }
}