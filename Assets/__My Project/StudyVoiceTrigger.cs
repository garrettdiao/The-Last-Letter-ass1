using UnityEngine;
using System.Collections;
public class StudyVoiceTrigger : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip jamesStudyVoice;
    [Header("Story Reference")]
    public IDCardInvestigation idCardInvestigation;
    [Header("Settings")]
    public float delayBeforeVoice = 1f;
    private bool hasPlayed = false;
    void OnTriggerEnter(Collider other)
    {
        if (hasPlayed)
            return;
        bool isPlayer =
            other.CompareTag("Player") ||
            other.transform.root.CompareTag("Player");
        if (!isPlayer)
            return;
        // =========================
        // 已经发现 Taylor 的真实身份
        // =========================
        if (idCardInvestigation != null &&
            idCardInvestigation.HasRevealed)
        {
            Debug.Log(
                "Player already knows the truth. Study voice skipped."
            );
            return;
        }
        // =========================
        // 还不知道真相 → 原来的剧情
        // =========================
        hasPlayed = true;
        StartCoroutine(PlayVoiceAfterDelay());
    }
    IEnumerator PlayVoiceAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeVoice);
        if (audioSource != null && jamesStudyVoice != null)
        {
            audioSource.clip = jamesStudyVoice;
            audioSource.Play();
        }
        Debug.Log("Original study voice played.");
    }
}