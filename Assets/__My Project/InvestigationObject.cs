using UnityEngine;
using System.Collections;
public class InvestigationObject : MonoBehaviour
{
    [Header("Investigation Voice")]
    public AudioSource voiceSource;
    public AudioClip investigationVoice;
    [Header("Hover Settings")]
    public float hoverDelay = 1.0f;
    private bool hasPlayed = false;
    private Coroutine hoverCoroutine;
    // EZPZ On Hover Enter 调用
    public void StartHover()
    {
        if (hasPlayed)
            return;
        if (hoverCoroutine != null)
            StopCoroutine(hoverCoroutine);
        hoverCoroutine = StartCoroutine(HoverDelayRoutine());
    }
    // EZPZ On Hover Exit 调用
    public void StopHover()
    {
        if (hoverCoroutine != null)
        {
            StopCoroutine(hoverCoroutine);
            hoverCoroutine = null;
        }
    }
    private IEnumerator HoverDelayRoutine()
    {
        yield return new WaitForSeconds(hoverDelay);
        PlayInvestigationVoice();
        hoverCoroutine = null;
    }
    private void PlayInvestigationVoice()
    {
        if (hasPlayed)
            return;
        if (voiceSource == null || investigationVoice == null)
        {
            Debug.LogWarning("Investigation audio is not assigned.");
            return;
        }
        hasPlayed = true;
        voiceSource.Stop();
        voiceSource.clip = investigationVoice;
        voiceSource.Play();
        Debug.Log("Investigated: " + gameObject.name);
    }
}