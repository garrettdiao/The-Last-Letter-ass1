using UnityEngine;

public class ClickableDoor : MonoBehaviour
{
    public Transform doorPivot;
    public Transform player;

    public float interactDistance = 5f;
    public float openAngle = -90f;
    public float openSpeed = 120f;

    public AudioSource doorAudio;
    public AudioClip openSound;

    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        closedRotation = doorPivot.rotation;

        openRotation = Quaternion.Euler(
            doorPivot.eulerAngles.x,
            doorPivot.eulerAngles.y + openAngle,
            doorPivot.eulerAngles.z
        );
    }

    void Update()
    {
        Quaternion targetRotation = isOpen ? openRotation : closedRotation;

        doorPivot.rotation = Quaternion.RotateTowards(
            doorPivot.rotation,
            targetRotation,
            openSpeed * Time.deltaTime
        );
    }

    void OnMouseDown()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= interactDistance)
        {
            isOpen = !isOpen;

            if (doorAudio != null && openSound != null)
            {
                doorAudio.PlayOneShot(openSound);
            }
        }
    }
}