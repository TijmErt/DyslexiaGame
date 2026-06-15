using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using Managers.Currency;
using UnityEngine.EventSystems;

public class ShopSlot : MonoBehaviour, IPointerClickHandler
{
    public CurrencyData currencyData;
    public ShopItemSO shopItemSO;
    public TMP_Text priceText;
    public Image shopItemImage;
    public Image currencyIconImage;
    [SerializeField] private ShopManager shopManager;
    private bool purchased;
    public bool Purchased => purchased;
    private int price;
    public int Price => price;

    public void Initialize(CurrencyData currencyData, ShopItemSO shopItemSO, int price)
    {
        // Fill the slot with item and currency info.
        this.currencyData = currencyData;
        this.shopItemSO = shopItemSO;
        if (shopItemImage != null)
        {
            shopItemImage.sprite = shopItemSO != null ? shopItemSO.shopItemIcon : null;
            shopItemImage.enabled = shopItemSO != null && shopItemSO.shopItemIcon != null;
        }
        if (currencyIconImage != null)
        {
            currencyIconImage.sprite = currencyData != null ? currencyData.icon : null;
            currencyIconImage.enabled = currencyData != null && currencyData.icon != null;
        }
        this.price = price;
        if (priceText != null)
        {
            priceText.text = price.ToString();
        }
    }

    public void OnSlotClicked()
    {
        if (purchased)
        return;
        
        if (shopManager != null)
        {
            shopManager.SelectShopItem(this);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnSlotClicked();
    }

    public void MarkAsPurchased()
    {
        purchased = true;

        var image = GetComponent<Image>();
        if (image != null)
        {
            image.color = Color.gray;
        }
}
}
