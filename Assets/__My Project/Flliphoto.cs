using UnityEngine;
using System.Collections;


public class FlipPhoto : MonoBehaviour
{
    [Header("Click Settings")]
    public Camera playerCamera;
    public Transform player;
    public float interactDistance = 10f;

    [Header("Photo")]
    public Transform photoRoot;

    [Header("Audio")]
    public AudioSource voiceSource;
    public AudioClip revealVoice;

    [Header("Story")]
    public DoorbellEvent doorbellEvent;
    public GameObject blackScreen;

    public float flipDuration = 0.6f;

    private bool hasFlipped = false;

    void Update()
    {
        if (hasFlipped) return;

        if (Input.GetMouseButtonDown(0))
        {
            TryClickPhoto();
        }
    }

    void TryClickPhoto()
    {
        if (playerCamera == null)
        {
            Debug.Log("Player Camera not assigned.");
            return;
        }

        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            Debug.Log("Clicked: " + hit.transform.name);

            if (photoRoot == null)
                photoRoot = transform;

            // 点到 Photo 本体，或点到 Photo 的子物体，都触发
            if (hit.transform == photoRoot || hit.transform.IsChildOf(photoRoot))
            {
                Debug.Log("Photo flip triggered.");
                hasFlipped = true;
                StartCoroutine(FlipAndReveal());
            }
        }
    }



    IEnumerator FlipAndReveal()
    {
        if (photoRoot == null)
            photoRoot = transform;

        Quaternion startRotation = photoRoot.rotation;
        Quaternion endRotation = photoRoot.rotation * Quaternion.Euler(0f, 180f, 0f);

        float timer = 0f;

        while (timer < flipDuration)
        {
            timer += Time.deltaTime;
            photoRoot.rotation = Quaternion.Slerp(startRotation, endRotation, timer / flipDuration);
            yield return null;
        }

        photoRoot.rotation = endRotation;

        yield return new WaitForSeconds(0.5f);

        if (doorbellEvent != null)
            doorbellEvent.PlaySuspenseMusic();

        if (voiceSource != null && revealVoice != null)
        {
            voiceSource.clip = revealVoice;
            voiceSource.Play();
            yield return new WaitWhile(() => voiceSource.isPlaying);
        }

        if (doorbellEvent != null)
            doorbellEvent.StopSuspenseMusic();

        if (blackScreen != null)
            blackScreen.SetActive(true);
    }
}