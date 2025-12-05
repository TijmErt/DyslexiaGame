using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class RepairSystem : MonoBehaviour
{
    public WordManager wordManager;
    public Customer customer;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI materialsText;
    private int score = 0;

    private List<SmithMaterialEnum> allMaterials = new List<SmithMaterialEnum>
    {
        SmithMaterialEnum.Iron,
        SmithMaterialEnum.Steel,
        SmithMaterialEnum.Gold,
        SmithMaterialEnum.Silver,
        SmithMaterialEnum.Bronze
    };

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
        }

        scoreText.text = "Voltooid: " + score;

        if (correct)
            Invoke(nameof(NextWord), 2f);
    }

    private void CompleteRepair()
    {
        // check before calling if allMaterils.Count > 0
        allMaterials.RemoveAt(allMaterials.Count - 1);
        allItems.Add(SmithItemEnum.Hammer); // Example item, replace with actual logic
    }

    private void EndRepairSession()
    {
        resultText.text = "Alle reparaties voltooid!";
        Debug.Log("Items repaired: " + allItems.Count);
        Debug.Log("Items: " + string.Join(", ", allItems));
    }

    private void NextWord()
    {
        wordManager.Next();
        resultText.text = "";
    }
}
