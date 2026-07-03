using UnityEngine;
using System.Collections;

public class RevealTaylor : MonoBehaviour
{
    [Header("Interaction")]
    public Transform player;
    public float interactDistance = 3f;

    [Header("Audio")]
    public AudioSource voiceSource;
    public AudioClip jamesRevealVoice;

    [Header("Music")]
    public DoorbellEvent doorbellEvent;

    [Header("End Screen")]
    public GameObject blackScreen;

    private bool hasTriggered = false;

    void OnMouseDown()
    {
        if (hasTriggered) return;
        if (player == null) return;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance > interactDistance)
        {
            Debug.Log("Too far from reveal object.");
            return;
        }

        hasTriggered = true;
        StartCoroutine(RevealSequence());
    }

    IEnumerator RevealSequence()
    {
        // 重新播放悬疑音乐
        if (doorbellEvent != null)
        {
            doorbellEvent.PlaySuspenseMusic();
        }

        // 播放反转台词
        if (voiceSource != null && jamesRevealVoice != null)
        {
            voiceSource.clip = jamesRevealVoice;
            voiceSource.Play();
            yield return new WaitWhile(() => voiceSource.isPlaying);
        }

        // 台词结束后立刻停止音乐
        if (doorbellEvent != null)
        {
            doorbellEvent.StopSuspenseMusic();
        }

        // 黑屏
        if (blackScreen != null)
        {
            blackScreen.SetActive(true);
        }
    }
}