namespace Managers.Saving
{ 
    
    /// <summary>
    /// Interface for objects that can save and restore their state through the save system.
    /// </summary>
    public interface ISaveable 
    {        
        /// <summary>
        /// Unique identifier used by the save system to store and retrieve this object's data.
        /// </summary>
        string UID { get; }
        
        /// <summary>
        /// Captures the current state of the object for serialization and saving.
        /// </summary>
        /// <returns>
        /// An object containing the data required to restore the object's state.
        /// </returns>
        object CaptureState();
        
        /// <summary>
        /// Restores the object's state from previously saved data.
        /// </summary>
        /// <param name="state">
        /// Serialized save data in the form of JSON.
        /// </param>
        void RestoreState(string state);
    }
}
