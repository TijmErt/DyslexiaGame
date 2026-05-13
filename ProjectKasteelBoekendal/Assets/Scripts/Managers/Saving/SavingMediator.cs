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
    }
}
