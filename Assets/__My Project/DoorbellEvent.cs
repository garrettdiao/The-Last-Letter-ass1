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
    public AudioClip unlockSound;
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
    [Header("Front Door")]
    public GameObject frontDoor;
    public Transform frontDoorHinge;
    [Header("Key Door Settings")]
    public float openAngle = 90f;
    public float openSpeed = 100f;
    private bool hasTriggered = false;
    private bool canChooseEntry = false;
    private bool hasEnteredHouse = false;
    private bool hasKey = false;
    private bool hasStone = false;
    // 钥匙系统
    public void GiveKey()
    {
        hasKey = true;
        Debug.Log("Player now has the spare key.");
    }
    public void GiveStone()
    {
        hasStone = true;
        Debug.Log("Player now has the stone.");
    }
    public bool HasStone()
    {
        return hasStone;
    }
    public bool HasKey()
    {
        return hasKey;
    }
    // 门铃
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
        // 门铃响6次
        for (int i = 0; i < 6; i++)
        {
            sfxSource.PlayOneShot(doorbellSound);
            yield return new WaitForSeconds(1.2f);
        }
        // 女声1
        voiceSource.clip = femaleDoorbell1;
        voiceSource.Play();
        yield return new WaitWhile(() => voiceSource.isPlaying);
        // 男声1
        voiceSource.clip = maleDoorbell1;
        voiceSource.Play();
        yield return new WaitWhile(() => voiceSource.isPlaying);
        // 女声2
        voiceSource.clip = femaleDoorbell2;
        voiceSource.Play();
        yield return new WaitWhile(() => voiceSource.isPlaying);
        // 尖叫
        sfxSource.PlayOneShot(screamSound);
        yield return new WaitForSeconds(0.8f);
        // 玻璃破碎
        sfxSource.PlayOneShot(glassBreakSound);
        yield return new WaitForSeconds(1f);
        // 停止环境音
        if (environmentSource != null)
        {
            environmentSource.Stop();
        }
        // 开始悬疑音乐
        PlaySuspenseMusic();
        // 男声2
        voiceSource.clip = maleDoorbell2;
        voiceSource.Play();
        yield return new WaitWhile(() => voiceSource.isPlaying);
        // James：需要寻找其它进入方式
        voiceSource.clip = maleNeedAnotherWay;
        voiceSource.Play();
        yield return new WaitWhile(() => voiceSource.isPlaying);
        // 开放进入方式
        canChooseEntry = true;
        Debug.Log("Doorbell sequence finished.");
    }
    // ==============================
    // Front Door Interaction
    // EZPZ Desktop / VR 都调用这里
    // ==============================
    public void TryForceDoor()
    {
        if (!canChooseEntry)
        {
            Debug.Log("Finish the doorbell event first.");
            return;
        }
        if (hasEnteredHouse)
            return;
        // 有钥匙：正常开门
        if (hasKey)
        {
            StartCoroutine(OpenDoorWithKey());
        }
        // 没钥匙：维持原来的强行破门
        else
        {
            StartCoroutine(ForceDoor());
        }
    }
    // 强行破门
    IEnumerator ForceDoor()
    {
        hasEnteredHouse = true;
        if (doorBreakSound != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(doorBreakSound);
        }
        yield return new WaitForSeconds(0.5f);
        if (frontDoor != null)
        {
            frontDoor.SetActive(false);
        }
        Debug.Log("Player entered by forcing the door.");
    }
    // 用钥匙正常开门
    IEnumerator OpenDoorWithKey()
    {
        hasEnteredHouse = true;
        if (unlockSound != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(unlockSound);
        }
        yield return new WaitForSeconds(0.3f);
        if (frontDoor == null || frontDoorHinge == null)
        {
            Debug.LogWarning("Front Door or Front Door Hinge is not assigned.");
            yield break;
        }
        float rotated = 0f;
        while (rotated < Mathf.Abs(openAngle))
        {
            float step = openSpeed * Time.deltaTime;
            if (rotated + step > Mathf.Abs(openAngle))
            {
                step = Mathf.Abs(openAngle) - rotated;
            }
            float direction = openAngle >= 0 ? 1f : -1f;
            frontDoor.transform.RotateAround(
                frontDoorHinge.position,
                Vector3.up,
                step * direction
            );
            rotated += step;
            yield return null;
        }
        Debug.Log("Player opened the front door with the spare key.");
    }
    // ==========================
    // Music Functions
    // ==========================
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