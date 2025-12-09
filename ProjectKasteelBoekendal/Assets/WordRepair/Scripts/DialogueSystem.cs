using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System.Collections;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] private GameObject customerFeedbackPopup;
    [SerializeField] private GameObject orderPopup;
    [SerializeField] private Transform popupParent;
    [SerializeField] private float popupDuration = 2f;
    [SerializeField] private GameObject materialsPopup;
    [SerializeField] private GameObject blacksmithFeedbackPopup;


    // Customer

    public void ShowCustomerFeedbackPopup(string message)
    {
        if (customerFeedbackPopup == null)
        {
            Debug.LogWarning("DialogueSystem: customerFeedbackPopup is not assigned.");
            return;
        }

        HidePopup(orderPopup);

        // update text inside the popup
        var tmp = customerFeedbackPopup.GetComponentInChildren<TextMeshProUGUI>();
        if (tmp != null) tmp.text = message;

        ShowPopup(customerFeedbackPopup);

        StartCoroutine(HidePopupAfterDelay(customerFeedbackPopup, popupDuration));
    }

    public void ShowOrderPopup(Sprite image)
    {
        if (orderPopup == null)
        {
            Debug.LogWarning("DialogueSystem: orderPopup is not assigned.");
            return;
        }

        HidePopup(customerFeedbackPopup);

        // Find the specific Image inside the instantiated popup (child named "OrderImage")
        Image img = null;
        Transform child = orderPopup.transform.Find("Image");
        if (child != null) img = child.GetComponent<Image>();

        // fallback to first Image in children if specific child not found
        if (img == null) img = orderPopup.GetComponentInChildren<Image>();

        if (img != null)
            img.sprite = image;
        else
            Debug.LogWarning("DialogueSystem: no Image found on order popup instance to set sprite.");

        ShowPopup(orderPopup);
    }

    // Blacksmith

    public void ShowMaterialsPopup(string message)
    {
        if (materialsPopup == null) return;

        HidePopup(blacksmithFeedbackPopup);

        materialsPopup.GetComponentInChildren<TextMeshProUGUI>().text = message;

        ShowPopup(materialsPopup);
    }

    public void ShowBlacksmithFeedback(string message)
    {
        if (blacksmithFeedbackPopup == null) return;

        Debug.Log("Showing blacksmith feedback: " + message);
        HidePopup(materialsPopup);

        // update text inside the popup
        var tmp = blacksmithFeedbackPopup.GetComponentInChildren<TextMeshProUGUI>();
        if (tmp != null) tmp.text = message;

        ShowPopup(blacksmithFeedbackPopup);

        StartCoroutine(HidePopupAfterDelay(blacksmithFeedbackPopup, popupDuration));
    }

    private void HidePopup(GameObject dialoguePopup)
    {
        if (dialoguePopup != null)
        {
            Debug.Log("Hiding popup: " + dialoguePopup.name);
            dialoguePopup.SetActive(false);
        }
    }

    private IEnumerator HidePopupAfterDelay(GameObject dialoguePopup, float delay)
    {
        yield return new WaitForSeconds(delay);
        HidePopup(dialoguePopup);
    }

    private void ShowPopup(GameObject dialoguePopup)
    {
        if (dialoguePopup != null)
        {
            dialoguePopup.SetActive(true);
        }
    }

    private IEnumerator ShowPopupAfterDelay(GameObject dialoguePopup, float delay)
    {
        yield return new WaitForSeconds(delay);
        ShowPopup(dialoguePopup);
    }
}
