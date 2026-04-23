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
    /*
        Potential alternative. Swapping this our with a hierarchy system 
        
        example class
        
        internal class FlagComp{
            public string Name;
            public bool Enabled
            public list<FlagComp> subFlag
        }
        
        Scene->Type (think area or npc)->name-> subtype (main, tutorial, etc.) -> state 
        then use the collective name of that hierarchy for the dictionary.
     */
    
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
    private void OnValidate()
    {
        RevalidateDictionary();
    }

    private void RevalidateDictionary()
    {
        foreach (Flag flag in eventFlags)
        {
            if (eventFlagDictionary.ContainsKey(flag.Name))
            {
                eventFlagDictionary[flag.Name] = flag.Enabled;
            }
        }
    }
    private void InitializeDictionary()
    {
        eventFlagDictionary.Clear();

        foreach (Flag flag in eventFlags)
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
        {
            return value;
        }
        return false;
    }

    public void ChangeFLagState(string flagName, bool enabled)
    {
        if (eventFlagDictionary.ContainsKey(flagName))
        {
            eventFlagDictionary[flagName] = enabled;

            int index = eventFlags.FindIndex(flag => flag.Name == flagName);
            if (index >= 0)
            {
                eventFlags[index].Enabled = enabled;
            }
        }
        else
        {
            Debug.LogWarning($"Flag '{flagName}' not found.");
        }
    }
}


