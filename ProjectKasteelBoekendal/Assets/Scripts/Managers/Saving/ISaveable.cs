namespace Managers.Saving
{ 
    public interface ISaveable 
    {
        string UID { get; }
        object CaptureState();
        void RestoreState(string state);
    }
}
