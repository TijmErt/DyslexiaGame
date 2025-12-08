using System.Collections.Generic;
using UnityEngine;

public class BlacksmithWordRepair : MonoBehaviour
{
    public float popupYOffset = 2f;
    [SerializeField] private DialogueSystem dialogueSystem;

    public void ShowMaterials(Dictionary<SmithMaterialEnum, int> allMaterials)
    {
        string materialsInfo = "Beschikbare materialen:\n";
        foreach (var material in allMaterials)
        {
            materialsInfo += $"{material.Key}: {material.Value}\n";
        }
        if (dialogueSystem != null)
            dialogueSystem.ShowMaterialsPopup(materialsInfo);
    }

    public void BlacksmithFeedback(string message)
    {
        if (dialogueSystem != null)
            dialogueSystem.ShowFeedback(message);
    }
}
