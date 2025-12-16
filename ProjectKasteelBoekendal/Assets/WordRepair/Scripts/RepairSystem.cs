using System.Collections.Generic;
using System.Dynamic;
using TMPro;
using UnityEngine;

// Manages the repair gameplay loop: checking words, consuming materials,
// awarding items, and progressing to the next order or ending the session.
public class RepairSystem : MonoBehaviour
{
    // Reference to the WordManager that validates answers and advances words.
    public WordManager wordManager;

    // Reference to the current customer (used to trigger customer-related UI/actions).
    public Customer customer;

    // Reference to the blacksmith helper used to display materials and feedback.
    public BlacksmithWordRepair blacksmith;


    // Text element used to show the end-of-session popup text.
    [SerializeField] private TextMeshProUGUI endPopupText;

    // UI elements that should be disabled when the repair session ends.
    [SerializeField] private List<GameObject> uiToDisableOnEnd = new List<GameObject>();


    // Tracks how many of each material the player/blacksmith has available.
    private Dictionary<SmithMaterialEnum, int> allMaterials = new Dictionary<SmithMaterialEnum, int>
    {
        {SmithMaterialEnum.ijzer, 1},
        {SmithMaterialEnum.staal, 1},
        {SmithMaterialEnum.goud, 1}
    };

    // On start, show the current materials to the player via the blacksmith UI.
    private void Start()
    {
        blacksmith.ShowMaterials(allMaterials);
    }

    // Collected reward items from successful repairs.
    private List<SmithItemEnum> allItems = new List<SmithItemEnum>();

    // Called when the player submits an answer for the current word.
    public void CompleteWord()
    {
        // Ask the WordManager if the current assembled word is correct.
        bool correct = wordManager.CheckAnswer();

        if (correct)
        {
            // If correct, apply repair effects: consume material and add reward.
            CompleteRepair();
            // Notify the customer that their order completed.
            customer.CompleteOrder();

            string resultText = "Goed gedaan! Het woord is correct.";
            // Show positive feedback through the blacksmith UI.
            blacksmith.BlacksmithFeedback(resultText);
        }
        else
        {
            string resultText = "Fout! Probeer het opnieuw.";
            blacksmith.BlacksmithFeedback(resultText);
        }

        // Check remaining materials; if none left, end the repair session.
        List<SmithMaterialEnum> availableMaterials = GetAvailableMaterials();

        if (availableMaterials.Count <= 0)
        {
            Invoke(nameof(EndRepairSession), 2f);
            return;
        }

        // Otherwise progress to the next word after a short delay.
        Invoke(nameof(NextWord), 2f);
    }


    // Handles the consequences of a successful repair: consumes a material and
    // grants a reward item to the player's collection.
    private void CompleteRepair()
    {
        UseMaterial();

        AddRewardItem();
    }

    // Consume one random available material (reduces its count by one).
    private void UseMaterial()
    {
        // Find which materials still have at least one unit available.
        List<SmithMaterialEnum> availableMaterials = GetAvailableMaterials();

        // If we have available materials, pick a random one and decrement it.
        if (availableMaterials.Count > 0)
        {
            int randomIndex = Random.Range(0, availableMaterials.Count);
            SmithMaterialEnum randomMaterial = availableMaterials[randomIndex];
            allMaterials[randomMaterial] -= 1;
        }
    }

    // Add a random reward item to the player's collected items list.
    private void AddRewardItem()
    {
        // Get all possible enum values for SmithItemEnum.
        SmithItemEnum[] possibleItems = (SmithItemEnum[])System.Enum.GetValues(typeof(SmithItemEnum));

        if (possibleItems.Length == 0) return;

        // Pick a random item and add it to the collection.
        int randomIndex = Random.Range(0, possibleItems.Length);
        SmithItemEnum reward = possibleItems[randomIndex];
        allItems.Add(reward);
    }

    // Return a list of materials that currently have at least one unit available.
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

    // Called when no more materials are available: disables UI and shows summary popup.
    private void EndRepairSession()
    {

        // Disable configured UI elements to prevent further interaction.
        foreach (var ui in uiToDisableOnEnd)
        {
            if (ui == null)
                continue;

            ui.SetActive(false);
        }

        // Build the end-session message showing count and list of collected items.
        string resultText = "alle reparaties voltooid!";
        endPopupText.text = resultText + "\ngerepareerde voorwerpen:" + allItems.Count + "\nvoorwerpen:\n" + string.Join(", ", allItems);
        // Activate the parent of the text element to show the popup in UI.
        endPopupText.transform.parent.gameObject.SetActive(true);
    }

    // Advance to the next word and refresh the displayed material counts.
    private void NextWord()
    {
        wordManager.Next();
        blacksmith.ShowMaterials(allMaterials);
    }
}
