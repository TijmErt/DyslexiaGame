using System;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem.OnScreen;
using Random = UnityEngine.Random;

public class Shopkeeper : MonoBehaviour
{
    // CanvasGroup used to show and hide the trading UI.
    public CanvasGroup shopCanvasGroup;

    // Reference to the player currently interacting with the shopkeeper.
    // Used to disable and re-enable movement while trading.
    private PlayerMovement currentPlayer;
    private GameObject joycon;
    private GameObject hudManager;

    // Text field used for shopkeeper dialogue.
    public TMP_Text dialogueText;

    // Sound for button
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip closeButtonSound;

    [Header("Dialogue Lines")]

    // Random greeting shown when the player starts trading.
    [TextArea] public string[] greetings =
    {
        "Hoi! Wil je ruilen?",
        "Ik heb leuke spullen om te ruilen.",
        "Zullen we ruilen?"
    };

    // Random dialogue shown after a successful trade.
    [TextArea] public string[] successLines =
    {
        "Dank je!",
        "Goed geruild!",
        "Dat is mooi!",
        "Fijne ruil!"
    };

    // Random dialogue shown when the player does not have enough currency.
    [TextArea] public string[] noMoneyLines =
    {
        "Je hebt te weinig.",
        "Dat kan nog niet."
    };

    // Random dialogue shown when the player tries to trade for an item
    // that has already been obtained.
    [TextArea] public string[] alreadyBoughtLines =
    {
        "Die heb je al.",
        "Dat heb je al.",
        "Je hebt dit al.",
        "Niet nog één."
    };

    private void Start()
    {
        joycon = GameObject.Find("Jocon_UI");
        hudManager = GameObject.Find("HUDCollection");
    }

    /// <summary>
    /// Opens the trading interface when a player enters the shopkeeper's trigger.
    /// Supports both Player1 and Player2.
    /// </summary>
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player1") || collision.CompareTag("Player2"))
        {
            currentPlayer =
                collision.GetComponent<PlayerMovement>() ??
                collision.GetComponentInParent<PlayerMovement>();
            
            OpenShop();
        }
    }

    /// <summary>
    /// Shows the trading UI and temporarily disables player movement.
    /// A random greeting is displayed each time the shop opens.
    /// </summary>
    private void OpenShop()
    {
        shopCanvasGroup.alpha = 1f;
        shopCanvasGroup.interactable = true;
        shopCanvasGroup.blocksRaycasts = true;

        if (currentPlayer != null)
        {
            currentPlayer.enabled = false;
        }

        if (joycon != null){
            joycon.SetActive(false);
        }

        if (hudManager != null)
        {
            hudManager.SetActive(false);
        }

        SayRandom(greetings);
    }

    /// <summary>
    /// Closes the trading UI and restores player movement.
    /// Called by the UI close button.
    /// </summary>
    public void CloseShop()
    {
        audioSource.PlayOneShot(closeButtonSound);
        shopCanvasGroup.alpha = 0f;
        shopCanvasGroup.interactable = false;
        shopCanvasGroup.blocksRaycasts = false;

        if (currentPlayer != null)
        {
            currentPlayer.enabled = true;
            currentPlayer = null;
        }
        if (joycon != null)
        {
            joycon.SetActive(true);
        }
        if (hudManager != null)
        {
            hudManager.SetActive(true);
        }
    }

    // Displays a random line from the provided dialogue category.
    public void SayRandom(string[] lines)
    {
        if (dialogueText == null || lines == null || lines.Length == 0)
            return;

        dialogueText.text = lines[Random.Range(0, lines.Length)];
    }

    // Displays positive dialogue after a successful trade.
    public void SaySuccess()
    {
        SayRandom(successLines);
    }

    // Displays dialogue when the player does not have enough currency.
    public void SayNoMoney()
    {
        SayRandom(noMoneyLines);
    }

    // Displays dialogue when the player tries to obtain an item that has already been traded for.
    public void SayAlreadyBought()
    {
        SayRandom(alreadyBoughtLines);
    }
}