using UnityEngine;

/// <summary>
/// This is a mediator between EventFlagManager and any UnityEvents like Buttons
/// </summary>
public class EventFlagMediator : MonoBehaviour
{
    public static EventFlagMediator instance;
    public void enableFlag(string flagName)
    {
        EventFlagManager.instance.ChangeFLagState(flagName, true);
    }
    public void disableFlag(string flagName)
    {
        EventFlagManager.instance.ChangeFLagState(flagName, false);
    }

    public bool getFlagState(string flagName)
    {
        return EventFlagManager.instance.IsFlagEnabled(flagName);
    }
}
