using UnityEngine;
public class WomanTrigger : MonoBehaviour
{
    [Header("Normal Path")]
    public AudioSource audioSource;
    public AudioClip womanVoice;
    [Header("Knife Path")]
    public KnifePickup knifePickup;
    public AudioClip jamesConfrontVoice;
    public AudioClip womanConfrontVoice;
    private bool hasPlayed = false;
    private void OnTriggerEnter(Collider other)
    {
        if (hasPlayed)
            return;
        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))
        {
            hasPlayed = true;
            // 玩家拿着刀 → 对质路线
            if (knifePickup != null && knifePickup.HasKnife)
            {
                StartCoroutine(PlayKnifePath());
            }
            // 玩家没有刀 → 原来的正常路线
            else
            {
                PlayNormalPath();
            }
        }
    }
    private void PlayNormalPath()
    {
        if (audioSource != null && womanVoice != null)
        {
            audioSource.clip = womanVoice;
            audioSource.Play();
        }
        Debug.Log("Normal woman dialogue triggered.");
    }
    private System.Collections.IEnumerator PlayKnifePath()
    {
        // James 对质
        if (audioSource != null && jamesConfrontVoice != null)
        {
            audioSource.clip = jamesConfrontVoice;
            audioSource.Play();
            yield return new WaitWhile(() => audioSource.isPlaying);
        }
        // 女人回应
        if (audioSource != null && womanConfrontVoice != null)
        {
            audioSource.clip = womanConfrontVoice;
            audioSource.Play();
            yield return new WaitWhile(() => audioSource.isPlaying);
        }
        Debug.Log("Knife confrontation dialogue finished.");
    }
}