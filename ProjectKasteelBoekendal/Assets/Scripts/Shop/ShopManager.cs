using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using Managers.Currency;

public class ShopManager : MonoBehaviour
{
    // Reference to the shopkeeper NPC.
    // Used to display dialogue based on player actions.
    [SerializeField] private Shopkeeper shopkeeper;

    // List of all items available in this shop.
    [SerializeField] private List<ShopItems> shopItems;

    // UI slots used to display shop items.
    [SerializeField] private ShopSlot[] shopSlots;

    // Handles checking and removing currency.
    [SerializeField] private CurrencyMediator currencyMediator;

    // Panel that displays detailed information about the selected item.
    [SerializeField] private ShopOverview overviewPanel;

    // Currently selected shop slot.
    private ShopSlot selectedSlot;

    private void Start()
    {
        // Populate the shop UI when the scene starts.
        PopulateShopItems();
    }

    /// <summary>
    /// Fills all available shop slots with item data.
    /// Unused slots are hidden.
    /// </summary>
    public void PopulateShopItems()
    {
        for (int i = 0; i < shopItems.Count && i < shopSlots.Length; i++)
        {
            ShopItems shopItem = shopItems[i];

            shopSlots[i].Initialize(
                shopItem.currencyData,
                shopItem.shopItemSO,
                shopItem.price);

            shopSlots[i].gameObject.SetActive(true);
        }

        // Hide any remaining slots that do not contain an item.
        for (int i = shopItems.Count; i < shopSlots.Length; i++)
        {
            shopSlots[i].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Attempts to trade for the selected item.
    /// Checks whether the player has enough currency before completing the trade.
    /// </summary>
    public void TryBuyItem(CurrencyData currencyData, ShopItemSO shopItemSO, int price)
    {
        // Prevent errors caused by missing data.
        if (shopItemSO == null || currencyData == null || string.IsNullOrEmpty(currencyData.currencyID))
        {
            Debug.LogWarning("Shop buy failed: missing item or currency data.");
            return;
        }

        Debug.Log($"Trying to buy '{shopItemSO.shopItemName}' for {price} using currency '{currencyData.currencyID}'.");

        // Trade succeeds if the player has enough currency.
        if (currencyMediator.HasEnoughCurrency(currencyData.currencyID, price))
        {
            currencyMediator.RemoveCurrency(currencyData.currencyID, price);

            // Prevent the item from being traded again.
            selectedSlot.MarkAsPurchased();

            // Shopkeeper reacts positively to the successful trade.
            shopkeeper.SaySuccess();

            // TODO: Add the traded item to the player's inventory.
            return;
        }

        // Shopkeeper reacts when the player does not have enough currency.
        shopkeeper.SayNoMoney();
    }

    /// <summary>
    /// Called when a shop slot is selected.
    /// Updates the overview panel with information about the selected item.
    /// </summary>
    public void SelectShopItem(ShopSlot shopSlot)
    {
        selectedSlot = shopSlot;

        overviewPanel.ShowItem(
            shopSlot.shopItemSO,
            shopSlot.currencyData,
            shopSlot.Price);

        overviewPanel.SetBuyCallback(this);
    }

    /// <summary>
    /// Attempts to trade for the currently selected item.
    /// Called when the trade button is pressed.
    /// </summary>
    public void BuySelectedItem()
    {
        // No item selected.
        if (selectedSlot == null)
            return;

        // Prevent already traded items from being traded again.
        if (selectedSlot.Purchased)
        {
            shopkeeper.SayAlreadyBought();
            return;
        }

        TryBuyItem(
            selectedSlot.currencyData,
            selectedSlot.shopItemSO,
            selectedSlot.Price);

        Debug.Log($"Buy button clicked for selected item: {selectedSlot.shopItemSO?.shopItemName}");
    }
}

/// <summary>
/// Represents an item that can appear in the shop.
/// Contains the item data, required currency and trade cost.
/// </summary>
[System.Serializable]
public class ShopItems
{
    public ShopItemSO shopItemSO;
    public int price;
    public CurrencyData currencyData;
}