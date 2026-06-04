using Managers.Saving;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SaveMenuUI : MonoBehaviour
{
    [Header("Mediator")]
    [SerializeField] private SavingMediator savingMediator;

    [Header("Menu")]
    [SerializeField] private GameObject panel;

    [Header("Save List")]
    [SerializeField] private Transform saveListParent;
    [SerializeField] private GameObject saveSlotPrefab;

    [Header("Create Save")]
    [SerializeField] private TMP_InputField saveNameInput;

    private void Start()
    {
        panel.SetActive(false);
    }

    private void Update()
    {
        if (Keyboard.current.f5Key.wasPressedThisFrame)
        {
            if (!savingMediator)
            {
                savingMediator = FindFirstObjectByType<SavingMediator>();
            }
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        bool active = !panel.activeSelf;

        panel.SetActive(active);
        if (active)
        {
            RefreshSaveList();
            Time. timeScale = 0; 
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
    public void RefreshSaveList()
    {
        foreach (Transform child in saveListParent)
        {
            Destroy(child.gameObject);
        }

        string[] saves = savingMediator.GetAllSaveFiles();

        foreach (string save in saves)
        {
            GameObject slot = Instantiate(saveSlotPrefab, saveListParent);

            SaveSlotUI slotUI = slot.GetComponent<SaveSlotUI>();

            slotUI.Setup(save, this);
        }
    }

    public void CreateSave()
    {
        string saveName = saveNameInput.text;

        if (string.IsNullOrWhiteSpace(saveName))
        {
            saveName = "savefile";
        }

        savingMediator.CreateSave(saveName);

        RefreshSaveList();
    }

    public void LoadSave(string saveName)
    {
        savingMediator.LoadSave(saveName);
    }

    public void DeleteSave(string saveName)
    {
        savingMediator.DeleteSave(saveName);

        RefreshSaveList();
    }
}
