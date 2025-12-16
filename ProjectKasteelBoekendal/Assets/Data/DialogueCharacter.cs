using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DialogueCharacter", menuName = "Scriptable Objects/DialogueCharacter")]
public class DialogueCharacter : ScriptableObject
{
    public string characterName;
    public string currentState;
    public bool IsSpokenTo;
    public List<DialogueEntry> entries;         
    private Dictionary<string, string> _lookup; 
    
    public void BuildLookup() 
    { 
        _lookup = new Dictionary<string, string>(); 
        foreach (DialogueEntry e in entries) 
            _lookup[e.State] = e.Text; 
    } 
    
    public string Get()
    { 
        if (_lookup == null) BuildLookup(); 
        return _lookup.TryGetValue(currentState, out var text) ? text : ""; 
    }

    public void ChangeState(string newState)
    {
        currentState = newState;
    }
}
