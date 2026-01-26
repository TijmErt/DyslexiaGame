using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System.Collections;

// Manages small UI popups used by in-game interactions (customers, orders, blacksmith).
public class DialogueSystem : MonoBehaviour
{
    [SerializeField] private GameObject customerFeedbackPopup;
    [SerializeField] private GameObject orderPopup;
    [SerializeField] private float popupDuration = 2f;
    [SerializeField] private GameObject materialsPopup;
    [SerializeField] private GameObject blacksmithFeedbackPopup;

    public void Start()
    {
        // Ensure all feedback popups are hidden at the start.
        HidePopup(customerFeedbackPopup);
        HidePopup(blacksmithFeedbackPopup);
    }

    // Customer

    // Show a short feedback popup for the customer (e.g., "Dank je wel!").
    // The method checks the assignment, updates the TextMeshProUGUI inside the
    // popup, displays it, and starts a coroutine to hide it after `popupDuration`.
    public void ShowCustomerFeedbackPopup(string message)
    {
        if (customerFeedbackPopup == null)
        {
            Debug.LogWarning("DialogueSystem: customerFeedbackPopup is not assigned.");
            return;
        }

        // If an order popup is currently visible, hide it when showing feedback.
        HidePopup(orderPopup);

        // Update the text element found inside the popup (uses TextMeshProUGUI).
        var tmp = customerFeedbackPopup.GetComponentInChildren<TextMeshProUGUI>();
        if (tmp != null) tmp.text = message;

        ShowPopup(customerFeedbackPopup);

        StartCoroutine(HidePopupAfterDelay(customerFeedbackPopup, popupDuration));
    }

    // Show an order popup showing the requested item image.
    // Looks for an Image component inside the `orderPopup` and assigns the provided sprite.
    public void ShowOrderPopup(Sprite image)
    {
        if (orderPopup == null)
        {
            Debug.LogWarning("DialogueSystem: orderPopup is not assigned.");
            return;
        }

        // Hide any customer feedback while showing the order popup.
        HidePopup(customerFeedbackPopup);

        // Try to find a child named "Image" first and get its Image component.
        Image img = null;
        Transform child = orderPopup.transform.Find("Image");
        if (child != null) img = child.GetComponent<Image>();

        // If not found by name, fall back to the first Image in children.
        if (img == null) img = orderPopup.GetComponentInChildren<Image>();

        // If an Image component was found, set its sprite; otherwise log a warning.
        if (img != null)
            img.sprite = image;
        else
            Debug.LogWarning("DialogueSystem: no Image found on order popup instance to set sprite.");

        ShowPopup(orderPopup);
    }

    // Blacksmith

    // Display the materials popup with the provided message text.
    // This assumes the popup contains a TextMeshProUGUI text element to receive the string.
    public void ShowMaterialsPopup(string message)
    {
        if (materialsPopup == null) return;

        HidePopup(blacksmithFeedbackPopup);

        // Update the text element and show the popup.
        materialsPopup.GetComponentInChildren<TextMeshProUGUI>().text = message;

        ShowPopup(materialsPopup);
    }

    // Display a transient blacksmith feedback message and hide other blacksmith-related popups.
    // The message is shown and then automatically hidden after `popupDuration`.
    public void ShowBlacksmithFeedback(string message)
    {
        if (blacksmithFeedbackPopup == null) return;

        Debug.Log("Showing blacksmith feedback: " + message);
        HidePopup(materialsPopup);

        // Update the text element inside the blacksmith feedback popup.
        var tmp = blacksmithFeedbackPopup.GetComponentInChildren<TextMeshProUGUI>();
        if (tmp != null) tmp.text = message;

        ShowPopup(blacksmithFeedbackPopup);

        StartCoroutine(HidePopupAfterDelay(blacksmithFeedbackPopup, popupDuration));
    }

    // --- Utility helpers used by multiple popup methods ---

    // Deactivates a popup GameObject if it's not null.
    private void HidePopup(GameObject dialoguePopup)
    {
        if (dialoguePopup != null)
        {
            Debug.Log("Hiding popup: " + dialoguePopup.name);
            dialoguePopup.SetActive(false);
        }
    }

    // Coroutine that waits for `delay` seconds then hides the provided popup.
    private IEnumerator HidePopupAfterDelay(GameObject dialoguePopup, float delay)
    {
        yield return new WaitForSeconds(delay);
        HidePopup(dialoguePopup);
    }

    // Activates a popup GameObject if it's not null.
    private void ShowPopup(GameObject dialoguePopup)
    {
        if (dialoguePopup != null)
        {
            dialoguePopup.SetActive(true);
        }
    }
}
