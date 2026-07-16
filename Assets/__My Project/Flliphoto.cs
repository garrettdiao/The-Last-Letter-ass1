using UnityEngine;
using System.Collections;
public class FlipPhoto : MonoBehaviour
{
    [Header("Photo")]
    public Transform photoRoot;
    [Header("Audio")]
    public AudioSource voiceSource;
    public AudioClip revealVoice;
    [Header("Story")]
    public DoorbellEvent doorbellEvent;
    public GameObject blackScreen;
    [Header("Animation")]
    public float flipDuration = 0.6f;
    private bool hasFlipped = false;
    // 给 EZPZ Interactable 调用
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
        Quaternion startRotation = photoRoot.rotation;
        Quaternion endRotation =
            photoRoot.rotation * Quaternion.Euler(0f, 180f, 0f);
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
        yield return new WaitForSeconds(0.5f);
        if (doorbellEvent != null)
        {
            doorbellEvent.PlaySuspenseMusic();
        }
        if (voiceSource != null && revealVoice != null)
        {
            voiceSource.clip = revealVoice;
            voiceSource.Play();
            yield return new WaitWhile(() => voiceSource.isPlaying);
        }
        if (doorbellEvent != null)
        {
            doorbellEvent.StopSuspenseMusic();
        }
        if (blackScreen != null)
        {
            blackScreen.SetActive(true);
        }
    }
    // 保留鼠标测试，可删除
    private void OnMouseDown()
    {
        TriggerPhotoFlip();
    }
}