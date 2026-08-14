using UnityEngine;
using System.Collections;
public class FlipPhoto : MonoBehaviour
{
    [Header("Photo")]
    public Transform photoRoot;
    [Header("Audio")]
    public AudioSource voiceSource;
    public AudioClip revealVoice;
    public AudioClip policeDecisionVoice;
    [Header("Story")]
    public DoorbellEvent doorbellEvent;
    [Header("Animation")]
    public float flipDuration = 0.6f;
    private bool hasFlipped = false;
    // 后面的电话脚本可以读取这个状态
    public bool HasRevealedTruth { get; private set; } = false;
    // 给 EZPZ Desktop / VR 调用
    public void TriggerPhotoFlip()
    {
        if (hasFlipped)
            return;
        hasFlipped = true;
        StartCoroutine(FlipAndReveal());
        Debug.Log("Photo flip triggered.");
    }
    private IEnumerator FlipAndReveal()
    {
        if (photoRoot == null)
        {
            photoRoot = transform;
        }
        // =========================
        // 翻转照片
        // =========================
        Quaternion startRotation = photoRoot.rotation;
        Quaternion endRotation =
            photoRoot.rotation *
            Quaternion.Euler(0f, 180f, 0f);
        float timer = 0f;
        while (timer < flipDuration)
        {
            timer += Time.deltaTime;
            photoRoot.rotation = Quaternion.Slerp(
                startRotation,
                endRotation,
                timer / flipDuration
            );
            yield return null;
        }
        photoRoot.rotation = endRotation;
        // 给玩家一点时间看清照片
        yield return new WaitForSeconds(0.5f);
        // =========================
        // 身份反转
        // =========================
        // 悬疑音乐重新响起
        if (doorbellEvent != null)
        {
            doorbellEvent.PlaySuspenseMusic();
        }
        // 原来的身份反转台词
        if (voiceSource != null && revealVoice != null)
        {
            voiceSource.Stop();
            voiceSource.clip = revealVoice;
            voiceSource.Play();
            yield return new WaitWhile(
                () => voiceSource.isPlaying
            );
        }
        // 玩家现在正式知道真相
        HasRevealedTruth = true;
        Debug.Log(
            "James discovered the truth through the photograph."
        );
        // 稍微停顿一下
        yield return new WaitForSeconds(0.5f);
        // James 决定报警
        if (voiceSource != null &&
            policeDecisionVoice != null)
        {
            voiceSource.Stop();
            voiceSource.clip = policeDecisionVoice;
            voiceSource.Play();
            yield return new WaitWhile(
                () => voiceSource.isPlaying
            );
        }
        // 决定报警以后停止悬疑音乐
        if (doorbellEvent != null)
        {
            doorbellEvent.StopSuspenseMusic();
        }
        Debug.Log(
            "Photo route finished. Player can now call the police."
        );
    }
}