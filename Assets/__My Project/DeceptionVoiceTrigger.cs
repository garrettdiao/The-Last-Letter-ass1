using UnityEngine;
public class DeceptionVoiceTrigger : MonoBehaviour
{
    [Header("Story References")]
    public IDCardInvestigation idCardInvestigation;
    public KnifePickup knifePickup;
    [Header("Audio")]
    public AudioSource voiceSource;
    public AudioClip deceptionVoice;
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
        // 必须已经发现 ID
        if (idCardInvestigation == null || !idCardInvestigation.HasRevealed)
            return;
        // 如果已经拿刀，就走 Knife Path，不播放这段
        if (knifePickup != null && knifePickup.HasKnife)
            return;
        hasPlayed = true;
        if (voiceSource != null && deceptionVoice != null)
        {
            voiceSource.Stop();
            voiceSource.clip = deceptionVoice;
            voiceSource.Play();
        }
        Debug.Log("Deception motivation voice triggered.");
    }
}