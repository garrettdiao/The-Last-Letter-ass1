using UnityEngine;
public class KeyPickup : MonoBehaviour
{
    [Header("Doorbell Event")]
    public DoorbellEvent doorbellEvent;
    private bool pickedUp = false;
    public void PickUpKey()
    {
        if (pickedUp)
            return;
        if (doorbellEvent == null)
        {
            Debug.LogWarning("DoorbellEvent is not assigned.");
            return;
        }
        pickedUp = true;
        // ?? DoorbellEvent ????????
        doorbellEvent.GiveKey();
        Debug.Log("Spare key picked up.");
        // ????????
        gameObject.SetActive(false);
    }
}