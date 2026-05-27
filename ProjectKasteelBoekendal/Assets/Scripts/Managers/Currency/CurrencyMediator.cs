using Managers.Currency;
using UnityEngine;

/// <summary>
/// The CurrencyMediator acts as a middle layer between other scripts/UI systems
/// and the CurrencyManager.
///
/// This prevents direct dependency on the CurrencyManager singleton instance,
/// reducing the chance of null reference errors if the manager has not yet
/// been initialized or is temporarily unavailable.
///
/// It also provides UI-friendly wrapper methods that can easily be connected
/// to Unity Events through the Inspector.
/// </summary>
public class CurrencyMediator : MonoBehaviour
{
    [SerializeField] private string currentCurrencyID = "";

    #region UI Methods
    /*
    
     Methods intended for Unity UI interaction.
    
     These methods simplify usage with Unity Events by allowing buttons,
     sliders, and other UI components to interact with currencies without
     needing direct access to the CurrencyManager.
    
     Most methods use the currently selected currency ID stored inside
     the mediator.
    
    */
    public void UIChangeCurrencyID(string newCurrencyID)
    {
        currentCurrencyID = newCurrencyID;
    }
    public void UIAddCurrency(int amount)
    {
        AddCurrency(currentCurrencyID, amount);
    }
    public void UIRemoveCurrency(int amount)
    {
        RemoveCurrency(currentCurrencyID, amount);
    }
    #endregion
    
    #region Standard Methods
    /*
     
     Standard wrapper methods for interacting with the CurrencyManager.
    
     These methods provide a safer and more centralized way for gameplay
     systems to modify or retrieve currency data without directly using
     the CurrencyManager singleton.
    
    */
    public void AddCurrency(string id, int amount)
    {
        CurrencyManager.instance.AddCurrency( id,  amount);
    }

    public void RemoveCurrency(string id, int amount)
    {
        CurrencyManager.instance.RemoveCurrency( id,  amount);
    }

    public int GetCurrencyAmount(string id)
    {
        return CurrencyManager.instance.GetCurrencyAmount(id);
    }
    public bool HasEnoughCurrency(string id, int amount)
    {
        return CurrencyManager.instance.HasEnoughCurrency(id, amount);
    }
    
    public CurrencyEntry GetCurrency(string id)
    {
        return CurrencyManager.instance.GetCurrency(id);
    } 
    
    #endregion
}
