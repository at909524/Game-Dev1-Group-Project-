
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Door Settings")]
    public Transform door;
    public float openHeight = 4f;
    public float openSpeed = 2f;
    public Collider2D doorBlocker;

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool isOpen = false;

    private void Start()
    {
        if (door == null)
        {
            Debug.LogError("Stone door has not been assigned!");
            enabled = false;
            return;
        }

        closedPosition = door.position;

        openPosition = closedPosition
                     + Vector3.up * openHeight;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") &&
            KeyInventory.HasKey)
        {
            isOpen = true;
        }
    }

    private void Update()
    {
        if (isOpen)
        {
            door.position = Vector3.MoveTowards(
                door.position,
                openPosition,
                openSpeed * Time.deltaTime
            );
        }
        if (doorBlocker != null)
        {
            doorBlocker.enabled =
                door.position.y < openPosition.y - 0.1f;
        }
    }
}
