using UnityEngine;
using UnityEngine.SceneManagement;

public class DJ_GameManager : MonoBehaviour
{
    public static DJ_GameManager I { get; private set; }

    [Header("Progress")]
    [SerializeField] public int scoreCorrect = 0;

    [Header("Difficulty (tiers)")]
    [SerializeField] public int minTier = 1;
    [SerializeField] public int maxTier = 1;

    [Header("Game Over")]
    [SerializeField] public bool isGameOver = false;

    private void Awake()
    {
        if (I != null && I != this) { Destroy(gameObject); return; }
        I = this;
    }

    public void AddCorrect()
    {
        scoreCorrect++;

        // Simple difficulty ramp: every 10 correct, expand tier range
        if (scoreCorrect == 10) maxTier = 2;
        if (scoreCorrect == 25) maxTier = 3;
    }

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        // Jam-simple restart: reload scene after short delay
        Invoke(nameof(Restart), 1.0f);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
