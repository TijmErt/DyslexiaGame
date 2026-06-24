using System;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{

	[field: SerializeField] public bool IsMinigame { get; set; } = false;
	
	private SceneMediator SceneMediator { get; set; }

	public void Start() {
		this.SceneMediator = FindFirstObjectByType<SceneMediator>();
	}

	public void OpenMenu() {
		Time.timeScale = 0;
		this.transform.GetChild(1).gameObject.SetActive(true);
	}

	public void OpenSettings() {
		throw new NotImplementedException();
	}
	
	public void CloseMenu() {
		Time.timeScale = 1;
		this.transform.GetChild(1).gameObject.SetActive(false);
	}

	public void GoToPreviousScene() {
		this.SceneMediator.LoadPreviousScene();
	}
}
