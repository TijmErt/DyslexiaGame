using UnityEngine;
using UnityEngine.SceneManagement;

public class FlappyRhymesGameManager : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private FlappyRhymesWordManager wordManager;
    [SerializeField] private FlappyRhymeScoreManager scoreManager;
    [SerializeField] private FlappyRhymesHealthManager healthManager;
    [SerializeField] private GameObject gameOverScreen;

    [ContextMenu("Reset Game")]
    public void ResetGame()
    {
        // Reset game time as well
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    [ContextMenu("Exit Game")]
    public void ExitGame()
    {
        if (string.IsNullOrEmpty(sceneName)) return;
        SceneManager.LoadScene(sceneName);
    }
}
