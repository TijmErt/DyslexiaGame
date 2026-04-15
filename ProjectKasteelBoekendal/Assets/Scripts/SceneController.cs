using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] private string sceneName = "";

    [SerializeField] private Collectible item;

    void OnTriggerEnter(Collider other)
    {
        if (string.IsNullOrEmpty(sceneName)) return;
        SceneManager.LoadScene(sceneName);
    }

    public void BackToMainRoom()
    {
        SceneManager.LoadScene("Combined");
    }

    public void RemoveBeforeMerge()
    {
        if (string.IsNullOrEmpty(sceneName)) return;
        SceneManager.LoadScene(sceneName);
    }

    public void LoadRoomScene(string roomSceneName)
    {
        // CollectibleStateHolder.RuntimeOf(item).hasBeenFound = true;

        if (string.IsNullOrEmpty(roomSceneName)) return;
        Time.timeScale = 1f;
        SceneManager.LoadScene(roomSceneName);
    }

    public void LoadMinigameScene(string minigameSceneName)
    {
        if (string.IsNullOrEmpty(minigameSceneName)) return;
        Time.timeScale = 1f;
        SceneManager.LoadScene(minigameSceneName);
    }

}
