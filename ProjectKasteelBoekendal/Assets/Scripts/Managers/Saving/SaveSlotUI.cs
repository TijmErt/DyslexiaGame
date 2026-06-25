using TMPro;
using UnityEngine;

public class SaveSlotUI : MonoBehaviour
{
    [SerializeField] private TMP_Text saveNameText;

    private string saveName;
    private SaveMenuUI menu;

    public void Setup(string newSaveName, SaveMenuUI saveMenu)
    {
        saveName = newSaveName;
        menu = saveMenu;

        saveNameText.text = saveName;
    }

    public void Load()
    {
        menu.LoadSave(saveName);
    }

    public void Delete()
    {
        menu.DeleteSave(saveName);
    }
}
