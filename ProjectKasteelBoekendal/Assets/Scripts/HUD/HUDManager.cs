using System;
using System.Collections.Generic;
using Managers.Currency;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class HUDManager : MonoBehaviour
{
    public Image playerInfoLabel;
    
    [SerializeField] private CurrencyMediator currencyMediator;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        //INSERT CODE: Change playername to player's name
        //INSERT CODE: Change coins to player's current/saved coins
        currencyMediator = FindFirstObjectByType<CurrencyMediator>();
        

    }

    private void Start()
    {
        if (currencyMediator == null)
        {
            Debug.LogError("CurrencyMediator is not assigned!");
            return;
        }

        currencyMediator.OnCurrencyChanged += UpdateCoinsUI; Debug.Log("Subscribed to OnCurrencyChanged event");
    }

    private void OnEnable()
    {
        if (currencyMediator == null)
        {
            Debug.LogError("CurrencyMediator is not assigned!");
            return;
        }

        currencyMediator.OnCurrencyChanged += UpdateCoinsUI; Debug.Log("Subscribed to OnCurrencyChanged event");
    }
    private void OnDisable()
    {
        if(currencyMediator != null) currencyMediator.OnCurrencyChanged -= UpdateCoinsUI; Debug.Log("UnSubscribed to OnCurrencyChanged event");
    }

    public void UpdateCoinsUI(string currencyID ,int currentAmount)
    {
        Debug.Log(currencyID + " : " + currentAmount);
        //Add featuure to update specific Coin shown, if the coin is shown
        string coinAmountText = currentAmount.ToString();
        playerInfoLabel.transform.Find("coinsamount").GetComponent<TMP_Text>().text = coinAmountText;
    }

    public void LoadPauseMenu()
    {
        //INSERT CODE: Load pause menu
    }

    public void LoadVirtualCoach()
    {
        //INSERT CODE: Load virtual coach
    }
}
