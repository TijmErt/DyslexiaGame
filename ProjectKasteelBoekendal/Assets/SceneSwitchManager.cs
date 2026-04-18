using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitchManager : MonoBehaviour
{
    public static SceneSwitchManager instance;
    
    [SerializeField] private string currentScene;
    [SerializeField] private string previousScene;
    private Vector3 PlayerPosition;
    private Quaternion PlayerRotation;
    private void Awake()
    {

        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        instance = this;
        DontDestroyOnLoad(gameObject);
        currentScene = SceneManager.GetActiveScene().name;
    }

    public void LoadMinigameScene(string scene)
    {
        previousScene = currentScene.Equals("") ? SceneManager.GetActiveScene().name : currentScene;
        Transform player = GameObject.FindGameObjectWithTag("Player1").transform;
        PlayerPosition = player.position;
        PlayerRotation = player.rotation;
        SceneManager.sceneLoaded += (scene, mode) =>
        {
            currentScene = SceneManager.GetActiveScene().name;
        };
        SceneManager.LoadScene(scene);
        

    }
    public void LoadPreviousScene()
    {
        SceneManager.sceneLoaded += (scene, mode) =>
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player1"); 
            player.transform.SetPositionAndRotation(PlayerPosition, PlayerRotation);
        };

        SceneManager.LoadScene(previousScene);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(currentScene);
    }
}
