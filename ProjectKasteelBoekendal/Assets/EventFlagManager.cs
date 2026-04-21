using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
internal class Flag
{
    public string Name;
    public bool Enabled;
}
public class EventFlagManager : MonoBehaviour
{
    public static EventFlagManager instance;
    [SerializeField]private List<Flag> eventFlags;
    private Dictionary<string, bool> eventFlagDictionary= new Dictionary<string, bool>();

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        instance = this;
        DontDestroyOnLoad(gameObject);
        InitializeDictionary();
    }
    private void InitializeDictionary()
    {
        eventFlagDictionary.Clear();

        foreach (var flag in eventFlags)
        {
            if (!eventFlagDictionary.ContainsKey(flag.Name))
            {
                eventFlagDictionary.Add(flag.Name, flag.Enabled);
            }
        }
        Debug.Log(eventFlagDictionary);
        Debug.Log(eventFlagDictionary.Count);
    }

    public bool IsFlagEnabled(string flagName)
    {
        if (eventFlagDictionary.TryGetValue(flagName, out bool value))
            return value;

        return false;
    }

    public void ChangeFLagState(string flagName, bool enabled)
    {
        eventFlagDictionary[flagName] = enabled;
        eventFlags[eventFlags.FindIndex(flag => flag.Name == flagName)].Enabled = enabled;
    }
}


