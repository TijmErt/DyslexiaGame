using UnityEngine;


/// <summary>
/// The EventFlagMediator acts as a middle layer between other scripts/UI systems
/// and the EventFlagManager.
///
/// It provides a simplified and centralized interface for enabling, disabling,
/// and querying event flags without requiring direct access to the manager singleton.
///
/// This reduces tight coupling between gameplay systems and the underlying
/// flag system implementation, while still allowing easy Inspector/Unity Event usage.
/// </summary>
public class EventFlagMediator : MonoBehaviour
{
    #region Flag Control Methods

    /*

     Wrapper methods for interacting with the EventFlagManager.

     These methods expose basic flag functionality such as enabling,
     disabling, and checking flag states.

     They are intended for use by gameplay systems, triggers,
     and UI events without requiring direct manager access.

    */
    public void enableFlag(string flagName)
    {
        EventFlagManager.instance.ChangeFlagState(flagName, true);
    }
    public void disableFlag(string flagName)
    {
        EventFlagManager.instance.ChangeFlagState(flagName, false);
    }

    public bool getFlagState(string flagName)
    {
        return EventFlagManager.instance.IsFlagEnabled(flagName);
    }
    #endregion
}
