using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Managers.Currency;

public class ShopOverview : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private Image currencyIconImage;
    [SerializeField] private Button buyButton;

    private ShopManager shopManager;

    private void Awake()
    {
        SetEmptyState();
    }

    private void Start()
    {
        if (buyButton != null)
        {
            buyButton.onClick.AddListener(OnBuyButtonClicked);
        }
    }

    public void ShowItem(ShopItemSO item, CurrencyData currency, int price)
    {
        if (item == null)
        {
            return;
        }

        gameObject.SetActive(true);

        if (itemImage != null)
        {
            itemImage.sprite = item.shopItemIcon;
            itemImage.enabled = item.shopItemIcon != null;
        }

        if (itemNameText != null)
        {
            itemNameText.text = item.shopItemName;
        }

        if (priceText != null)
        {
            priceText.text = price.ToString();
        }

        if (currencyIconImage != null && currency != null)
        {
            currencyIconImage.sprite = currency.icon;
            currencyIconImage.enabled = currency.icon != null;
        }
    }

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

        gameObject.SetActive(false);
    }

    public void SetBuyCallback(ShopManager manager)
    {
        shopManager = manager;
    }

    private void OnBuyButtonClicked()
    {
        if (shopManager != null)
        {
            shopManager.BuySelectedItem();
        }
    }
}
