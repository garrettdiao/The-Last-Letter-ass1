using UnityEngine;
public class StoneVoiceTrigger : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource voiceSource;
    public AudioClip stoneHintVoice;
    private bool hasTriggered = false;
    private void OnTriggerEnter(Collider other)
    {
        // 已经播放过就不再播放
        if (hasTriggered)
            return;
        // 检查进入区域的是不是玩家
        if (other.CompareTag("Player"))
        {
            hasTriggered = true;
            if (voiceSource != null && stoneHintVoice != null)
            {
                voiceSource.clip = stoneHintVoice;
                voiceSource.Play();
            }
            Debug.Log("Stone hint voice played.");
        }
    }
}