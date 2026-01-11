using UnityEngine;

public class DJ_OneWayPlatform : MonoBehaviour
{
    [SerializeField] private Collider solidPlatformCollider;
    [SerializeField] private float topBuffer = 0.05f;

    private void Reset()
    {
        // If placed on child trigger, try to auto-find parent collider
        if (solidPlatformCollider == null)
            solidPlatformCollider = GetComponentInParent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Start ignored so player can pass upward through it.
        Physics.IgnoreCollision(other, solidPlatformCollider, true);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!GetComponentInParent<DJ_WordPlatform>().isCorrect) return;

        if (!other.CompareTag("Player")) return;

        Rigidbody playerRb = other.attachedRigidbody;
        if (playerRb == null) return;

        float platformTop = solidPlatformCollider.bounds.max.y;
        float playerBottom = other.bounds.min.y;

        bool playerAbove = playerBottom >= platformTop - topBuffer;
        bool playerFalling = playerRb.linearVelocity.y <= 0f;

        bool shouldCollide = playerAbove && playerFalling;

        Physics.IgnoreCollision(other, solidPlatformCollider, !shouldCollide);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Reset to allow passing upward again next time
        Physics.IgnoreCollision(other, solidPlatformCollider, false);
    }
}
