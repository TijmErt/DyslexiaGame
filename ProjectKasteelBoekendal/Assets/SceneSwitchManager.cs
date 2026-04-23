using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class SceneSwitchManager : MonoBehaviour
{
    public static SceneSwitchManager instance;
    
    [SerializeField] private string currentScene;
    [SerializeField] private string previousScene;
    [SerializeField] private Vector3 PlayerPosition;
    [SerializeField] private Quaternion PlayerRotation;
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

    #region MinigameScene
        public void LoadMinigameScene(string sceneName)
        {
            previousScene = string.IsNullOrEmpty(currentScene)
                ? SceneManager.GetActiveScene().name
                : currentScene;

            Transform player = GameObject.FindGameObjectWithTag("Player1").transform;
            PlayerPosition = player.position;
            PlayerRotation = player.rotation;

            SceneManager.sceneLoaded += OnMinigameLoaded;
            SceneManager.LoadScene(sceneName);
        }
        private void OnMinigameLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= OnMinigameLoaded;
            currentScene = scene.name;
        }
        
    #endregion

    #region PreviousScene

    public void LoadPreviousScene()
    {
        //swaps values around as the previous scene will become the current scene and vice versa

        SceneManager.sceneLoaded += OnPreviousSceneLoaded;
        SceneManager.LoadScene(previousScene);
    }
    private void OnPreviousSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnPreviousSceneLoaded;

        GameObject player = GameObject.FindGameObjectWithTag("Player1");

        if (player != null)
        {
            player.transform.SetPositionAndRotation(PlayerPosition, PlayerRotation);

            // IMPORTANT: fix NavMeshAgent snapping
            var agent = player.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.Warp(PlayerPosition);
            }
        }
        previousScene = currentScene;
        currentScene = SceneManager.GetActiveScene().name;
        
    }

    #endregion

    public void ReloadScene()
    {
        SceneManager.LoadScene(currentScene);
    }
    
    


    
}
