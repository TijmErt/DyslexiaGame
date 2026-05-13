using UnityEngine;

namespace Managers.Saving
{
    public class SaveManager : MonoBehaviour
    {
        public static SaveManager instance;
        
        [SerializeField] private string saveFileName = "savefile.sav";
        
        private readonly SavingSystem savingSystem = new();
        
        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
        public void Save()
        {
            if(savingSystem.SaveGameToFile(saveFileName))
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
            if (savingSystem.LoadGameFromFile(saveFileName))
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
            if (savingSystem.RemoveGameSave(saveFileName))
            {
                Debug.Log("Save file deleted successfully");
            }
            else
            {
                Debug.Log("Failed to delete the save file");
            }
        }
    }
}
