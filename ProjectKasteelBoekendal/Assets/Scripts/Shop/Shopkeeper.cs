using UnityEngine;

public class Shopkeeper : MonoBehaviour
{
    public CanvasGroup shopCanvasGroup;
    private PlayerMovement currentPlayer;
    private bool playerInRange;

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
}
