using System;
using System.Collections.Generic;
using Scripts.Enums;
using UnityEngine;

namespace Scripts
{
    [CreateAssetMenu(fileName = "BankSettings", menuName = "SO/Bank_Settings", order = 1)]
    public sealed class BankSettings : ScriptableObject
    {
        [SerializeField]
        private List<ResourcePrice> _resourcePrices;

        public int GetResourcePriceByPlantType(PlantType plantType)
        {
            foreach (var resourcePrice in _resourcePrices)
            {
                if (resourcePrice.PlantType == plantType)
                {
                    return resourcePrice.Price;
                }
            }

            return 0;
        }
    }

    [Serializable]
    public struct ResourcePrice
    {
        public int Price;
        public PlantType PlantType;
    }
}