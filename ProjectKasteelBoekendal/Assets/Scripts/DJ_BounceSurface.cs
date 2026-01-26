using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DJ_BounceSurface : MonoBehaviour
{
    public float overrideJumpVelocity = -1f; // -1 means "use player's default"

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("GROUND HIT: " + collision.collider.name);

        if (!collision.collider.CompareTag("Player")) return;

        var rb = collision.rigidbody;
        if (rb != null && rb.linearVelocity.y > 0f) return;

        var player = collision.collider.GetComponent<DJ_PlayerController>();
        if (player == null) return;

        if (overrideJumpVelocity > 0f)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, overrideJumpVelocity);
        else
            player.Bounce();
    }
}
