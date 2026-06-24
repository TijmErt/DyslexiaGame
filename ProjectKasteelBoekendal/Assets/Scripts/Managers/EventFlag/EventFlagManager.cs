using System;
using System.Collections.Generic;
using Managers.Saving;
using UnityEngine;

[System.Serializable]
internal class Flag
{
    public string Name;
    public bool Enabled;
}
public class EventFlagManager : MonoBehaviour, ISaveable
{
    public static EventFlagManager instance;
    [SerializeField]private List<Flag> eventFlags; 
    public string UID => "EventFlagManager";
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
    
    private Dictionary<string, Flag> eventFlagDictionary= new Dictionary<string, Flag>();

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

        foreach (Flag flag in eventFlags)
        {
            if (!eventFlagDictionary.ContainsKey(flag.Name))
            {
                eventFlagDictionary.Add(flag.Name, flag);
            }
        }
        Debug.Log(eventFlagDictionary);
        Debug.Log(eventFlagDictionary.Count);
    }

    public bool IsFlagEnabled(string flagName)
    {
        if (eventFlagDictionary.TryGetValue(flagName, out Flag flag))
        {
            return flag.Enabled;
        }
        return false;
    }

    public void ChangeFLagState(string flagName, bool enabled)
    {
        if (eventFlagDictionary.ContainsKey(flagName))
        {
            eventFlagDictionary[flagName].Enabled = enabled;

            // int index = eventFlags.FindIndex(flag => flag.Name == flagName);
            // if (index >= 0)
            // {
            //     eventFlags[index].Enabled = enabled;
            // }
        }
        else
        {
            Debug.LogWarning($"Flag '{flagName}' not found.");
        }
    }


    public object CaptureState()
    {
        EventFlagData data = new EventFlagData
        {
            Flags = new List<string>()
        };

        foreach (Flag flag in eventFlags)
        {
            if (flag.Enabled)
            {
                data.Flags.Add(flag.Name);
            }
        }

        return data;
    }

    public void RestoreState(string state)
    {
        EventFlagData data = JsonUtility.FromJson<EventFlagData>(state);

        if (data.Flags == null)
            return;

        // First reset all flags to false
        foreach (Flag flag in eventFlags)
        {
            flag.Enabled = false;
        }

        // Then enable only the saved ones
        foreach (string flagName in data.Flags)
        {
            if (eventFlagDictionary.TryGetValue(flagName, out Flag flag))
            {
                flag.Enabled = true;
            }
            else
            {
                Debug.LogWarning($"Saved flag '{flagName}' does not exist in EventFlagManager.");
            }
        }
    }

    private struct EventFlagData
    {
        public List<string> Flags;
    }
}


