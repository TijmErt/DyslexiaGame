using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] private string CurrentScene = "";
    private Transform playerPosition;
    private string previousScene = "";

    private void Awake()
    {
        CurrentScene = SceneManager.GetActiveScene().name;
    }

    public void LoadAreaScene()
    {

        SceneManager.sceneLoaded += (scene, mode) =>
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.transform.SetPositionAndRotation(playerPosition.position, playerPosition.transform.rotation);
        };

        SceneManager.LoadScene(previousScene);
    }

    public void LoadMinigameScene(string sceneName)
    {
        string previousScene;
        if(CurrentScene.Equals("")) previousScene = SceneManager.GetActiveScene().name;
        else previousScene = CurrentScene;
        playerPosition = GameObject.FindGameObjectWithTag("Player1").transform;
        CurrentScene = sceneName;
        SceneManager.LoadScene(sceneName);
    }

    public void reloadScene()
    {
        SceneManager.LoadScene(CurrentScene);
    }
}
