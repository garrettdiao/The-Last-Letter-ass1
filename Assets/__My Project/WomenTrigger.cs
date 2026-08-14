using UnityEngine;

using System.Collections;

public class WomanTrigger : MonoBehaviour

{

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
        // 稍微停顿一下
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
        // 播放打斗声
        if (sfxSource != null && fightSound != null)
        {
            sfxSource.PlayOneShot(fightSound);
        }
        Debug.Log("Fight sequence started.");
    }
}