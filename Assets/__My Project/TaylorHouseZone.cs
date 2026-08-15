using UnityEngine;
public class TaylorHouseZone : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource voiceSource;
    public AudioClip houseConfirmVoice;
    private bool hasPlayed = false;
    private void OnTriggerEnter(Collider other)
    {
        if (hasPlayed)
            return;
        bool isPlayer =
            other.CompareTag("Player") ||
            other.transform.root.CompareTag("Player");
        if (!isPlayer)
            return;
        hasPlayed = true;
        if (voiceSource != null &&
            houseConfirmVoice != null)
        {
            voiceSource.Stop();
            voiceSource.clip = houseConfirmVoice;
            voiceSource.Play();
        }
        Debug.Log("Taylor house confirmed.");
    }
}