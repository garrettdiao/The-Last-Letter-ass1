using UnityEngine;

using System.Collections;
using StarterAssets;

public class WomanTrigger : MonoBehaviour

{
    [Header("Ending Sequence")]
    public AudioClip finalHitSound;
    public AudioClip footstepsSound;
    public AudioClip policeSirenSound;
    public AudioClip policeVoice;
    public AudioClip jamesFinalVoice;
    public float fadeToBlackDuration = 2.0f;
    public float fullBlackDelay = 5.0f;
    public float jamesFinalDelay = 2.0f;
    [Header("Story Music")]
    public DoorbellEvent doorbellEvent;

    [Header("Normal Path")]

    public AudioSource audioSource;

    public AudioClip womanVoice;


    [Header("Knife Path")]
    public KnifePickup knifePickup;

    public AudioClip jamesConfrontVoice;

    public AudioClip womanConfrontVoice;


    [Header("Fight Sequence")]
    public AudioSource sfxSource;
    public AudioClip fightSound;
    [Header("After Fight")]
    public AudioClip womanAfterFightVoice;
    public float wakeUpDelay = 1.0f;

    [Header("Player Movement Lock")]
    public FirstPersonController pcController;
    public Behaviour vrMovementScript;

        [Header("Black Screens")]
    public GameObject pcBlackScreen;
    public GameObject vrBlackScreen;
    [Header("Investigation Zones")]

    public GameObject bodyInteractZone;

    public GameObject bloodInteractZone;

    public GameObject knifeInteractZone;

    private bool normalPathPlayed = false;

    private bool knifePathPlayed = false;

    private void OnTriggerEnter(Collider other)

    {

        bool isPlayer =

            other.CompareTag("Player") ||

            other.transform.root.CompareTag("Player");

        if (!isPlayer)

            return;

        Debug.Log("Player entered WomanTrigger.");

        // =========================

        // 玩家已经拿刀

        // =========================

        if (knifePickup != null && knifePickup.HasKnife)

        {

            if (knifePathPlayed)

                return;

            knifePathPlayed = true;

            Debug.Log("Knife detected. Starting confrontation path.");

            StartCoroutine(PlayKnifePath());

            return;

        }

        // =========================

        // 玩家第一次正常上楼

        // =========================

        if (!normalPathPlayed)

        {

            normalPathPlayed = true;

            // 第一次上楼后关闭楼下调查语音

            DisableInvestigationZones();

            Debug.Log("Starting normal woman dialogue.");

            PlayNormalPath();

        }

    }

    private void DisableInvestigationZones()

    {

        if (bodyInteractZone != null)

        {

            bodyInteractZone.SetActive(false);

        }

        if (bloodInteractZone != null)

        {

            bloodInteractZone.SetActive(false);

        }

        if (knifeInteractZone != null)

        {

            knifeInteractZone.SetActive(false);

        }

        Debug.Log("Investigation zones disabled.");

    }

    private void PlayNormalPath()

