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
    
    private Dictionary<string, Flag> eventFlagDictionary = new Dictionary<string, Flag>();
    
    public event Action<string, bool> OnFlagChanged;
    public string UID => "EventFlagManager";
    /*
        Potential alternative. Swapping this out with a hierarchy system 
        
        example class
        
        internal class FlagComp{
            public string Name;
            public bool Enabled
            public list<FlagComp> subFlag
        }
        
        Scene->Type (think area or npc)->name-> subtype (main, tutorial, etc.) -> state 
        then use the collective name of that hierarchy for the dictionary.
        
        AT the moment we have it listed as
        Kitchen.MiniG.MemPuz.Tut.Open
        
        This could be changed to have each word be their own FlagComp. The reason for this is mainly to increase readability, as you can group for example all kitchen events under kitchen and so forth. making it work more like a tree then a list.
        These I would suggest to intialize as a dictionary or something similar.
     */
    
    


    private void Awake()
    {
        instance = this;

        InitializeDictionary();
    }
    
    /// <summary>
    /// Initializes the List of Flags into a dictionary for better and more performative lookup of the eventflag when needing to change a value.
    /// </summary>
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
    }
    
    /// <summary>
    /// Checks whether a flag exists and is currently enabled.
    /// </summary>
    /// <param name="flagName">Name of the flag to check.</param>
    /// <returns>
    /// True if the flag exists and is enabled; otherwise false.
    /// </returns>
    public bool IsFlagEnabled(string flagName)
    {
        if (eventFlagDictionary.TryGetValue(flagName, out Flag flag))
        {
            return flag.Enabled;
        }
        return false;
    }
    
    /// <summary>
    /// Changes the enabled state of a flag and notifies any listeners that the flag has changed.
    /// </summary>
    /// <param name="flagName">Name of the flag to modify.</param>
    /// <param name="enabled">The new state of the flag.</param>
    public void ChangeFlagState(string flagName, bool enabled)
    {
        if (eventFlagDictionary.ContainsKey(flagName))
        {
            eventFlagDictionary[flagName].Enabled = enabled;
            OnFlagChanged?.Invoke(flagName, enabled);
            
        }
        else
        {
            Debug.LogWarning($"Flag '{flagName}' not found.");
        }
    }


    #region Saving
    public object CaptureState()
    {
        EventFlagData data = new EventFlagData
        {
            Flags = new List<string>()
        };

        //This only saves the flags that are already triggered, which will save on space and iteration
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
    #endregion
}


