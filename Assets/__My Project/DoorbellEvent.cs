using UnityEngine;
using System.Collections;
public class DoorbellEvent : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource sfxSource;
    public AudioSource voiceSource;
    public AudioSource musicSource;
    public AudioSource environmentSource;
    [Header("Sound Effects")]
    public AudioClip doorbellSound;
    public AudioClip screamSound;
    public AudioClip glassBreakSound;
    public AudioClip doorBreakSound;
    [Header("Voice Clips")]
    public AudioClip femaleDoorbell1;
    public AudioClip maleDoorbell1;
    public AudioClip femaleDoorbell2;
    public AudioClip maleDoorbell2;
    public AudioClip maleNeedAnotherWay;
    [Header("Music")]
    public AudioClip suspenseMusic;
    [Header("Interaction")]
    public Transform player;
    public float interactDistance = 5f;
    [Header("Door")]
    public GameObject frontDoor;
    private bool hasTriggered = false;
    public void TriggerDoorbell()
    {
        if (hasTriggered)
            return;
        hasTriggered = true;
        StartCoroutine(PlayDoorbellSequence());
    }
    void OnMouseDown()
    {
        if (hasTriggered)
            return;
        if (player == null)
        {
            Debug.Log("Player not assigned.");
            return;
        }
        float distance = Vector3.Distance(
            player.position,
            transform.position
        );
        if (distance <= interactDistance)
        {
            TriggerDoorbell();
        }
        else
        {
            Debug.Log("Too far from doorbell.");
        }
    }
    IEnumerator PlayDoorbellSequence()
    {
        // 门铃响六次
        for (int i = 0; i < 6; i++)
        {
            if (sfxSource != null && doorbellSound != null)
            {
                sfxSource.PlayOneShot(doorbellSound);
            }
            yield return new WaitForSeconds(1.2f);
        }
        // 女声1
        if (voiceSource != null && femaleDoorbell1 != null)
        {
            voiceSource.clip = femaleDoorbell1;
            voiceSource.Play();
            yield return new WaitWhile(() => voiceSource.isPlaying);
        }
        // 男声1
        if (voiceSource != null && maleDoorbell1 != null)
        {
            voiceSource.clip = maleDoorbell1;
            voiceSource.Play();
            yield return new WaitWhile(() => voiceSource.isPlaying);
        }
        // 女声2
        if (voiceSource != null && femaleDoorbell2 != null)
        {
            voiceSource.clip = femaleDoorbell2;
            voiceSource.Play();
            yield return new WaitWhile(() => voiceSource.isPlaying);
        }
        // 尖叫
        if (sfxSource != null && screamSound != null)
        {
            sfxSource.PlayOneShot(screamSound);
        }
        yield return new WaitForSeconds(0.8f);
        // 玻璃破碎
        if (sfxSource != null && glassBreakSound != null)
        {
            sfxSource.PlayOneShot(glassBreakSound);
        }
        yield return new WaitForSeconds(1f);
        // 停止环境音
        if (environmentSource != null)
        {
            environmentSource.Stop();
        }
        // 开始悬疑音乐
        PlaySuspenseMusic();
        // 男声2
        if (voiceSource != null && maleDoorbell2 != null)
        {
            voiceSource.clip = maleDoorbell2;
            voiceSource.Play();
            yield return new WaitWhile(() => voiceSource.isPlaying);
            // James 提示玩家寻找其他进入方式
            voiceSource.clip = maleNeedAnotherWay;
            voiceSource.Play();
            yield return new WaitWhile(() => voiceSource.isPlaying);
        }
        // 门保持存在，等待玩家选择进入方式
        Debug.Log(
            "Doorbell sequence finished. Player can now choose how to enter."
        );
    }
    public void PlaySuspenseMusic()
    {
        if (musicSource == null || suspenseMusic == null)
            return;
        musicSource.Stop();
        musicSource.clip = suspenseMusic;
        musicSource.loop = true;
        musicSource.Play();
    }
    public void StopSuspenseMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }
    public void ChangeMusic(AudioClip newMusic, bool loop = true)
    {
        if (musicSource == null || newMusic == null)
            return;
        musicSource.Stop();
        musicSource.clip = newMusic;
        musicSource.loop = loop;
        musicSource.Play();
    }
}