using TMPro;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DJ_WordPlatform : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshPro wordText;

    [Header("State")]
    [SerializeField] private bool isCorrect = true;

    private Collider col;
    private bool used = false;

    private void Awake()
    {
        col = GetComponent<Collider>();
        if (wordText == null) wordText = GetComponentInChildren<TextMeshPro>();
    }

    public void Setup(string word, bool correct)
    {
        used = false;
        isCorrect = correct;

        if (wordText != null) wordText.text = word;

        col.enabled = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        HandleCollision(collision);
    }

    private void HandleCollision(Collision collision)
    {
        if (used) return;
        if (!collision.collider.CompareTag("Player")) return;

        // Only count as "landing" if player is coming down onto it
        Rigidbody prb = collision.collider.GetComponent<Rigidbody>();
        if (prb != null && prb.linearVelocity.y > 0f) return;

        used = true;

        DJ_PlayerController player = collision.collider.GetComponent<DJ_PlayerController>();
        if (player == null) return;

        if (isCorrect)
        {
            DJ_GameManager.I?.AddCorrect();
            player.Bounce();
        }
        else
        {
            // Break: disable collision so player falls through
            col.enabled = false;
            // Optional: play crack animation/sound here
        }
    }
}
