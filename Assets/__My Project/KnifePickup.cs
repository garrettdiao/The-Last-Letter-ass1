using UnityEngine;
public class KnifePickup : MonoBehaviour
{
    [Header("Pickup Voice")]
    public AudioSource voiceSource;
    public AudioClip pickupVoice;
    [Header("Knife Hold Point")]
    public Transform knifeHoldPoint;
    [Header("Optional Investigation Zone")]
    public GameObject knifeInteractZone;
    private bool hasKnife = false;
    public bool HasKnife
    {
        get { return hasKnife; }
    }
    // Desktop click / EZPZ / XR 都调用这个
    public void PickUpKnife()
    {
        if (hasKnife)
            return;
        if (knifeHoldPoint == null)
        {
            Debug.LogWarning("Knife Hold Point is not assigned.");
            return;
        }
        hasKnife = true;
        // 刀变成 Left Controller 的子物体
        transform.SetParent(knifeHoldPoint);
        // 移动到握持点
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        // 拿起来以后关闭原来的调查区域
        if (knifeInteractZone != null)
        {
            knifeInteractZone.SetActive(false);
        }
        Debug.Log("Knife picked up.");
        if (voiceSource != null && pickupVoice != null)
        {
            voiceSource.Stop();
            voiceSource.clip = pickupVoice;
            voiceSource.Play();
        }
    }

}