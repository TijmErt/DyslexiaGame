using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers.Saving
{
    /// <summary>
    /// The SavingMediator acts as a middle layer between other scripts/UI systems
    /// and the SaveManager.
    ///
    /// This helps reduce direct dependency on the SaveManager singleton instance,
    /// lowering the chance of null reference errors if the manager is unavailable
    /// or has not yet been initialized.
    ///
    /// It also exposes simplified wrapper methods that can easily be used
    /// by UI systems and Unity Events through the Inspector.
    /// </summary>
    public class SavingMediator : MonoBehaviour
    {
        #region Core Save Methods

        /*

         Core wrapper methods for interacting with the SaveManager.

         These methods directly expose the primary save system functionality
         without modifying or preparing save file data beforehand.

         They are intended for general gameplay systems, autosaves,
         debugging, and other direct save interactions.

        */
        
        public void Save()
        {
            SaveManager.instance.Save();
        }
        public void Load()
        {
            SaveManager.instance.Load();
        }
        public void Remove()
        {
            SaveManager.instance.Remove();
        }

        public void ChangeSaveFile(string newSaveFileName)
        {
            SaveManager.instance.ChangeSaveFile(newSaveFileName);
        }
        
        public string[] GetAllSaveFiles()
        {
            return SaveManager.instance.GetAllSaveFiles();
        }
        
        #endregion
        
        #region Save File Utility Methods

        /*

         Utility methods for save file workflows.

         These methods combine multiple save operations into simplified
         helper calls that work with specific save file names.

         Primarily intended for save menus and UI systems,
         though they can also be used by gameplay systems when needed.

        */
        public void LoadSave(string saveName)
        {
            ChangeSaveFile(saveName);
            Load();
        }

        public void DeleteSave(string saveName)
        {
            ChangeSaveFile(saveName);
            Remove();
        }

        public void CreateSave(string saveName)
        {
            ChangeSaveFile(saveName);
            Save();
        }
        #endregion

    }
}
