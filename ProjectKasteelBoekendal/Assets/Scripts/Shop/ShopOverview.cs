using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Managers.Currency;

public class ShopOverview : MonoBehaviour
{
    // UI elements used to display the currently selected item.
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private Image currencyIconImage;

    // Button used to trade for the selected item.
    [SerializeField] private Button buyButton;

    // Reference to the ShopManager so this panel can notify it when the player presses the trade button.
    private ShopManager shopManager;

    private void Awake()
    {
        // Hide the overview panel until an item is selected.
        SetEmptyState();
    }

    private void Start()
    {
        // Register the button click event.
        if (buyButton != null)
        {
            buyButton.onClick.AddListener(OnBuyButtonClicked);
        }
    }

    /// <summary>
    /// Displays information about the selected item in the overview panel.
    /// Called by ShopManager when a ShopSlot is selected.
    /// </summary>
    public void ShowItem(ShopItemSO item, CurrencyData currency, int price)
    {
        if (item == null)
        {
            return;
        }

        // Make the panel visible once an item has been selected.
        gameObject.SetActive(true);

        // Display the item icon.
        if (itemImage != null)
        {
            itemImage.sprite = item.shopItemIcon;
            itemImage.enabled = item.shopItemIcon != null;
        }

        // Display the item name.
        if (itemNameText != null)
        {
            itemNameText.text = item.shopItemName;
        }

        // Display the item's trade cost.
        if (priceText != null)
        {
            priceText.text = price.ToString();
        }

        // Display the currency icon required for the trade.
        if (currencyIconImage != null && currency != null)
        {
            currencyIconImage.sprite = currency.icon;
            currencyIconImage.enabled = currency.icon != null;
        }
    }

    /// <summary>
    /// Clears all displayed item information and hides the panel.
    /// Used when no item is selected.
    /// </summary>
    private void SetEmptyState()
    {
        if (itemImage != null)
        {
            itemImage.sprite = null;
            itemImage.enabled = false;
        }

        if (itemNameText != null)
        {
            itemNameText.text = string.Empty;
        }

        if (priceText != null)
        {
            priceText.text = string.Empty;
        }

        if (currencyIconImage != null)
        {
            currencyIconImage.sprite = null;
            currencyIconImage.enabled = false;
        }

        // Hide the panel when there is no selected item.
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Stores a reference to the ShopManager.
    /// This allows the overview panel to trigger purchases/trades.
    /// </summary>
    public void SetBuyCallback(ShopManager manager)
    {
        shopManager = manager;
    }

    /// <summary>
    /// Called when the trade button is pressed.
    /// Passes the request to the ShopManager.
    /// </summary>
    private void OnBuyButtonClicked()
    {
        if (shopManager != null)
        {
            shopManager.BuySelectedItem();
        }
    }
}
