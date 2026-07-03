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

    private bool hasTriggered = false;

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
        // Doorbell rings 3 times
        for (int i = 0; i < 3; i++)
        {
            sfxSource.PlayOneShot(doorbellSound);
            yield return new WaitForSeconds(1.2f);
        }

        // Female voice 1
        voiceSource.clip = femaleDoorbell1;
        voiceSource.Play();
        yield return new WaitWhile(() => voiceSource.isPlaying);

        // Male voice 1
        voiceSource.clip = maleDoorbell1;
        voiceSource.Play();
        yield return new WaitWhile(() => voiceSource.isPlaying);

        // Female voice 2
        voiceSource.clip = femaleDoorbell2;
        voiceSource.Play();
        yield return new WaitWhile(() => voiceSource.isPlaying);

        // Scream
        sfxSource.PlayOneShot(screamSound);
        yield return new WaitForSeconds(0.8f);

        // Glass break
        sfxSource.PlayOneShot(glassBreakSound);
        yield return new WaitForSeconds(1f);

        // Stop ambience
        if (environmentSource != null)
        {
            environmentSource.Stop();
        }

        // Start suspense music
        PlaySuspenseMusic();

        // Male voice 2
        voiceSource.clip = maleDoorbell2;
        voiceSource.Play();
    }

    // ---------- Music Functions ----------

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
        {
            musicSource.Stop();
        }
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
