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

            audioSource.clip =

                womanVoice;

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

        // Woman:

        // "Did you open the briefcase?"

        if (audioSource != null &&

            womanPoliceReturnVoice != null)

        {

            audioSource.Stop();

            audioSource.clip =

                womanPoliceReturnVoice;

            audioSource.Play();

            // 等女人整段话说完

            yield return new WaitWhile(

                () => audioSource.isPlaying

            );

        }


        // 稍微停顿

        yield return new WaitForSeconds(0.5f);


        // 女人说完后才出现 YES / NO

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

        // 1. James 承认并摊牌

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

        // 2. Woman 回应

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

        // YES → 失败剧情

        // -------------------------------------------------

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

        //

        // "No. I didn't open it."

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

        //

        // "Thank God you came, James..."

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

        // 3. James 假装仍然相信她是 Taylor

        //    并开始套话

        //

        // "But I still don't understand...

        // who would go this far just to steal your research?

        // Do you have any idea who they might be?"

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

        //

        // "I'm not sure...

        // I only heard something about an organisation.

        // I think they called it—"

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

        // 5. 警笛突然响起

        // -------------------------------------------------

        if (sfxSource != null &&

            arrestSirenSound != null)

        {

            sfxSource.PlayOneShot(

                arrestSirenSound

            );

        }


        Debug.Log(

            "NO path reached police siren."

        );


        // 下一步从这里继续：

        // James:

        // "Sounds like the ambulance is here.

        // Come on, let's go."

    }


    // =====================================================

    // Knife Path

    // =====================================================

    private IEnumerator PlayKnifePath()

    {

        // -------------------------------------------------

        // 1. James 拿刀对质

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

        // 2. Woman 回应

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

        // Knife → 失败剧情

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

        // 4. 黑屏继续保持

        // -------------------------------------------------

        yield return new WaitForSeconds(

            wakeUpDelay

        );


        // -------------------------------------------------

        // 5. James 醒来

        // -------------------------------------------------

        HideBlackScreens();


        // -------------------------------------------------

        // 6. 锁玩家移动

        // -------------------------------------------------

        LockPlayerMovement();


        Debug.Log(

            "Player wakes up and movement is locked."

        );


        // 让玩家先看到女人

        yield return new WaitForSeconds(1.0f);


        // -------------------------------------------------

        // 7. Woman 战斗后的台词

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

        // 9. James 失去意识

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

        // 11. 黑屏保持

        // -------------------------------------------------

        yield return new WaitForSeconds(

            fullBlackDelay

        );


        // -------------------------------------------------

        // 12. 脚步声 + 警笛

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


        // 等待脚步结束

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

        // PC：锁走路，但保留鼠标视角

        if (pcController != null)

        {

            pcController.MoveSpeed = 0f;

            pcController.SprintSpeed = 0f;

        }


        // VR：关闭移动，但保留 Head Tracking

        if (vrMovementScript != null)

        {

            vrMovementScript.enabled = false;

        }


        Debug.Log(

            "Player movement locked, look remains active."

        );

    }

}
