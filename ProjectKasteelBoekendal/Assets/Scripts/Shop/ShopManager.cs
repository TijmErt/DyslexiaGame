using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using Managers.Currency;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private Shopkeeper shopkeeper;
    [SerializeField] private List<ShopItems> shopItems;

    [SerializeField] private ShopSlot[] shopSlots;
    [SerializeField] private CurrencyMediator currencyMediator;
    [SerializeField] private ShopOverview overviewPanel;
    private ShopSlot selectedSlot;

    private void Start()
    {
        PopulateShopItems();
    }

    public void PopulateShopItems()
    {
        for (int i = 0; i < shopItems.Count && i < shopSlots.Length; i++)
        {
            ShopItems shopItem = shopItems[i];
            shopSlots[i].Initialize(shopItem.currencyData, shopItem.shopItemSO, shopItem.price);
            shopSlots[i].gameObject.SetActive(true);
        }

        for (int i = shopItems.Count; i < shopSlots.Length; i++)
        {
            shopSlots[i].gameObject.SetActive(false);
        }
    }

    public void TryBuyItem(CurrencyData currencyData, ShopItemSO shopItemSO, int price)
    {
        if (shopItemSO == null || currencyData == null || string.IsNullOrEmpty(currencyData.currencyID))
        {
            Debug.LogWarning("Shop buy failed: missing item or currency data.");
            return;
        }

        Debug.Log($"Trying to buy '{shopItemSO.shopItemName}' for {price} using currency '{currencyData.currencyID}'.");

        if (currencyMediator.HasEnoughCurrency(currencyData.currencyID, price))
        {
            currencyMediator.RemoveCurrency(currencyData.currencyID, price);
            Debug.Log($"Shop buy succeeded: '{shopItemSO.shopItemName}' bought for {price} {currencyData.currencyID}.");
            selectedSlot.MarkAsPurchased();
            shopkeeper.SaySuccess();
            //TODO: Add the item to the player's inventory
            return;
        }

        shopkeeper.SayNoMoney();
    }

    public void SelectShopItem(ShopSlot shopSlot)
    {
        selectedSlot = shopSlot;
        Debug.Log($"Shop slot selected: {shopSlot.shopItemSO?.shopItemName}");
        overviewPanel.ShowItem(shopSlot.shopItemSO, shopSlot.currencyData, shopSlot.Price);
        overviewPanel.SetBuyCallback(this);
    }

    public void BuySelectedItem()
    {
        if (selectedSlot == null)
        return;

        if (selectedSlot.Purchased)
        {
            shopkeeper.SayAlreadyBought();
            return;
        }

        TryBuyItem(selectedSlot.currencyData, selectedSlot.shopItemSO, selectedSlot.Price);

        Debug.Log($"Buy button clicked for selected item: {selectedSlot.shopItemSO?.shopItemName}");
        
    }
}

[System.Serializable]
public class ShopItems
{
    public ShopItemSO shopItemSO;
    public int price;
    public CurrencyData currencyData;
}
