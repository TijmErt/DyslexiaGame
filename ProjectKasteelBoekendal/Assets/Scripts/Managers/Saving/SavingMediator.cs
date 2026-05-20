using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers.Saving
{
    public class SavingMediator : MonoBehaviour
    {

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

        public string[] GetAllSaveFiles()
        {
            return SaveManager.instance.GetAllSaveFiles();
        }
    }
}
