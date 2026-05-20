using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Managers.Saving
{
    public class SavingSystem
    {
        private string GetPath(string fileName)
        {
            return Application.persistentDataPath + "/" + fileName;
        }
        public bool SaveGameToFile(string fileName)
        {
            try
            {
                SaveFile saveFile = new SaveFile();
            
                ISaveable[] saveables = Object.FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None)
                    .OfType<ISaveable>()
                    .ToArray();

                foreach (ISaveable saveable in saveables)
                {
                    SaveEntry entry = new SaveEntry();

                    entry.UID = saveable.UID;
                    entry.JsonData = JsonUtility.ToJson(saveable.CaptureState());

                    saveFile.saveEntries.Add(entry);
                }
                
                Debug.Log("saveEntries Count : " + saveFile.saveEntries.Count);
                string json = JsonUtility.ToJson(saveFile, true);

                File.WriteAllText(GetPath(fileName), json);
                Debug.Log(GetPath(fileName));

                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                return false;
            }

        }

        public bool LoadGameFromFile(string fileName)
        {
            string path = GetPath(fileName);

            if (!File.Exists(path))
            {
                Debug.Log("No save file found");
                return false;
            }

            try
            {
                string fileJson = File.ReadAllText(path);
                SaveFile saveFile = JsonUtility.FromJson<SaveFile>(fileJson);


                Dictionary<string, string> lookup = new();
                foreach (SaveEntry entry in saveFile.saveEntries)
                {
                    if (!lookup.ContainsKey(entry.UID))
                        lookup.Add(entry.UID, entry.JsonData);
                    else
                        Debug.LogWarning($"Duplicate UID found: {entry.UID}");lookup[entry.UID] = entry.JsonData;
                }


                // Find all saveables
                ISaveable[] saveables = Object.FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include,
                        FindObjectsSortMode.None)
                    .OfType<ISaveable>()
                    .ToArray();


                foreach (ISaveable saveable in saveables)
                {
                    if (!lookup.TryGetValue(saveable.UID, out string json))
                        continue;
                    saveable.RestoreState(json);
                }
                
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return false;
            }

        }

        public bool RemoveGameSave(string fileName)
        {
            string path = GetPath(fileName);

            if (!File.Exists(path))
            {
                Debug.LogWarning("Save file does not exist");
                return false;
            }

            try
            {
                File.Delete(path);


                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to delete save file: {e.Message}");
                return false;
            }
        }
        
        public string[] GetAllSaveFiles()
        {
            string[] files = Directory.GetFiles(Application.persistentDataPath, "*.sav");

            for (int i = 0; i < files.Length; i++)
            {
                files[i] = Path.GetFileNameWithoutExtension(files[i]);
            }

            return files;
        }
    }
}
