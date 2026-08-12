using UnityEngine;
public class StonePickup : MonoBehaviour
{
    [Header("Doorbell Event")]
    public DoorbellEvent doorbellEvent;
    private bool pickedUp = false;
    // 给 EZPZ Desktop / VR 调用
    public void PickUpStone()
    {
        if (pickedUp)
            return;
        if (doorbellEvent == null)
        {
            Debug.LogWarning("DoorbellEvent is not assigned.");
            return;
        }
        pickedUp = true;
        // 告诉 DoorbellEvent 玩家已经拿到石头
        doorbellEvent.GiveStone();
        Debug.Log("Stone picked up.");
        // 拿到石头以后隐藏模型
        gameObject.SetActive(false);
    }
}