    {

        if (audioSource != null && womanVoice != null)

        {

            audioSource.Stop();

            audioSource.clip = womanVoice;

            audioSource.Play();

        }

        Debug.Log("Normal woman dialogue triggered.");

    }
    private void HideBlackScreens()
    {
        if (pcBlackScreen != null)
        {
            pcBlackScreen.SetActive(false);
        }
        if (vrBlackScreen != null)
        {
            vrBlackScreen.SetActive(false);
        }
    }
    private void LockPlayerMovement()
    {
        // PC：只锁移动，不锁鼠标视角
        if (pcController != null)
        {
            pcController.MoveSpeed = 0f;
            pcController.SprintSpeed = 0f;
        }
        // VR 暂时继续关闭移动组件
        if (vrMovementScript != null)
        {
            vrMovementScript.enabled = false;
        }
        Debug.Log("Player movement locked, look remains active.");
    }
    private void ShowBlackScreens()
    {
        if (pcBlackScreen != null)
        {
            pcBlackScreen.SetActive(true);
        }
        if (vrBlackScreen != null)
        {
            vrBlackScreen.SetActive(true);
        }
    }
    private IEnumerator PlayKnifePath()
    {
        // James 对质
        if (audioSource != null && jamesConfrontVoice != null)
        {
            audioSource.Stop();
            audioSource.clip = jamesConfrontVoice;
            audioSource.Play();
            yield return new WaitWhile(() => audioSource.isPlaying);
        }
        // 女人回应
        if (audioSource != null && womanConfrontVoice != null)
        {
            audioSource.Stop();
            audioSource.clip = womanConfrontVoice;
            audioSource.Play();
            yield return new WaitWhile(() => audioSource.isPlaying);
        }
        // 停顿
        yield return new WaitForSeconds(0.5f);
        // PC 黑屏
        if (pcBlackScreen != null)
        {
            pcBlackScreen.SetActive(true);
        }
        // VR 黑屏
        if (vrBlackScreen != null)
        {
            vrBlackScreen.SetActive(true);
        }
        // 打斗声
        if (sfxSource != null && fightSound != null)
        {
            sfxSource.PlayOneShot(fightSound);
            yield return new WaitForSeconds(fightSound.length);
        }
        // 打斗结束后停止悬疑音乐
        if (doorbellEvent != null)
        {
            doorbellEvent.StopSuspenseMusic();
        }
        Debug.Log("Fight finished. Suspense music stopped.");
        // 打斗结束后继续保持黑屏一小会
        yield return new WaitForSeconds(wakeUpDelay);
        // 恢复画面
        HideBlackScreens();
        Debug.Log("Player wakes up.");
        // 恢复画面
        HideBlackScreens();
        // 玩家醒来，但已经受伤，不能移动
        LockPlayerMovement();
        Debug.Log("Player wakes up and movement is locked.");
        // 稍微停顿，让玩家先看到女人
        yield return new WaitForSeconds(1.0f);
        // 女人说战斗后的台词
        if (audioSource != null && womanAfterFightVoice != null)
        {
            audioSource.Stop();
            audioSource.clip = womanAfterFightVoice;
            audioSource.Play();
            yield return new WaitWhile(() => audioSource.isPlaying);
        }
        Debug.Log("Woman after-fight dialogue finished.");
        // 女人说完后，停顿一下
        yield return new WaitForSeconds(0.5f);
        // 最后一击声音
        if (sfxSource != null && finalHitSound != null)
        {
            sfxSource.PlayOneShot(finalHitSound);
        }
        // 模拟玩家逐渐失去意识
        yield return new WaitForSeconds(fadeToBlackDuration);
        // 完全黑屏
        ShowBlackScreens();
        Debug.Log("Player lost consciousness.");
        // 完全黑屏保持 5 秒
        yield return new WaitForSeconds(fullBlackDelay);
        // 脚步声 + 警笛声同时开始
        if (sfxSource != null)
        {
            if (footstepsSound != null)
            {
                sfxSource.PlayOneShot(footstepsSound);
            }
            if (policeSirenSound != null)
            {
                sfxSource.PlayOneShot(policeSirenSound);
            }
        }
        // 等待脚步声结束
        if (footstepsSound != null)
        {
            yield return new WaitForSeconds(footstepsSound.length);
       
    }
        // 警察声音
        if (audioSource != null && policeVoice != null)
        {
            audioSource.Stop();
            audioSource.clip = policeVoice;
            audioSource.Play();
            yield return new WaitWhile(() => audioSource.isPlaying);
        }
        // 警察说完后再等 2 秒
        yield return new WaitForSeconds(jamesFinalDelay);
        // James 最后的 Resolution 台词
        if (audioSource != null && jamesFinalVoice != null)
        {
            audioSource.Stop();
            audioSource.clip = jamesFinalVoice;
            audioSource.Play();
            yield return new WaitWhile(() => audioSource.isPlaying);
        }
        Debug.Log("Knife path ending finished.");
    }

}