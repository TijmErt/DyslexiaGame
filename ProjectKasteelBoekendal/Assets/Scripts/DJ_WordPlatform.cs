using TMPro;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DJ_WordPlatform : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshPro wordText;

    [Header("State")]
    [SerializeField] public bool isCorrect = true;

    private Collider col;
    private bool scored = false;

    private void Awake()
    {
        col = GetComponent<Collider>();
        if (wordText == null) wordText = GetComponentInChildren<TextMeshPro>();
    }

    public void Setup(string word, bool correct)
    {
        isCorrect = correct;
        scored = false;

        if (wordText != null) wordText.text = word;        

        if (correct)
        {
            col.enabled = true;
        }
        else
        {
            col.enabled = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("GROUND HIT: " + collision.collider.name);

        HandleCollision(collision);
    }

    private void HandleCollision(Collision collision)
    {
        if (!collision.collider.CompareTag("Player")) return;

        Rigidbody prb = collision.rigidbody; // player's rigidbody
        if (prb != null && prb.linearVelocity.y > 0f) return; // only when falling/downward

        var player = collision.collider.GetComponent<DJ_PlayerController>();
        if (player == null) return;

        if (isCorrect)
        {
            if (!scored)
            {
                scored = true;
                DJ_GameManager.I?.AddCorrect(); // score only once
            }

            player.Bounce(); // ALWAYS bounce on correct
        }
        else
        {
            // Wrong: break once and fall through
        }
    }
}
