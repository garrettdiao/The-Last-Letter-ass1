using StarterAssets;
using System.Collections;
using UnityEngine;
public class WomanTrigger : MonoBehaviour
{
    // =====================================================
    // Story Music
    // =====================================================
    [Header("Story Music")]
    public DoorbellEvent doorbellEvent;

    // =====================================================
    // Normal Path
    // =====================================================
    [Header("Normal Path")]
    public AudioSource audioSource;
    public AudioClip womanVoice;

    // =====================================================
    // Knife Path
    // =====================================================
    [Header("Knife Path")]
    public KnifePickup knifePickup;
    public AudioClip jamesConfrontVoice;
    public AudioClip womanConfrontVoice;

    // =====================================================
    // Police Return Path
    // =====================================================
    [Header("Police Return Path")]
    public PhoneCall phoneCall;
    public AudioClip womanPoliceReturnVoice;

    // =====================================================
    // Briefcase Choice
    // =====================================================
    [Header("Briefcase Choice")]
    public GameObject briefcaseChoice;
    [Header("Briefcase YES Choice")]
    public AudioClip jamesYesVoice;
    public AudioClip womanYesVoice;

    // =====================================================
    // Fight Sequence
    // =====================================================
    [Header("Fight Sequence")]
    public AudioSource sfxSource;
    public AudioClip fightSound;

    // =====================================================
    // After Fight
    // =====================================================
    [Header("After Fight")]
    public AudioClip womanAfterFightVoice;
    public float wakeUpDelay = 1.0f;

    // =====================================================
    // Ending Sequence
    // =====================================================
    [Header("Ending Sequence")]
    public AudioClip finalHitSound;
    public AudioClip footstepsSound;
    public AudioClip policeSirenSound;
    public AudioClip policeVoice;
    public AudioClip jamesFinalVoice;
    public float fadeToBlackDuration = 2.0f;
    public float fullBlackDelay = 5.0f;
    public float jamesFinalDelay = 2.0f;

    // =====================================================
    // Player Movement Lock
    // =====================================================
    [Header("Player Movement Lock")]
    public FirstPersonController pcController;
    public Behaviour vrMovementScript;

    // =====================================================
    // Black Screens
    // =====================================================
    [Header("Black Screens")]
    public GameObject pcBlackScreen;
    public GameObject vrBlackScreen;

    // =====================================================
    // Investigation Zones
    // =====================================================
    [Header("Investigation Zones")]
    public GameObject bodyInteractZone;
    public GameObject bloodInteractZone;
    public GameObject knifeInteractZone;

    // =====================================================
    // Story States
    // =====================================================
    private bool normalPathPlayed = false;
    private bool knifePathPlayed = false;
    private bool policeReturnPathPlayed = false;
    private bool briefcaseChoiceResolved = false;

    // =====================================================
    // Main Trigger
    // =====================================================
    private void OnTriggerEnter(Collider other)
    {
        bool isPlayer =
            other.CompareTag("Player") ||
            other.transform.root.CompareTag("Player");
        if (!isPlayer)
            return;
        Debug.Log("Player entered WomanTrigger.");

        // -------------------------------------------------
        // 1. Knife Path
        // -------------------------------------------------
        if (knifePickup != null && knifePickup.HasKnife)
        {
            if (knifePathPlayed)
                return;
            knifePathPlayed = true;
            Debug.Log(
                "Knife detected. Starting confrontation path."
            );
            StartCoroutine(PlayKnifePath());
            return;
        }

        // -------------------------------------------------
        // 2. Police Return Path
        // -------------------------------------------------
        if (phoneCall != null && phoneCall.HasCalledPolice)
        {
            if (policeReturnPathPlayed)
                return;
            policeReturnPathPlayed = true;
            Debug.Log(
                "Police already called. Starting police return path."
            );
            StartCoroutine(PlayPoliceReturnPath());
            return;
        }

        // -------------------------------------------------
        // 3. Normal Path
        // -------------------------------------------------
        if (!normalPathPlayed)
        {
            normalPathPlayed = true;
            DisableInvestigationZones();
            Debug.Log(
                "Starting normal woman dialogue."
            );
            PlayNormalPath();
        }
    }

