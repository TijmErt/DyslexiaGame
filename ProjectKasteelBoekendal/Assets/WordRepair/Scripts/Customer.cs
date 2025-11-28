using UnityEngine;

public class Customer : MonoBehaviour
{
    public float popupYOffset = 2f;
    public float orderPopupYOffset;
    public float orderPopupXOffset;
    public DialogueSystem dialogueSystem;
    public Sprite orderImage;

    public void ProgressOrder()
    {
        if (dialogueSystem != null)
            dialogueSystem.ShowFeedbackPopup(gameObject, "Thank you!", popupYOffset);
    }

    public void NewOrder()
    {
        if (dialogueSystem != null)
        {
            dialogueSystem.ShowOrderPopup(gameObject, orderImage, orderPopupXOffset, orderPopupYOffset);
        }
    }
}
