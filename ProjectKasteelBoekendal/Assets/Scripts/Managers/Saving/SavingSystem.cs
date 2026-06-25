using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Managers.Saving
{
    /// <summary>
    /// Handles the serialization, deserialization, and management of save files.
    /// Collects all objects implementing ISaveable and stores their state in a save file.
    /// </summary>
    public class SavingSystem
    {
        /// <summary>
        /// Returns the full file path for a save file.
        /// </summary>
        /// <param name="fileName">Name of the save file.</param>
        /// <returns>The absolute path to the save file.</returns>
        private string GetPath(string fileName)
        {
            return Application.persistentDataPath + "/" + fileName;
        }
        
        /// <summary>
        /// Saves the current game state to a file by collecting data from all objects
        /// implementing the ISaveable interface within the current scene.
        /// </summary>
        /// <param name="fileName">Name of the save file to create or overwrite.</param>
        /// <returns>
        /// True if the save operation succeeds; otherwise false.
        /// </returns>
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

        /// <summary>
        /// Loads a game state from a save file and restores the state of all objects
        /// implementing the ISaveable interface within the current scene.
        /// </summary>
        /// <param name="fileName">Name of the save file to load.</param>
        /// <returns>
        /// True if the load operation succeeds; otherwise false.
        /// </returns>
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
        
        /// <summary>
        /// Deletes an existing save file from disk.
        /// </summary>
        /// <param name="fileName">Name of the save file to remove.</param>
        /// <returns>
        /// True if the file was successfully deleted; otherwise false.
        /// </returns>
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
        
        /// <summary>
        /// Retrieves the names of all available save files.
        /// </summary>
        /// <returns>
        /// An array containing the names of all save files without their extensions.
        /// </returns>
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
