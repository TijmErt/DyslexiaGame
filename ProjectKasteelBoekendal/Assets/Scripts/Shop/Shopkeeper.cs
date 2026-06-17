using UnityEngine;
using TMPro;

public class Shopkeeper : MonoBehaviour
{
    public CanvasGroup shopCanvasGroup;
    private PlayerMovement currentPlayer;
    private bool playerInRange;
    public TMP_Text dialogueText;

    [Header("Dialogue Lines")]
    [TextArea] public string[] greetings =
    {
        "Hoi! Wil je ruilen?",
        "Ik heb leuke spullen om te ruilen.",
        "Zullen we ruilen?"
    };

    [TextArea] public string[] successLines =
    {
        "Dank je!",
        "Goed geruild!",
        "Dat is mooi!",
        "Fijne ruil!"
    };

    [TextArea] public string[] noMoneyLines =
    {
        "Je hebt te weinig.",
        "Dat kan nog niet."
    };

    [TextArea] public string[] alreadyBoughtLines =
    {
        "Die heb je al.",
        "Dat heb je al.",
        "Je hebt dit al.",
        "Niet nog één."
    };

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player1") || collision.CompareTag("Player2"))
        {
            currentPlayer = collision.GetComponent<PlayerMovement>() ?? collision.GetComponentInParent<PlayerMovement>();

            OpenShop();
        }
    }

    private void OpenShop()
    {
        shopCanvasGroup.alpha = 1f; // Show the shop UI
        shopCanvasGroup.interactable = true; // Allow interaction with the shop UI
        shopCanvasGroup.blocksRaycasts = true; // Enable raycast blocking for the shop UI

        if (currentPlayer != null)
        {
            currentPlayer.enabled = false; // Disable player movement while the shop is open
        }

        SayRandom(greetings);

    }

    public void CloseShop(){
        shopCanvasGroup.alpha = 0f; // Hide the shop UI
        shopCanvasGroup.interactable = false; // Disable interaction with the shop UI
        shopCanvasGroup.blocksRaycasts = false; // Disable raycast blocking for the shop UI

        if (currentPlayer != null)
        {
            currentPlayer.enabled = true; // Re-enable player movement when the shop is closed
            currentPlayer = null; // Clear the reference to the current player
        }
    }

    // -------------------------
    // Dialogue system
    // -------------------------

    public void SayRandom(string[] lines)
    {
        if (dialogueText == null || lines == null || lines.Length == 0)
            return;

        dialogueText.text = lines[Random.Range(0, lines.Length)];
    }

    public void SaySuccess()
    {
        SayRandom(successLines);
    }

    public void SayNoMoney()
    {
        SayRandom(noMoneyLines);
    }

    public void SayAlreadyBought()
    {
        SayRandom(alreadyBoughtLines);
    }
}
