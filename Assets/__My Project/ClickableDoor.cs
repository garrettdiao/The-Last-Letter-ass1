using UnityEngine;
public class ClickableDoor : MonoBehaviour
{
    [Header("Door")]
    public Transform doorPivot;
    [Header("Movement")]
    public float openAngle = -90f;
    public float openSpeed = 120f;
    [Header("Audio")]
    public AudioSource doorAudio;
    public AudioClip openSound;
    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;
    private void Start()
    {
        if (doorPivot == null)
        {
            Debug.LogWarning("Door Pivot is not assigned.");
            return;
        }
        closedRotation = doorPivot.rotation;
        openRotation = Quaternion.Euler(
            doorPivot.eulerAngles.x,
            doorPivot.eulerAngles.y + openAngle,
            doorPivot.eulerAngles.z
        );
    }
    private void Update()
    {
        if (doorPivot == null)
            return;
        Quaternion targetRotation = isOpen
            ? openRotation
            : closedRotation;
        doorPivot.rotation = Quaternion.RotateTowards(
            doorPivot.rotation,
            targetRotation,
            openSpeed * Time.deltaTime
        );
    }
    // 给 EZPZ Interactable 调用
    public void ToggleDoor()
    {
        if (doorPivot == null)
        {
            Debug.LogWarning("Door Pivot is not assigned.");
            return;
        }
        isOpen = !isOpen;
        if (doorAudio != null && openSound != null)
        {
            doorAudio.PlayOneShot(openSound);
        }
        Debug.Log(isOpen ? "Door opened." : "Door closed.");
    }
}