using UnityEngine;

public class Customer : MonoBehaviour
{
    public float popupYOffset = 2f;
    public float orderPopupYOffset;
    public float orderPopupXOffset;
    public DialogueSystem dialogueSystem;
    public Sprite orderImage;

    public void CompleteOrder()
    {
        if (dialogueSystem != null)
            dialogueSystem.ShowCustomerFeedbackPopup("Dank je wel!");
    }

    public void NewOrder()
    {
        if (dialogueSystem != null)
        {
            dialogueSystem.ShowOrderPopup(orderImage);
        }
    }
}
