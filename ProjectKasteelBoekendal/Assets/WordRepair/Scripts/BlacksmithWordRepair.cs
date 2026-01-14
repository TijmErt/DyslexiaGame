using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Helper component for the blacksmith UI.
/// Responsible for formatting and sending material lists and feedback
/// messages to the DialogueSystem for display.
/// </summary>
public class BlacksmithWordRepair : MonoBehaviour
{
    public float popupYOffset = 2f;
    [SerializeField] private DialogueSystem dialogueSystem;

    /// Formats and displays the available materials to the player.
    public void ShowMaterials(Dictionary<SmithMaterialEnum, int> allMaterials)
    {
        string materialsInfo = "Beschikbare materialen:\n";
        foreach (var material in allMaterials)
        {
            materialsInfo += $"{material.Key}: {material.Value}\n";
        }

        // Only attempt to show the popup if we have a dialogueSystem reference.
        if (dialogueSystem != null)
            dialogueSystem.ShowMaterialsPopup(materialsInfo);
    }

    /// Sends a short feedback message to the blacksmith dialogue UI.
    public void BlacksmithFeedback(string message)
    {
        if (dialogueSystem != null)
            dialogueSystem.ShowBlacksmithFeedback(message);
    }
}
