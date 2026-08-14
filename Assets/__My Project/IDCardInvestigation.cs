using UnityEngine;
using System.Collections;
public class IDCardInvestigation : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource voiceSource;
    public AudioClip noticeVoice;
    public AudioClip revealVoice;
    [Header("Music")]
    public DoorbellEvent doorbellEvent;
    [Header("ID Card")]
    public GameObject normalCard;
    public GameObject revealedCard;
    [Header("Hover")]
    public float hoverDelay = 0.5f;
    private bool noticePlayed = false;
    private bool hasRevealed = false;
    private Coroutine hoverCoroutine;
    public bool HasRevealed
    {
        get { return hasRevealed; }
    }
    // EZPZ Hover Enter
    public void StartHover()
    {
        if (noticePlayed || hasRevealed)
            return;
        if (hoverCoroutine != null)
        {
            StopCoroutine(hoverCoroutine);
        }
        hoverCoroutine = StartCoroutine(HoverRoutine());
    }
    // EZPZ Hover Exit
    public void StopHover()
    {
        if (hoverCoroutine != null)
        {
            StopCoroutine(hoverCoroutine);
            hoverCoroutine = null;
        }
    }
    private IEnumerator HoverRoutine()
    {
        yield return new WaitForSeconds(hoverDelay);
        if (!noticePlayed && !hasRevealed)
        {
            noticePlayed = true;
            if (voiceSource != null && noticeVoice != null)
            {
                voiceSource.Stop();
                voiceSource.clip = noticeVoice;
                voiceSource.Play();
            }
            Debug.Log("James noticed the ID card.");
        }
        hoverCoroutine = null;
    }
    // EZPZ Primary Interact
    public void RevealID()
    {
        if (hasRevealed)
            return;
        hasRevealed = true;
        StartCoroutine(RevealSequence());
    }
    private IEnumerator RevealSequence()
    {
        // 小 ID 消失
        if (normalCard != null)
        {
            normalCard.SetActive(false);
        }
        // 大 Taylor ID 出现
        if (revealedCard != null)
        {
            revealedCard.SetActive(true);
        }
        // 悬疑音乐重新响起
        if (doorbellEvent != null)
        {
            doorbellEvent.PlaySuspenseMusic();
        }
        yield return new WaitForSeconds(0.5f);
        // 播放反转台词
        if (voiceSource != null && revealVoice != null)
        {
            voiceSource.Stop();
            voiceSource.clip = revealVoice;
            voiceSource.Play();
            yield return new WaitWhile(() => voiceSource.isPlaying);
        }
        Debug.Log("Taylor's identity revealed.");
    }
}