using UnityEngine;

/// <summary>
/// The SceneMediator acts as a middle layer between other scripts/UI systems
/// and the SceneSwitchManager.
///
/// This helps reduce direct dependency on the SceneSwitchManager singleton
/// instance while providing simplified access to scene loading functionality.
///
/// The mediator also exposes scene management methods in a format that can
/// easily be used by Unity Events and UI systems through the Inspector.
/// </summary>
public class SceneMediator : MonoBehaviour
{
	#region Scene Loading Methods

	/*

	 Wrapper methods for interacting with the SceneSwitchManager.

	 These methods provide centralized access to scene loading
	 functionality while avoiding direct singleton usage throughout
	 gameplay systems and UI elements.

	 Primarily intended for scene transitions, menu buttons,
	 minigame loading, and scene reloading operations.

	*/
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
	
	#endregion
}
