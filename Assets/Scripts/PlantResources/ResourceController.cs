using Scripts.Enums;
using System;
using System.Collections.Generic;
using Scripts.Plants;
using Scripts.Resources;
using UnityEngine;
using VContainer;

namespace Scripts.Game
{
    public sealed class ResourceController : IResourceController
    {
        public event Action<PlantBlock, int> OnAddPlant;
        public event Action<BuyResourceData> OnBuyPlants;
        public event Action<PlantType> OnResourceComplete;
        public event Action<bool> QuestComplete;

        private readonly IBank _bank;
        
        private readonly List<PlantBlock> _plants = new List<PlantBlock>();
        private Dictionary<PlantType, int> _questMap;
        
        private readonly BankSettings _bankSettings;
        private BuyResourceData _buyResourceData;
        
        private bool isQuestComplete;

        public int Money => _bank.Money;

        [Inject]
        public ResourceController(IBank bank, BankSettings bankSettings)
        {
            _bank = bank;
            _bankSettings = bankSettings;
            _buyResourceData = new BuyResourceData();
        }

        public void SetQuestMap(in Dictionary<PlantType, int> questMap)
        {
            _plants.Clear();
            _buyResourceData.BuyResources = null;
            _buyResourceData.oldMoneyValue = 0;
            _buyResourceData.newMoneyValue = 0;
            
            isQuestComplete = false;

            if (_questMap != null)
            {
                _questMap.Clear();
                _questMap = null;
            }

            _questMap = questMap;
        }

        public void CalculateQuestComplete(in SowingField completeField)
        {
            if (_questMap[completeField.PlantType] <= 0 || isQuestComplete)
                return;
            
            var calculatedCount = _questMap[completeField.PlantType] -= completeField.Count;
            if (calculatedCount <= 0)
                OnResourceComplete?.Invoke(completeField.PlantType);

            foreach (var map in _questMap)
            {
                if (map.Value > 0)
                {
                    return;
                }
            }

            isQuestComplete = true;
            QuestComplete?.Invoke(isQuestComplete);
        }

        public void TryGetMoney(int value, Action<bool> callBack)
        {
            var isEnough = _bank.IsEnough(value);
            
            if (isEnough)
            {
                _bank.MoneyValueChange(-value);
            }
            
            callBack?.Invoke(isEnough);
        }

        public void Add(PlantBlock plantBlock)
        {
            _plants.Add(plantBlock);
            OnAddPlant?.Invoke(plantBlock, GetPlantsCountByType(plantBlock.PlantType));
        }

        public void Buy(in List<PlantType> plantTypes)
        {
            if (_plants.Count == 0 || !isQuestComplete)
            {
                QuestComplete?.Invoke(isQuestComplete);
                return;
            }
            
            var totalMoney = 0;
            var buyResources = new BuyResource[plantTypes.Count];

            for (var i = 0; i < plantTypes.Count; i++)
            {
                var type = plantTypes[i];
                buyResources[i].PlantType = type;
                buyResources[i].Count = GetPlantsCountByType(type);
                totalMoney += GetPlantsCountByType(type) * _bankSettings.GetResourcePriceByPlantType(type);
            }
            
            _bank.MoneyValueChange(totalMoney);
            var oldMoney = Money - totalMoney;

            _buyResourceData.BuyResources = buyResources;
            _buyResourceData.oldMoneyValue = oldMoney;
            _buyResourceData.newMoneyValue = Money;
            
            OnBuyPlants?.Invoke(_buyResourceData);
        }

        private int GetPlantsCountByType(PlantType type)
        {
            var count = 0;
            foreach (var plant in _plants)
            {
                if (plant.PlantType == type)
                    count++;
            }

            return count;
        }
    }

    public class BuyResourceData
    {
        public BuyResource[] BuyResources;
        public int oldMoneyValue;
        public int newMoneyValue;
    }

    public struct BuyResource
    {
        public PlantType PlantType;
        public int Count;
    }
}