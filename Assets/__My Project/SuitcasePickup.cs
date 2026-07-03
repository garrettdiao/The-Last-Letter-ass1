using UnityEngine;

public class SuitcasePickup : MonoBehaviour
{
    public Transform player;
    public Transform holdPoint;
    public float interactDistance = 3f;

    private bool pickedUp = false;

    void OnMouseDown()
    {
        if (pickedUp) return;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance > interactDistance) return;

        PickUp();
    }

    void PickUp()
    {
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
    }

    public bool IsPickedUp()
    {
        return pickedUp;
    }

    public void PutDown(Transform placePoint)
    {
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

