using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndMinigameUI : MonoBehaviour
{
    public void ExitMinigame(string flag)
    {
        FindFirstObjectByType<EventFlagMediator>().enableFlag(flag);
        SceneSwitchManager.instance.LoadPreviousScene();
    }
    public void RestartMinigame()
    {
        SceneSwitchManager.instance.ReloadScene();
    }
}
