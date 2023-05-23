using System;
using Scripts.Enums;
using UnityEngine;

namespace Scripts
{
    [CreateAssetMenu(fileName = "BankSettings", menuName = "Bank_Settings", order = 1)]
    public class BankSettings : ScriptableObject
    {
        
    }

    [Serializable]
    public readonly struct ResourceCost
    {
        public readonly int Price;
        public readonly PlantType PlantType;
    }
}