using UnityEngine;
using System.Collections;
public class WomanTrigger : MonoBehaviour
{
    [Header("Normal Path")]
    public AudioSource audioSource;
    public AudioClip womanVoice;
    [Header("Knife Path")]
    public KnifePickup knifePickup;
    public AudioClip jamesConfrontVoice;
    public AudioClip womanConfrontVoice;
    // 两条剧情分别记录
    private bool normalPathPlayed = false;
    private bool knifePathPlayed = false;
    private void OnTriggerEnter(Collider other)
    {
        bool isPlayer =
            other.CompareTag("Player") ||
            other.transform.root.CompareTag("Player");
        if (!isPlayer)
            return;
        Debug.Log("Player entered WomanTrigger.");
        // =========================
        // 玩家已经拿刀
        // =========================
        if (knifePickup != null && knifePickup.HasKnife)
        {
            // 防止刀路线重复触发
            if (knifePathPlayed)
                return;
            knifePathPlayed = true;
            Debug.Log("Knife detected. Starting confrontation path.");
            StartCoroutine(PlayKnifePath());
            return;
        }
        // =========================
        // 玩家没有刀
        // =========================
        if (!normalPathPlayed)
        {
            normalPathPlayed = true;
            Debug.Log("Starting normal woman dialogue.");
            PlayNormalPath();
        }
    }
    private void PlayNormalPath()
    {
        if (audioSource != null && womanVoice != null)
        {
            audioSource.Stop();
            audioSource.clip = womanVoice;
            audioSource.Play();
        }
        Debug.Log("Normal woman dialogue triggered.");
    }
    private IEnumerator PlayKnifePath()
    {
        // James 对质
        if (audioSource != null && jamesConfrontVoice != null)
        {
            audioSource.Stop();
            audioSource.clip = jamesConfrontVoice;
            audioSource.Play();
            yield return new WaitWhile(() => audioSource.isPlaying);
        }
        // 女人回应
        if (audioSource != null && womanConfrontVoice != null)
        {
            audioSource.Stop();
            audioSource.clip = womanConfrontVoice;
            audioSource.Play();
            yield return new WaitWhile(() => audioSource.isPlaying);
        }
        Debug.Log("Knife confrontation dialogue finished.");
    }
}
