using UnityEngine;

public class SceneMediator : MonoBehaviour
{
	public void LoadMinigameScene(string scene)
	{
		SceneSwitchManager.instance.LoadMinigameScene(scene);
	}
	public void LoadPreviousScene()
	{
		SceneSwitchManager.instance.LoadPreviousScene();
	}

	public void ReloadScene()
	{
		SceneSwitchManager.instance.ReloadScene();
	}
}
