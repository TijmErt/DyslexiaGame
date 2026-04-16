using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("Persistent Objects")]
    public GameObject[] persistentObjects; 
    private void Awake()
    {
        if (instance != null)
        {
            CleanUpAndDestory();
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            MarkPersistentObjects();
        }
    }

    private void CleanUpAndDestory()
    {
        foreach (GameObject persistentObject in persistentObjects)
        {
            Destroy(persistentObject);
        }
        Destroy(gameObject);
    }

    private void MarkPersistentObjects()
    {
        foreach (GameObject persistentObject in persistentObjects)
        {
            if (persistentObject != null)
            {
                DontDestroyOnLoad(persistentObject);
            }
        }
    }
}
