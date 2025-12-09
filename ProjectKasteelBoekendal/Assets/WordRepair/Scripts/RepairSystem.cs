using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RepairSystem : MonoBehaviour
{
    public WordManager wordManager;
    public Customer customer;
    public BlacksmithWordRepair blacksmith;
    [SerializeField] private DialogueSystem dialogueSystem;
    private int score = 0;

    private Dictionary<SmithMaterialEnum, int> allMaterials = new Dictionary<SmithMaterialEnum, int>
    {
        {SmithMaterialEnum.Ijzer, 2},
        {SmithMaterialEnum.Staal, 3},
        {SmithMaterialEnum.Goud, 2}
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
            if (allMaterials[0] > 0)
            {
                CompleteRepair();
                customer.ProgressOrder();
            }
            else
            {
                EndRepairSession();
            }
            score++;

            string resultText = "Goed gedaan! Het woord is correct.";
            blacksmith.BlacksmithFeedback(resultText);

            Invoke(nameof(NextWord), 2f);
        }
        else
        {
            string resultText = "Fout! Probeer het opnieuw.";
            blacksmith.BlacksmithFeedback(resultText);
        }
    }


    private void CompleteRepair()
    {
        foreach (var material in allMaterials.Keys)
        {
            if (allMaterials[material] > 0)
            {
                allMaterials[material] -= 1;
                return;
            }
        }

        allItems.Add(SmithItemEnum.Hammer); // Example item, replace with actual logic
    }

    private void EndRepairSession()
    {
        string resultText = "Alle reparaties voltooid!";
        blacksmith.BlacksmithFeedback(resultText);
        Debug.Log("Items repaired: " + allItems.Count);
        Debug.Log("Items: " + string.Join(", ", allItems));
    }

    private void NextWord()
    {
        wordManager.Next();
        blacksmith.ShowMaterials(allMaterials);
    }
}
