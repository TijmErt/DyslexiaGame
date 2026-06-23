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
    
    private Dictionary<string, bool> eventFlagDictionary= new Dictionary<string, bool>();
    public event Action<string, bool> OnFlagChanged;
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
            OnFlagChanged?.Invoke(flagName, enabled);
            
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


    public object CaptureState()
    {
        EventFlagData data = new EventFlagData
        {
            Flags = new List<Flag>()
        };

        foreach (Flag flag in eventFlags)
        {
            data.Flags.Add(new Flag
            {
                Name = flag.Name,
                Enabled = flag.Enabled
            });
        }

        return data;
    }

    public void RestoreState(string state)
    {
        EventFlagData data = JsonUtility.FromJson<EventFlagData>(state);

        if (data.Flags == null)
            return;

        eventFlags.Clear();
        eventFlagDictionary.Clear();

        foreach (Flag flag in data.Flags)
        {
            eventFlags.Add(new Flag
            {
                Name = flag.Name,
                Enabled = flag.Enabled
            });

            eventFlagDictionary[flag.Name] = flag.Enabled;
        }
    }

    private struct EventFlagData
    {
        public List<Flag> Flags;
    }
}


