using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public Image textboxImage;
    public Button closeButton;

    [SerializeField]
    private InputReader inputReader;

    public void Start()
    {
        ClearDialogue();
    }
    public void UpdateDialogue(string newDialogue)
    {
        textboxImage.enabled = true;
        closeButton.enabled = true;
        dialogueText.text = newDialogue;
    }

    public void ClearDialogue()
    {
        dialogueText.text = "";
        textboxImage.enabled = false;
        closeButton.enabled = false;
        inputReader.EnableCookingWithWords();
    }
}
