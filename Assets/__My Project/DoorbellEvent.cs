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

    [Header("Voice Clips")]
    public AudioClip femaleDoorbell1;
    public AudioClip maleDoorbell1;
    public AudioClip femaleDoorbell2;
    public AudioClip maleDoorbell2;

    [Header("Music")]
    public AudioClip suspenseMusic;

    [Header("Interaction")]
    public Transform player;
    public float interactDistance = 5f;

    [Header("Door Auto Open")]
    public Transform doorPivot;
    public float doorOpenAngle = -90f;
    public float doorOpenSpeed = 120f;

    private bool hasTriggered = false;
    private bool openDoor = false;
    private Quaternion doorClosedRotation;
    private Quaternion doorOpenRotation;

    void Start()
    {
        if (doorPivot != null)
        {
            doorClosedRotation = doorPivot.rotation;
            doorOpenRotation = Quaternion.Euler(
                doorPivot.eulerAngles.x,
                doorPivot.eulerAngles.y + doorOpenAngle,
                doorPivot.eulerAngles.z
            );
        }
    }

    void Update()
    {
        if (openDoor && doorPivot != null)
        {
            doorPivot.rotation = Quaternion.RotateTowards(
                doorPivot.rotation,
                doorOpenRotation,
                doorOpenSpeed * Time.deltaTime
            );
        }
    }

    void OnMouseDown()
    {
        if (hasTriggered) return;

        if (player == null)
        {
            Debug.Log("Player not assigned.");
            return;
        }

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= interactDistance)
        {
            hasTriggered = true;
            StartCoroutine(PlayDoorbellSequence());
        }
        else
        {
            Debug.Log("Too far from doorbell.");
        }
    }

    IEnumerator PlayDoorbellSequence()
    {
        for (int i = 0; i < 3; i++)
        {
            sfxSource.PlayOneShot(doorbellSound);
            yield return new WaitForSeconds(1.2f);
        }

        voiceSource.clip = femaleDoorbell1;
        voiceSource.Play();
        yield return new WaitWhile(() => voiceSource.isPlaying);

        voiceSource.clip = maleDoorbell1;
        voiceSource.Play();
        yield return new WaitWhile(() => voiceSource.isPlaying);

        voiceSource.clip = femaleDoorbell2;
        voiceSource.Play();
        yield return new WaitWhile(() => voiceSource.isPlaying);

        sfxSource.PlayOneShot(screamSound);
        yield return new WaitForSeconds(0.8f);

        sfxSource.PlayOneShot(glassBreakSound);
        yield return new WaitForSeconds(1f);

        if (environmentSource != null)
            environmentSource.Stop();

        PlaySuspenseMusic();

        voiceSource.clip = maleDoorbell2;
        voiceSource.Play();
        yield return new WaitWhile(() => voiceSource.isPlaying);

        openDoor = true;
    }

    public void PlaySuspenseMusic()
    {
        if (musicSource == null || suspenseMusic == null) return;

        musicSource.Stop();
        musicSource.clip = suspenseMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopSuspenseMusic()
    {
        if (musicSource != null)
            musicSource.Stop();
    }

    public void ChangeMusic(AudioClip newMusic, bool loop = true)
    {
        if (musicSource == null || newMusic == null) return;

        musicSource.Stop();
        musicSource.clip = newMusic;
        musicSource.loop = loop;
        musicSource.Play();
    }
}