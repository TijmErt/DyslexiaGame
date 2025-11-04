using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Customer : MonoBehaviour
{
    public int done = 100;
    public int currentProgress;
    public int popupYOffset = 2;
    public DialogueSystem dialogueSystem;

    void Start()
    {
        currentProgress = 0;
    }

    public void ProgressOrder(int amount)
    {
        currentProgress += amount;
        
        if (dialogueSystem != null)
            dialogueSystem.ShowFeedbackPopup(gameObject, "Thank you!", popupYOffset);
    }
}
