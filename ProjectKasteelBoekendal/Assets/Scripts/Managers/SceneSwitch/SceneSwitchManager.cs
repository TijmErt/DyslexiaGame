using System;
using Managers.Saving;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class SceneSwitchManager : MonoBehaviour, ISaveable
{
    public static SceneSwitchManager instance;
    public string UID => "SceneSwitchManager";
    [SerializeField] private string currentScene;
    [SerializeField] private string previousScene;
    [SerializeField] private Vector3 PlayerPosition;
    [SerializeField] private Quaternion PlayerRotation;

    private void Awake()
    {

        instance = this;

        currentScene = SceneManager.GetActiveScene().name;
    }

    private void SavePlayerTransform()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player1").transform;
        PlayerPosition = player.position;
        PlayerRotation = player.rotation;
    }

    private void SetPlayerTransform()
    {
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
    }
    #region MinigameScene

    public void LoadMinigameScene(string sceneName)
    {
        previousScene = string.IsNullOrEmpty(currentScene)
            ? SceneManager.GetActiveScene().name
            : currentScene;

        SavePlayerTransform();

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

        SetPlayerTransform();

        previousScene = currentScene;
        currentScene = SceneManager.GetActiveScene().name;
    }

    #endregion

    public void ReloadScene()
    {
        SceneManager.LoadScene(currentScene);
    }

    #region Saving



    public object CaptureState()
    {
        SavePlayerTransform(); // this needs to be removed aswell as the players location but that is for later
        SceneSwitchData data = new SceneSwitchData
        {
            Position = new float[] { PlayerPosition.x, PlayerPosition.y, PlayerPosition.z },
            Rotation = new float[] { PlayerRotation.x, PlayerRotation.y, PlayerRotation.z, PlayerRotation.w },
            CurrentScene = currentScene,
            PreviousScene = previousScene
        };
        return data;
    }

    public void RestoreState(string state)
    {
        SceneSwitchData data = JsonUtility.FromJson<SceneSwitchData>(state);
        PlayerPosition = new Vector3(data.Position[0], data.Position[1], data.Position[2]);
        PlayerRotation = new Quaternion(data.Rotation[0], data.Rotation[1], data.Rotation[2], data.Rotation[3]);
        currentScene = data.CurrentScene; 
        previousScene = data.PreviousScene;
        SetPlayerTransform();
    }
    
    private struct SceneSwitchData
    {
        public float[] Position;
        public float[] Rotation;
        public string CurrentScene;
        public string PreviousScene;
    } 
    #endregion
}