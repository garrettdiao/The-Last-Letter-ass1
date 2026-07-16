using UnityEngine;
public class SuitcasePickup : MonoBehaviour
{
    public Transform player;
    public Transform holdPoint;
    public float interactDistance = 3f;
    private bool pickedUp = false;
    // 保留电脑鼠标测试
    private void OnMouseDown()
    {
        if (pickedUp || player == null)
            return;
        float distance = Vector3.Distance(
            player.position,
            transform.position
        );
        if (distance <= interactDistance)
        {
            PickUp();
        }
    }
    // 给 EZPZ / XR 调用
    public void PickUp()
    {
        if (pickedUp)
            return;
        pickedUp = true;
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        Debug.Log("Suitcase picked up.");
    }
    public bool IsPickedUp()
    {
        return pickedUp;
    }
    public void PutDown(Transform placePoint)
    {
        if (placePoint == null)
            return;
        pickedUp = false;
        transform.SetParent(null);
        transform.position = placePoint.position;
        transform.rotation = placePoint.rotation;
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }
}