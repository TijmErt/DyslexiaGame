using UnityEngine;

namespace Managers.Saving
{
    public class SaveManager : MonoBehaviour
    {
        public static SaveManager instance;
        
        [SerializeField] private string saveFileName = "savefile";
        private string FullSaveFileName => saveFileName + ".sav"; //Ensures that the save files all use the correct file type
        
        private readonly SavingSystem savingSystem = new();
        
        private void Awake()
        {
            instance = this;
        }
        
      
        public void Save()
        {
            if(savingSystem.SaveGameToFile(FullSaveFileName))
            {
                Debug.Log("Save file saved successfully");
            }
            else
            {
                Debug.Log("Failed to save the save file");
            }
              
        }

        public void Load()
        {
            if (savingSystem.LoadGameFromFile(FullSaveFileName))
            {
                Debug.Log("Save file Loaded successfully");
            }
            else
            {
                Debug.Log("Failed to Load the save file");
            }
        }

        public void Remove()
        {
            if (savingSystem.RemoveGameSave(FullSaveFileName))
            {
                Debug.Log("Save file deleted successfully");
            }
            else
            {
                Debug.Log("Failed to delete the save file");
            }
        }
        
        public void ChangeSaveFile(string newSaveFileName)
        {
            if (string.IsNullOrWhiteSpace(newSaveFileName))
                return;

            saveFileName = newSaveFileName.Trim();
        }
        public string[] GetAllSaveFiles()
        {
            return savingSystem.GetAllSaveFiles();
        }
    }
}
