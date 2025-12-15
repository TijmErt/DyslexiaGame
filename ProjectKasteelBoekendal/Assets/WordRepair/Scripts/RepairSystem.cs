using System.Collections.Generic;
using System.Dynamic;
using TMPro;
using UnityEngine;

public class RepairSystem : MonoBehaviour
{
    public WordManager wordManager;
    public Customer customer;
    public BlacksmithWordRepair blacksmith;
    [SerializeField] private DialogueSystem dialogueSystem;
    [SerializeField] private TextMeshProUGUI endPopupText;
    [SerializeField] private List<GameObject> uiToDisableOnEnd = new List<GameObject>();


    private Dictionary<SmithMaterialEnum, int> allMaterials = new Dictionary<SmithMaterialEnum, int>
    {
        {SmithMaterialEnum.ijzer, 1},
        {SmithMaterialEnum.staal, 1},
        {SmithMaterialEnum.goud, 1}
    };

    private void Start()
    {
        blacksmith.ShowMaterials(allMaterials);
    }

    private List<SmithItemEnum> allItems = new List<SmithItemEnum>();

    public void CompleteWord()
    {
        bool correct = wordManager.CheckAnswer();

        if (correct)
        {
            CompleteRepair();
            customer.ProgressOrder();

            string resultText = "Goed gedaan! Het woord is correct.";
            blacksmith.BlacksmithFeedback(resultText);
        }
        else
        {
            string resultText = "Fout! Probeer het opnieuw.";
            blacksmith.BlacksmithFeedback(resultText);
        }

        List<SmithMaterialEnum> availableMaterials = GetAvailableMaterials();

        if (availableMaterials.Count <= 0)
        {
            Invoke(nameof(EndRepairSession), 2f);
            return;
        }

        Invoke(nameof(NextWord), 2f);
    }


    private void CompleteRepair()
    {
        UseMaterial();

        AddRewardItem();
    }

    private void UseMaterial()
    {
        // Get all materials that have at least 1 available
        List<SmithMaterialEnum> availableMaterials = GetAvailableMaterials();

        // If we have available materials, pick a random one
        if (availableMaterials.Count > 0)
        {
            int randomIndex = Random.Range(0, availableMaterials.Count);
            SmithMaterialEnum randomMaterial = availableMaterials[randomIndex];
            allMaterials[randomMaterial] -= 1;
        }
    }

    private void AddRewardItem()
    {
        // Get all possible SmithItemEnum values
        SmithItemEnum[] possibleItems = (SmithItemEnum[])System.Enum.GetValues(typeof(SmithItemEnum));
        if (possibleItems.Length == 0) return;

        // Pick a random item
        int randomIndex = Random.Range(0, possibleItems.Length);
        SmithItemEnum reward = possibleItems[randomIndex];
        allItems.Add(reward);
    }

    private List<SmithMaterialEnum> GetAvailableMaterials()
    {
        List<SmithMaterialEnum> availableMaterials = new List<SmithMaterialEnum>();
        
        foreach (var material in allMaterials.Keys)
        {
            if (allMaterials[material] > 0)
            {
                availableMaterials.Add(material);
            }
        }

        return availableMaterials;
    }

    private void EndRepairSession()
    {
        
        foreach (var ui in uiToDisableOnEnd)
        {
            if (ui != null)
            {
                ui.SetActive(false);
            }
        }

        string resultText = "Alle reparaties voltooid!";
        endPopupText.text = resultText + "\nGerepareerde items:" + allItems.Count + "\nItems:\n" + string.Join(", ", allItems);
        endPopupText.transform.parent.gameObject.SetActive(true);
    }

    private void NextWord()
    {
        wordManager.Next();
        blacksmith.ShowMaterials(allMaterials);
    }
}
