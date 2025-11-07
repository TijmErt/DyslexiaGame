using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Customer : MonoBehaviour
{
    public int done = 100;
    public int popupYOffset = 2;
    public DialogueSystem dialogueSystem;

    public void ProgressOrder()
    {
        if (dialogueSystem != null)
            dialogueSystem.ShowFeedbackPopup(gameObject, "Thank you!", popupYOffset);
    }
}
