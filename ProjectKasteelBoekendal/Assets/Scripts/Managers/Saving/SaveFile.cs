using System.Collections.Generic;

namespace Managers.Saving
{
    [System.Serializable]
    public class SaveFile
    {
        public List<SaveEntry> saveEntries = new List<SaveEntry>();
    }

    [System.Serializable]
    public class SaveEntry
    {
        public string UID;
        public string JsonData;
    }
}