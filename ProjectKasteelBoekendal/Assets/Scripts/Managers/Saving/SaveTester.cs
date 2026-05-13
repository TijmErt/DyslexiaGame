using Managers.Saving;
using UnityEngine;
using UnityEngine.InputSystem;

public class SaveTester : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Update()
    {
        if (Keyboard.current.f5Key.wasPressedThisFrame)
        {
            SaveManager.instance.Save();
        }

        if (Keyboard.current.f9Key.wasPressedThisFrame)
        {
            SaveManager.instance.Load();
        }

        if (Keyboard.current.f11Key.wasPressedThisFrame)
        {
            SaveManager.instance.Remove();
        }
    }
}