    // =====================================================
    // Briefcase Choice Public Functions
    // =====================================================
    public void TriggerBriefcaseYesChoice()
    {
        if (briefcaseChoiceResolved)
            return;
        briefcaseChoiceResolved = true;
        Debug.Log(
            "Starting YES briefcase choice."
        );
        StartCoroutine(
            PlayBriefcaseYesPath()
        );
    }

    public void TriggerBriefcaseNoChoice()
    {
        if (briefcaseChoiceResolved)
            return;
        briefcaseChoiceResolved = true;
        Debug.Log(
            "Starting NO briefcase choice."
        );
        // 下一步我们继续做 NO 路线
    }

    // =====================================================
    // Disable Investigation Zones
    // =====================================================
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
        Debug.Log(
            "Investigation zones disabled."
        );
    }

    // =====================================================
    // Normal Path
    // =====================================================
    private void PlayNormalPath()
    {
        if (audioSource != null &&
            womanVoice != null)
        {
            audioSource.Stop();
            audioSource.clip = womanVoice;
            audioSource.Play();
        }
        Debug.Log(
            "Normal woman dialogue triggered."
        );
    }

    // =====================================================
    // Police Return Path
    // =====================================================
    private IEnumerator PlayPoliceReturnPath()
    {
        // 女人询问 James：
        // “Did you open the briefcase?”
        if (audioSource != null &&
            womanPoliceReturnVoice != null)
        {
            audioSource.Stop();
            audioSource.clip =
                womanPoliceReturnVoice;
            audioSource.Play();
            // 等女人整段说完
            yield return new WaitWhile(
                () => audioSource.isPlaying
            );
        }
        // 稍微停顿
        yield return new WaitForSeconds(0.5f);
        // 女人说完以后才显示 YES / NO
        if (briefcaseChoice != null)
        {
            briefcaseChoice.SetActive(true);
        }
        Debug.Log(
            "Woman finished asking. Briefcase choices shown."
        );
    }

    // =====================================================
    // YES Choice Path
    // =====================================================
    private IEnumerator PlayBriefcaseYesPath()
    {
        // -------------------------------------------------
        // James 承认打开过公文包
        // -------------------------------------------------
        if (audioSource != null &&
            jamesYesVoice != null)
        {
            audioSource.Stop();
            audioSource.clip =
                jamesYesVoice;
            audioSource.Play();
            yield return new WaitWhile(
                () => audioSource.isPlaying
            );
        }
        yield return new WaitForSeconds(0.4f);

        // -------------------------------------------------
        // Woman 回应
        // -------------------------------------------------
        if (audioSource != null &&
            womanYesVoice != null)
        {
            audioSource.Stop();
            audioSource.clip =
                womanYesVoice;
            audioSource.Play();
            yield return new WaitWhile(
                () => audioSource.isPlaying
            );
        }
        yield return new WaitForSeconds(0.5f);
        Debug.Log(
            "YES choice leads to defeat sequence."
        );

        // -------------------------------------------------
        // YES 和 Knife Path 在这里汇合
        // -------------------------------------------------
        yield return StartCoroutine(
            DefeatSequence()
        );
    }

    // =====================================================
    // Knife Path
    // =====================================================
    private IEnumerator PlayKnifePath()
    {
        // -------------------------------------------------
        // James 对质
        // -------------------------------------------------
        if (audioSource != null &&
            jamesConfrontVoice != null)
        {
            audioSource.Stop();
            audioSource.clip =
                jamesConfrontVoice;
            audioSource.Play();
            yield return new WaitWhile(
                () => audioSource.isPlaying
            );
        }

        // -------------------------------------------------
        // Woman 回应
        // -------------------------------------------------
        if (audioSource != null &&
            womanConfrontVoice != null)
        {
            audioSource.Stop();
            audioSource.clip =
                womanConfrontVoice;
            audioSource.Play();
            yield return new WaitWhile(
                () => audioSource.isPlaying
            );
        }
        yield return new WaitForSeconds(0.5f);

        // -------------------------------------------------
        // Knife 和 YES 在这里汇合
        // -------------------------------------------------
        yield return StartCoroutine(
            DefeatSequence()
        );
    }

    // =====================================================
    // Shared Defeat Sequence
    // Knife Path + YES Choice 共用
    // =====================================================
    private IEnumerator DefeatSequence()
    {
        Debug.Log(
            "Defeat sequence started."
        );

        // -------------------------------------------------
        // 1. 黑屏
        // -------------------------------------------------
        ShowBlackScreens();

        // -------------------------------------------------
        // 2. 打斗声
        // -------------------------------------------------
        if (sfxSource != null &&
            fightSound != null)
        {
            sfxSource.PlayOneShot(
                fightSound
            );
            yield return new WaitForSeconds(
                fightSound.length
            );
        }

        // -------------------------------------------------
        // 3. 停止悬疑音乐
        // -------------------------------------------------
        if (doorbellEvent != null)
        {
            doorbellEvent.StopSuspenseMusic();
        }
        Debug.Log(
            "Fight finished. Suspense music stopped."
        );

        // -------------------------------------------------
        // 4. 继续黑屏一会
        // -------------------------------------------------
        yield return new WaitForSeconds(
            wakeUpDelay
        );

        // -------------------------------------------------
        // 5. 恢复画面
        // -------------------------------------------------
        HideBlackScreens();

        // -------------------------------------------------
        // 6. 锁玩家移动
        // -------------------------------------------------
        LockPlayerMovement();
        Debug.Log(
            "Player wakes up and movement is locked."
        );

        // 先让玩家看到女人
        yield return new WaitForSeconds(1.0f);

        // -------------------------------------------------
        // 7. Woman 战斗后台词
        // -------------------------------------------------
        if (audioSource != null &&
            womanAfterFightVoice != null)
        {
            audioSource.Stop();
            audioSource.clip =
                womanAfterFightVoice;
            audioSource.Play();
            yield return new WaitWhile(
                () => audioSource.isPlaying
            );
        }
        Debug.Log(
            "Woman after-fight dialogue finished."
        );

        // -------------------------------------------------
        // 8. 最后一击
        // -------------------------------------------------
        yield return new WaitForSeconds(0.5f);
        if (sfxSource != null &&
            finalHitSound != null)
        {
            sfxSource.PlayOneShot(
                finalHitSound
            );
        }

        // -------------------------------------------------
        // 9. 等待“失去意识”
        // -------------------------------------------------
        yield return new WaitForSeconds(
            fadeToBlackDuration
        );

        // -------------------------------------------------
        // 10. 再次完全黑屏
        // -------------------------------------------------
        ShowBlackScreens();
        Debug.Log(
            "Player lost consciousness."
        );

        // -------------------------------------------------
        // 11. 完全黑屏保持
        // -------------------------------------------------
        yield return new WaitForSeconds(
            fullBlackDelay
        );

        // -------------------------------------------------
        // 12. 脚步声 + 警笛同时播放
        // -------------------------------------------------
        if (sfxSource != null)
        {
            if (footstepsSound != null)
            {
                sfxSource.PlayOneShot(
                    footstepsSound
                );
            }
            if (policeSirenSound != null)
            {
                sfxSource.PlayOneShot(
                    policeSirenSound
                );
            }
        }

        // 等待脚步声结束
        if (footstepsSound != null)
        {
            yield return new WaitForSeconds(
                footstepsSound.length
            );
        }

        // -------------------------------------------------
        // 13. Police Voice
        // -------------------------------------------------
        if (audioSource != null &&
            policeVoice != null)
        {
            audioSource.Stop();
            audioSource.clip =
                policeVoice;
            audioSource.Play();
            yield return new WaitWhile(
                () => audioSource.isPlaying
            );
        }

        // -------------------------------------------------
        // 14. James Resolution Voice
        // -------------------------------------------------
        yield return new WaitForSeconds(
            jamesFinalDelay
        );
        if (audioSource != null &&
            jamesFinalVoice != null)
        {
            audioSource.Stop();
            audioSource.clip =
                jamesFinalVoice;
            audioSource.Play();
            yield return new WaitWhile(
                () => audioSource.isPlaying
            );
        }

        Debug.Log(
            "Defeat sequence finished."
        );
    }

    // =====================================================
    // Black Screen Helpers
    // =====================================================
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

    // =====================================================
    // Movement Lock
    // =====================================================
    private void LockPlayerMovement()
    {
        // PC：只锁走路，保留鼠标视角
        if (pcController != null)
        {
            pcController.MoveSpeed = 0f;
            pcController.SprintSpeed = 0f;
        }
        // VR：关闭移动脚本，但保留 Head Tracking
        if (vrMovementScript != null)
        {
            vrMovementScript.enabled = false;
        }
        Debug.Log(
            "Player movement locked, look remains active."
        );
    }
}