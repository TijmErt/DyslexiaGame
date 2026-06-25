using System;
using Managers.Quest;
using Managers.Saving;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages scene transitions while preserving the player's location and orientation.
/// Also stores scene navigation data for returning from minigames and supports save/load functionality.
/// </summary>
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

        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        currentScene = SceneManager.GetActiveScene().name;
    }
    
    /// <summary>
    /// Saves the player's current position and rotation for later restoration.
    /// </summary>
    private void SavePlayerTransform()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player1").transform;
        PlayerPosition = player.position;
        PlayerRotation = player.rotation;
    }
    /// <summary>
    /// Restores the player's saved position and rotation.
    /// If a NavMeshAgent is present, it is warped to the restored position
    /// to prevent navigation-related position corrections.
    /// </summary>
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

    /// <summary>
    /// Loads a minigame scene while storing the current scene and player transform
    /// so they can be restored when returning.
    /// </summary>
    /// <param name="sceneName">Name of the minigame scene to load.</param>
    public void LoadMinigameScene(string sceneName)
    {
        previousScene = string.IsNullOrEmpty(currentScene)
            ? SceneManager.GetActiveScene().name
            : currentScene;

        SavePlayerTransform();

        SceneManager.sceneLoaded += OnMinigameLoaded;
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Called when a minigame scene has finished loading.
    /// Updates the current scene reference and unregisters the load callback.
    /// </summary>
    /// <param name="scene">The loaded scene.</param>
    /// <param name="mode">The scene loading mode.</param>
    private void OnMinigameLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnMinigameLoaded;
        currentScene = scene.name;
        QuestManager.instance.CheckAvailability();
    }

    #endregion

    #region PreviousScene

    /// <summary>
    /// Returns to the previously loaded scene.
    /// </summary>
    public void LoadPreviousScene()
    {
        //swaps values around as the previous scene will become the current scene and vice versa

        SceneManager.sceneLoaded += OnPreviousSceneLoaded;
        SceneManager.LoadScene(previousScene);
    }

    /// <summary>
    /// Called when the previous scene has finished loading.
    /// Restores the player's transform and updates the scene references.
    /// </summary>
    /// <param name="scene">The loaded scene.</param>
    /// <param name="mode">The scene loading mode.</param>
    private void OnPreviousSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnPreviousSceneLoaded;

        SetPlayerTransform();
        QuestManager.instance.CheckAvailability();

        previousScene = currentScene;
        currentScene = SceneManager.GetActiveScene().name;
    }

    #endregion

    /// <summary>
    /// Reloads the currently tracked scene.
    /// </summary>
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