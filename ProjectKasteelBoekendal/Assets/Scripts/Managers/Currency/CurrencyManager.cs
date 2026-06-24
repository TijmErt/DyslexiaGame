using System;
using System.Collections.Generic;
using Managers.Saving;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Managers.Currency
{
   [Serializable]
   public class CurrencyEntry
   {
      public CurrencyData currencyData;
      public int amount;
      
   }
   public class CurrencyManager : MonoBehaviour, ISaveable
   {
      public string UID => "CurrencyManager";

      public static CurrencyManager instance; 
      
      [SerializeField] private List<CurrencyEntry> currencies;


      private void Awake()
      {
         if (instance != null)
         {
            Destroy(gameObject);
            return;
         }
        
         instance = this;
         DontDestroyOnLoad(gameObject);
      }
      
      
      public void AddCurrency(string id, int amount)
      {
            CurrencyEntry entry = GetCurrency(id);

            if (entry == null)
            {
                Debug.LogWarning($"Currency with ID '{id}' not found.");
                return;
            }

            entry.amount += amount;
      }
      
      public void RemoveCurrency(string id, int amount)
      {
         CurrencyEntry entry = GetCurrency(id);

         if (entry == null)
         {
            Debug.LogWarning($"Currency with ID '{id}' not found.");
            return;
         }

         entry.amount -= amount;

         // Prevent negative currency
         if (entry.amount < 0)
            entry.amount = 0;
      }

      public int GetCurrencyAmount(string id)
      {
         CurrencyEntry entry = GetCurrency(id);
         return entry?.amount ?? 0; //returns amount '0' if entry doesn't exist or matches give id
      }

      public CurrencyEntry GetCurrency(string id)
      {
         return currencies.Find(entry =>
            entry.currencyData != null &&
            entry.currencyData.currencyID == id);
      }

      #region Saving

      public bool HasEnoughCurrency(string id, int amount)
      {
         return GetCurrencyAmount(id) >= amount;
      }
      
      public object CaptureState()
      {
         CurrencySaveWrapper saveData = new CurrencySaveWrapper{
            Currencies = new List<CurrencySaveData>()
         };

         foreach (CurrencyEntry entry in currencies)
         {
            if (entry.currencyData == null)
               continue;

            saveData.Currencies.Add(new CurrencySaveData
            {
               CurrencyID = entry.currencyData.currencyID,
               Amount = entry.amount
            }); 
         }

         return saveData;
      }

      public void RestoreState(string state)
      {
         
         Debug.Log("Attempting Restore Currencies");
         
         foreach (CurrencyEntry entry in currencies)
         {
            entry.amount = 0;
         }
         List<CurrencySaveData> saveData = JsonUtility.FromJson<CurrencySaveWrapper>(state).Currencies;
         
         foreach (CurrencySaveData data in saveData)
         {
            CurrencyEntry entry = GetCurrency(data.CurrencyID);

            if (entry != null)
            {
               entry.amount = data.Amount;
               Debug.Log( data.CurrencyID + " Restored, Value : " + data.Amount);
            }
         }
      }

      [Serializable]
      private class CurrencySaveData
      {
         public string CurrencyID;
         public int Amount;
      }
      private struct CurrencySaveWrapper
      {
         public List<CurrencySaveData> Currencies;
      }
      #endregion

   }
}
