using System.Collections;
using UnityEngine;
public class OpeningNarrationSequence : MonoBehaviour
{
    public AudioSource voiceSource;
    public AudioClip openingVoice;
    public AudioClip addressVoice;
    public float pauseBetween = 0.3f;
    private void Start()
    {
        StartCoroutine(PlaySequence());
    }
    private IEnumerator PlaySequence()
    {
        if (voiceSource == null)
            yield break;
        // 1. 原来的 James 开场白
        if (openingVoice != null)
        {
            voiceSource.clip = openingVoice;
            voiceSource.Play();
            yield return new WaitWhile(() => voiceSource.isPlaying);
        }
        // 2. 停顿一下
        yield return new WaitForSeconds(pauseBetween);
        // 3. 地址信息
        if (addressVoice != null)
        {
            voiceSource.clip = addressVoice;
            voiceSource.Play();
            yield return new WaitWhile(() => voiceSource.isPlaying);
        }
        Debug.Log("Opening narration finished.");
    }
}