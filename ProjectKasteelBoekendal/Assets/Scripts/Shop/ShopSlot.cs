using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Managers.Currency;
using UnityEngine.EventSystems;

public class ShopSlot : MonoBehaviour, IPointerClickHandler
{
    // Currency required to trade for this item.
    public CurrencyData currencyData;

    // ScriptableObject containing the item's name and icon.
    public ShopItemSO shopItemSO;

    // UI references for displaying item information.
    public TMP_Text priceText;
    public Image shopItemImage;
    public Image currencyIconImage;

    // Reference to the ShopManager that handles item selection and trading.
    [SerializeField] private ShopManager shopManager;

    // Audio source for playing button sounds
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip buttonSound;

    // Tracks whether this item has already been traded for.
    // Once purchased, the slot becomes unavailable.
    private bool purchased;

    // Read-only access to the purchased state for other scripts.
    public bool Purchased => purchased;

    // Cost of this item.
    private int price;

    // Read-only access to the item's price.
    public int Price => price;

    /// <summary>
    /// Populates the slot with item data when the shop is created.
    /// Called by ShopManager.
    /// </summary>
    public void Initialize(CurrencyData currencyData, ShopItemSO shopItemSO, int price)
    {
        this.currencyData = currencyData;
        this.shopItemSO = shopItemSO;

        // Display the item icon if one exists.
        if (shopItemImage != null)
        {
            shopItemImage.sprite = shopItemSO != null ? shopItemSO.shopItemIcon : null;
            shopItemImage.enabled = shopItemSO != null && shopItemSO.shopItemIcon != null;
        }

        // Display the currency icon if one exists.
        if (currencyIconImage != null)
        {
            currencyIconImage.sprite = currencyData != null ? currencyData.icon : null;
            currencyIconImage.enabled = currencyData != null && currencyData.icon != null;
        }

        this.price = price;

        // Display the item's cost.
        if (priceText != null)
        {
            priceText.text = price.ToString();
        }
    }

    /// <summary>
    /// Called when the player clicks/taps this slot.
    /// Selects the item in the shop overview panel.
    /// Purchased items cannot be selected again.
    /// </summary>
    public void OnSlotClicked()
    {
        // Ignore clicks on items that have already been purchased.
        if (purchased)
            return;

        if (shopManager != null)
        {
            shopManager.SelectShopItem(this);
        }
        audioSource.PlayOneShot(buttonSound);
    }

    /// <summary>
    /// Unity UI click handler.
    /// Redirects pointer clicks to the slot selection logic.
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        OnSlotClicked();
    }

    /// <summary>
    /// Marks this item as purchased.
    /// The slot is visually greyed out and can no longer be selected.
    /// Called by ShopManager after a successful trade.
    /// </summary>
    public void MarkAsPurchased()
    {
        purchased = true;

        // Grey out the slot so the player can see it is no longer available.
        var image = GetComponent<Image>();

        if (image != null)
        {
            image.color = Color.gray;
        }
    }
}
