using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndMInigameUI : MonoBehaviour
{
    public void ExitMinigame()
    {
        SceneSwitchManager.instance.LoadPreviousScene();
    }
    public void RestartMinigame()
    {
        SceneSwitchManager.instance.ReloadScene();
    }
}
