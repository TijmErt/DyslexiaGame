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
        {SmithMaterialEnum.Iron, 5},
        {SmithMaterialEnum.Steel, 3},
        {SmithMaterialEnum.Gold, 2}
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
            if (allMaterials.Count > 0)
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

        blacksmith.ShowMaterials(allMaterials);
    }


    private void CompleteRepair()
    {
        // check before calling if allMaterils.Count > 0
        allMaterials[0] -= 1;
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
    }
}
