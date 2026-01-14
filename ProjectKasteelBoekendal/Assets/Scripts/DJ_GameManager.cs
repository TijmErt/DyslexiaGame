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

    [Header("Win Condition")]
    [SerializeField] private int targetScoreToWin = 20;

    [Header("Scenes")]
    [SerializeField] private string overworldSceneName = "Overworld";

    [Header("State")]
    public bool isGameOver = false;
    public bool isWin = false;

    [SerializeField] private GameObject minigameEndMenu;
    [SerializeField] private Rigidbody rb;

    private void Awake()
    {
        if (I != null && I != this) { Destroy(gameObject); return; }
        I = this;
    }

    public void AddCorrect()
    {
        if (isGameOver || isWin) return;

        scoreCorrect++;

        if (scoreCorrect >= targetScoreToWin)
        {
            Win();
        }

        //if (scoreCorrect == 10) maxTier = 2;
        //if (scoreCorrect == 25) maxTier = 3;
    }

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        Invoke(nameof(Restart), 1.0f);
    }

    private void Win()
    {
        if (isWin || isGameOver) return;
        isWin = true;

        minigameEndMenu.SetActive(true);
        rb.isKinematic = true;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
