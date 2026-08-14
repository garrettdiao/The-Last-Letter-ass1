using UnityEngine;
using System.Collections;
public class PhoneCall : MonoBehaviour
{
    [Header("Phone Object")]
    public Transform phoneObject;
    [Header("Story References")]
    public IDCardInvestigation idCardInvestigation;
    public FlipPhoto flipPhoto;
    [Header("Hold Points")]
    public Transform pcHoldPoint;
    public Transform vrHoldPoint;
    [Header("Player Mode")]
    public bool useVR = false;
    [Header("Phone Hold Rotation")]
    public Vector3 holdRotationOffset = new Vector3(90f, 90f, 0f);
    [Header("Audio Sources")]
    public AudioSource voiceSource;
    public AudioSource sfxSource;
    [Header("Police Call Audio")]
    public AudioClip callSound;
    public AudioClip policeAnswerVoice;
    public AudioClip jamesReportVoice;
    public AudioClip policeResponseVoice;
    public AudioClip jamesFinalVoice;
    [Header("Timing")]
    public float pauseBetweenVoices = 0.3f;
    public float pauseBeforeFinalVoice = 0.7f;
    [Header("Interaction Zone")]
    public GameObject phoneInteractZone;
    private bool hasPickedUp = false;
    private bool isCalling = false;
    // 记录电话原来的位置
    private Transform originalParent;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    public bool HasCalledPolice { get; private set; } = false;
    private void Start()
    {
        // 游戏开始时记住电话原来的位置和旋转
        if (phoneObject != null)
        {
            originalParent = phoneObject.parent;
            originalPosition = phoneObject.position;
            originalRotation = phoneObject.rotation;
        }
    }
    // 给 EZPZ Desktop / VR 调用
    public void TryUsePhone()
    {
        // 已经报警或者正在通话时不能重复触发
        if (HasCalledPolice || isCalling)
            return;
        // 玩家是否已经知道真相
        bool knowsTruth =
            (idCardInvestigation != null &&
             idCardInvestigation.HasRevealed)
            ||
            (flipPhoto != null &&
             flipPhoto.HasRevealedTruth);
        if (!knowsTruth)
        {
            Debug.Log("Player does not know the truth yet.");
            return;
        }
        isCalling = true;
        StartCoroutine(PoliceCallSequence());
    }
    private IEnumerator PoliceCallSequence()
    {
        // =========================
        // 1. 拿起电话
        // =========================
        if (!hasPickedUp)
        {
            hasPickedUp = true;
            Transform targetPoint =
                useVR ? vrHoldPoint : pcHoldPoint;
            if (targetPoint != null && phoneObject != null)
            {
                phoneObject.SetParent(targetPoint);
                phoneObject.localPosition = Vector3.zero;
                // X +90, 
                phoneObject.localRotation =
                    Quaternion.Euler(holdRotationOffset);
                Debug.Log("Phone picked up.");
            }
            else
            {
                Debug.LogWarning(
                    "Phone Object or Hold Point is not assigned."
                );
            }
        }
        // =========================
        // 2. 电话接通声
        // =========================
        if (sfxSource != null && callSound != null)
        {
            sfxSource.PlayOneShot(callSound);
            yield return new WaitForSeconds(
                callSound.length
            );
        }
        // =========================
        // 3. 警察接电话
        // =========================
        if (voiceSource != null &&
            policeAnswerVoice != null)
        {
            voiceSource.Stop();
            voiceSource.clip =
                policeAnswerVoice;
            voiceSource.Play();
            yield return new WaitWhile(
                () => voiceSource.isPlaying
            );
        }
        yield return new WaitForSeconds(
            pauseBetweenVoices
        );
        // =========================
        // 4. James说明情况
        // =========================
        if (voiceSource != null &&
            jamesReportVoice != null)
        {
            voiceSource.Stop();
            voiceSource.clip =
                jamesReportVoice;
            voiceSource.Play();
            yield return new WaitWhile(
                () => voiceSource.isPlaying
            );
        }
        yield return new WaitForSeconds(
            pauseBetweenVoices
        );
        // =========================
        // 5. 警察回应
        // =========================
        if (voiceSource != null &&
            policeResponseVoice != null)
        {
            voiceSource.Stop();
            voiceSource.clip =
                policeResponseVoice;
            voiceSource.Play();
            yield return new WaitWhile(
                () => voiceSource.isPlaying
            );
        }
        // =========================
        // 6. James最后自白
        // =========================
        yield return new WaitForSeconds(
            pauseBeforeFinalVoice
        );
        if (voiceSource != null &&
            jamesFinalVoice != null)
        {
            voiceSource.Stop();
            voiceSource.clip =
                jamesFinalVoice;
            voiceSource.Play();
            yield return new WaitWhile(
                () => voiceSource.isPlaying
            );
        }
        // =========================
        // 7. 报警完成
        // =========================
        HasCalledPolice = true;
        Debug.Log("Police have been called.");
        // 稍微等一下再把电话放回去
        yield return new WaitForSeconds(0.5f);
        ReturnPhoneToTable();
        isCalling = false;
        // 整个 Coroutine 最后才关闭 Zone
        if (phoneInteractZone != null)
        {
            phoneInteractZone.SetActive(false);
        }
        Debug.Log(
            "Police call finished. Player should return upstairs."
        );
    }
    private void ReturnPhoneToTable()
    {
        if (phoneObject == null)
            return;
        // 回到原来的 Parent
        phoneObject.SetParent(originalParent);
        // 恢复游戏开始时的位置
        phoneObject.position = originalPosition;
        // 恢复游戏开始时的角度
        phoneObject.rotation = originalRotation;
        hasPickedUp = false;
        Debug.Log("Phone returned to the table.");
    }
}