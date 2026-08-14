using UnityEngine;
using System.Collections;
public class StudyChoiceHint : MonoBehaviour
{
    [Header("Story References")]
    public IDCardInvestigation idCardInvestigation;
    public KnifePickup knifePickup;
    [Header("Audio")]
    public AudioSource voiceSource;
    public AudioClip studyChoiceVoice;
    [Header("Hover Settings")]
    public float hoverDelay = 1.0f;
    private bool hasPlayed = false;
    private Coroutine hoverCoroutine;
    // EZPZ On Hover Enter
    public void StartHover()
    {
        // 已经播放过 → 永久不再播放
        if (hasPlayed)
            return;
        // ★ 核心条件：
        // 玩家必须已经点击并打开过 ID
        if (idCardInvestigation == null)
            return;
        if (!idCardInvestigation.HasRevealed)
        {
            Debug.Log("Study Choice locked: ID has not been revealed.");
            return;
        }
        // 已经拿刀 → 玩家已经选择对质方向
        if (knifePickup != null && knifePickup.HasKnife)
        {
            Debug.Log("Study Choice locked: player has knife.");
            return;
        }
        // 满足条件，开始计算 Hover 1 秒
        if (hoverCoroutine != null)
            StopCoroutine(hoverCoroutine);
        hoverCoroutine = StartCoroutine(HoverRoutine());
    }
    // EZPZ On Hover Exit
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
        // 1秒后再次确认 ID 状态
        if (idCardInvestigation == null ||
            !idCardInvestigation.HasRevealed)
        {
            hoverCoroutine = null;
            yield break;
        }
        // 再确认玩家没有拿刀
        if (knifePickup != null && knifePickup.HasKnife)
        {
            hoverCoroutine = null;
            yield break;
        }
        PlayChoiceVoice();
        hoverCoroutine = null;
    }
    private void PlayChoiceVoice()
    {
        if (hasPlayed)
            return;
        if (voiceSource == null || studyChoiceVoice == null)
            return;
        hasPlayed = true;
        voiceSource.Stop();
        voiceSource.clip = studyChoiceVoice;
        voiceSource.Play();
        Debug.Log("Study police choice hint triggered.");
    }
}