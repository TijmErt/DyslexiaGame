using UnityEngine;

namespace Managers.Currency
{
    [CreateAssetMenu(fileName = "CurrencyData", menuName = "Scriptable Objects/Managers/CurrencyData")]
    public class CurrencyData : ScriptableObject
    {
        public string currencyID;
        public string displayName;
        public Sprite icon;
    }
}
