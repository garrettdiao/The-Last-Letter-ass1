using StarterAssets;
using System.Collections;
using UnityEngine;
public class WomanTrigger : MonoBehaviour
{
    // =====================================================
    // Police Scene Objects
    // =====================================================
    [Header("Police Scene Objects")]
    public GameObject policeCar;
    public GameObject policeOfficer;

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

    // =====================================================
    // YES Choice
    // =====================================================
    [Header("Briefcase YES Choice")]
    public AudioClip jamesYesVoice;
    public AudioClip womanYesVoice;

    // =====================================================
    // NO Choice
    // =====================================================
    [Header("Briefcase NO Choice")]
    public AudioClip jamesNoVoice;
    public AudioClip womanReliefVoice;
    public AudioClip jamesQuestionVoice;
    public AudioClip womanOrganisationVoice;
    public AudioClip arrestSirenSound;
    public AudioClip jamesAmbulanceVoice;

    // =====================================================
    // Police Scene Transition
    // =====================================================
    [Header("Police Scene Transition")]
    public Transform playerTransform;
    public Transform womanTransform;
    public Transform policeScenePlayerPoint;
    public Transform policeSceneWomanPoint;
    public float transitionBlackTime = 1.0f;
    public AudioClip policeArrestVoice;
    public float policeArrestDelay = 0.5f;
    public AudioClip jamesRevealVoice;
    public AudioClip womanFinalArrestVoice;
    public AudioClip jamesSnowDukeVoice;
    public float pauseBeforeJamesReveal = 0.5f;
    public float pauseBeforeFinalBlack = 0.8f;
    public float pauseBeforeSnowDukeVoice = 1.5f;

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
        StartCoroutine(
            PlayBriefcaseNoPath()
        );
    }

    // =====================================================
    // Disable Investigation Zones
    // =====================================================
    private void DisableInvestigationZones()
    {
        if (bodyInteractZone != null)
            bodyInteractZone.SetActive(false);
        if (bloodInteractZone != null)
            bloodInteractZone.SetActive(false);
        if (knifeInteractZone != null)
            knifeInteractZone.SetActive(false);
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
        if (audioSource != null &&
            womanPoliceReturnVoice != null)
        {
            audioSource.Stop();
            audioSource.clip =
                womanPoliceReturnVoice;
            audioSource.Play();
            // 等女人问完再显示选择
            yield return new WaitWhile(
                () => audioSource.isPlaying
            );
        }
        yield return new WaitForSeconds(0.5f);
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
        // James 摊牌
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

        // Woman 回应
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

        // YES → 进入失败剧情
        yield return StartCoroutine(
            DefeatSequence()
        );
    }

    // =====================================================
    // NO Choice Path
    // =====================================================
    private IEnumerator PlayBriefcaseNoPath()
    {
        Debug.Log(
            "NO deception path started."
        );

        // -------------------------------------------------
        // 1. James 否认打开公文包
        // -------------------------------------------------
        if (audioSource != null &&
            jamesNoVoice != null)
        {
            audioSource.Stop();
            audioSource.clip =
                jamesNoVoice;
            audioSource.Play();
            yield return new WaitWhile(
                () => audioSource.isPlaying
            );
        }
        yield return new WaitForSeconds(0.4f);

        // -------------------------------------------------
        // 2. Woman 放松警惕
        // -------------------------------------------------
        if (audioSource != null &&
            womanReliefVoice != null)
        {
            audioSource.Stop();
            audioSource.clip =
                womanReliefVoice;
            audioSource.Play();
            yield return new WaitWhile(
                () => audioSource.isPlaying
            );
        }
        yield return new WaitForSeconds(0.5f);

        // -------------------------------------------------
        // 3. James 套话
        // -------------------------------------------------
        if (audioSource != null &&
            jamesQuestionVoice != null)
        {
            audioSource.Stop();
            audioSource.clip =
                jamesQuestionVoice;
            audioSource.Play();
            yield return new WaitWhile(
                () => audioSource.isPlaying
            );
        }
        yield return new WaitForSeconds(0.4f);

        // -------------------------------------------------
        // 4. Woman 差点透露组织
        // -------------------------------------------------
        if (audioSource != null &&
            womanOrganisationVoice != null)
        {
            audioSource.Stop();
            audioSource.clip =
                womanOrganisationVoice;
            audioSource.Play();
            yield return new WaitWhile(
                () => audioSource.isPlaying
            );
        }

        // -------------------------------------------------
        // 5. 警笛响起
        // -------------------------------------------------
        if (sfxSource != null &&
            arrestSirenSound != null)
        {
            sfxSource.PlayOneShot(
                arrestSirenSound
            );
        }
        yield return new WaitForSeconds(1.0f);

        // -------------------------------------------------
        // 6. James 假装是救护车
        // -------------------------------------------------
        if (audioSource != null &&
            jamesAmbulanceVoice != null)
        {
            audioSource.Stop();
            audioSource.clip =
                jamesAmbulanceVoice;
            audioSource.Play();
            yield return new WaitWhile(
                () => audioSource.isPlaying
            );
        }
        Debug.Log(
            "James convinced the woman to go outside."
        );

        // -------------------------------------------------
        // 7. 进入屋外警察场景
        // -------------------------------------------------
        yield return StartCoroutine(
            TransitionToPoliceScene()
        );
    }

    // =====================================================
    // Police Scene Transition
    // =====================================================
    private IEnumerator TransitionToPoliceScene()
    {
        // James说完“救护车到了”以后稍微停一下
        yield return new WaitForSeconds(0.5f);

        // =====================================================
        // 1. 黑屏
        // =====================================================
        ShowBlackScreens();
        yield return new WaitForSeconds(
            transitionBlackTime
        );

        // =====================================================
        // 2. 黑屏期间显示 Police Scene
        // =====================================================
        if (policeCar != null)
        {
            policeCar.SetActive(true);
        }
        if (policeOfficer != null)
        {
            policeOfficer.SetActive(true);
        }

        // =====================================================
        // 3. 移动 Player
        // =====================================================
        if (playerTransform != null &&
            policeScenePlayerPoint != null)
        {
            CharacterController controller =
                playerTransform.GetComponent<CharacterController>();
            if (controller != null)
            {
                controller.enabled = false;
            }
            playerTransform.position =
                policeScenePlayerPoint.position;
            playerTransform.rotation =
                policeScenePlayerPoint.rotation;
            if (controller != null)
            {
                controller.enabled = true;
            }
        }

        // =====================================================
        // 4. 移动 Woman
        // =====================================================
        if (womanTransform != null &&
            policeSceneWomanPoint != null)
        {
            womanTransform.position =
                policeSceneWomanPoint.position;
            womanTransform.rotation =
                policeSceneWomanPoint.rotation;
        }

        yield return null;
        yield return new WaitForSeconds(0.5f);

        // =====================================================
        // 5. 恢复画面
        // =====================================================
        HideBlackScreens();

        // =====================================================
        // 6. Police arrest command
        // =====================================================
        yield return new WaitForSeconds(
            policeArrestDelay
        );
        if (audioSource != null &&
            policeArrestVoice != null)
        {
            audioSource.Stop();
            audioSource.clip =
                policeArrestVoice;
            audioSource.Play();
            yield return new WaitWhile(
                () => audioSource.isPlaying
            );
        }

        // =====================================================
        // 7. James 揭露骗局
        // =====================================================
        yield return new WaitForSeconds(
            pauseBeforeJamesReveal
        );
        if (audioSource != null &&
            jamesRevealVoice != null)
        {
            audioSource.Stop();
            audioSource.clip =
                jamesRevealVoice;
            audioSource.Play();
            yield return new WaitWhile(
                () => audioSource.isPlaying
            );
        }

        yield return new WaitForSeconds(0.5f);

        // =====================================================
        // 8. Woman 最终回应
        // =====================================================
        if (audioSource != null &&
            womanFinalArrestVoice != null)
        {
            audioSource.Stop();
            audioSource.clip =
                womanFinalArrestVoice;
            audioSource.Play();
            yield return new WaitWhile(
                () => audioSource.isPlaying
            );
        }

        // =====================================================
        // 9. 最终黑屏
        // =====================================================
        yield return new WaitForSeconds(
            pauseBeforeFinalBlack
        );
        ShowBlackScreens();

        // =====================================================
        // 10. 黑屏后 James 对 Snow Duke 的旁白
        // =====================================================
        yield return new WaitForSeconds(
            pauseBeforeSnowDukeVoice
        );
        if (audioSource != null &&
            jamesSnowDukeVoice != null)
        {
            audioSource.Stop();
            audioSource.clip =
                jamesSnowDukeVoice;
            audioSource.Play();
            yield return new WaitWhile(
                () => audioSource.isPlaying
            );
        }

        Debug.Log(
            "NO route arrest ending finished."
        );
    }
    // =====================================================
    // Knife Path
    // =====================================================
    private IEnumerator PlayKnifePath()
    {
        // James 对质
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

        // Woman 回应
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

        // Knife → 共用失败剧情
        yield return StartCoroutine(
            DefeatSequence()
        );
    }

    // =====================================================
    // Shared Defeat Sequence
    // Knife Path + YES Choice
    // =====================================================
    private IEnumerator DefeatSequence()
    {
        Debug.Log(
            "Defeat sequence started."
        );

        // -------------------------------------------------
        // 黑屏
        // -------------------------------------------------
        ShowBlackScreens();

        // -------------------------------------------------
        // 打斗声
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
        // 停止悬疑音乐
        // -------------------------------------------------
        if (doorbellEvent != null)
        {
            doorbellEvent.StopSuspenseMusic();
        }
        Debug.Log(
            "Fight finished. Suspense music stopped."
        );

        // -------------------------------------------------
        // 黑屏保持
        // -------------------------------------------------
        yield return new WaitForSeconds(
            wakeUpDelay
        );

        // -------------------------------------------------
        // 恢复画面 + 锁移动
        // -------------------------------------------------
        HideBlackScreens();
        LockPlayerMovement();
        yield return new WaitForSeconds(1.0f);

        // -------------------------------------------------
        // Woman 战斗后台词
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

        // -------------------------------------------------
        // 最后一击
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
        // 再次失去意识
        // -------------------------------------------------
        yield return new WaitForSeconds(
            fadeToBlackDuration
        );
        ShowBlackScreens();

        // -------------------------------------------------
        // 黑屏保持
        // -------------------------------------------------
        yield return new WaitForSeconds(
            fullBlackDelay
        );

        // -------------------------------------------------
        // 脚步 + 警笛
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

        if (footstepsSound != null)
        {
            yield return new WaitForSeconds(
                footstepsSound.length
            );
        }

        // -------------------------------------------------
        // Police Voice
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
        // James Resolution Voice
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
        // PC：锁走路，保留鼠标视角
        if (pcController != null)
        {
            pcController.MoveSpeed = 0f;
            pcController.SprintSpeed = 0f;
        }
        // VR：关闭移动但保留头部追踪
        if (vrMovementScript != null)
        {
            vrMovementScript.enabled = false;
        }
        Debug.Log(
            "Player movement locked, look remains active."
        );
    }
}