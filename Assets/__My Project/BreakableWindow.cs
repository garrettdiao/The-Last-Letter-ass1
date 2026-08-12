using UnityEngine;
using System.Collections;
public class BreakableWindow : MonoBehaviour
{
    [Header("Story")]
    public DoorbellEvent doorbellEvent;
    [Header("Window")]
    public GameObject window;
    [Header("Audio")]
    public AudioSource sfxSource;
    public AudioClip glassBreakSound;
    private bool hasBroken = false;
    // EZPZ Desktop / VR 都调用这个
    public void TryBreakWindow()
    {
        if (hasBroken)
            return;
        if (doorbellEvent == null)
        {
            Debug.LogWarning("DoorbellEvent is not assigned.");
            return;
        }
        // 检查玩家有没有石头
        if (!doorbellEvent.HasStone())
        {
            Debug.Log("You need a stone to break the window.");
            return;
        }
        StartCoroutine(BreakWindow());
    }
    private IEnumerator BreakWindow()
    {
        hasBroken = true;
        // 播放玻璃破碎音效
        if (sfxSource != null && glassBreakSound != null)
        {
            sfxSource.PlayOneShot(glassBreakSound);
        }
        // 稍微等一下，让声音先开始
        yield return new WaitForSeconds(0.3f);
        // 整个窗户消失
        if (window != null)
        {
            window.SetActive(false);
        }
        Debug.Log("Window broken with the stone.");
    }
}