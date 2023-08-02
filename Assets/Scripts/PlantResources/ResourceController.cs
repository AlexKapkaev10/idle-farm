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
        public event Action<int, int> OnChangeMoney;
        public event Action<BuyResource[]> OnBuyPlants;
        public event Action QuestNotComplete;
        public event Action<PlantType> OnResourceComplete;

        private readonly IBank _bank;
        private readonly List<PlantBlock> _plants = new List<PlantBlock>();
        private readonly BankSettings _bankSettings;
        private Dictionary<PlantType, int> _questMap;
        private bool isQuestComplete;

        public int Money => _bank.Money;

        [Inject]
        public ResourceController(IBank bank, BankSettings bankSettings)
        {
            _bank = bank;
            _bankSettings = bankSettings;
            _bank.OnMoneyChange += ChangeMoney;
        }

        public void SetQuestMap(in Dictionary<PlantType, int> questMap)
        {
            isQuestComplete = false;
            _plants.Clear();
            
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
            Debug.Log("questComplete");
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
                QuestNotComplete?.Invoke();
                return;
            }
            
            var totalMoney = 0;
            var buyResources = new BuyResource[plantTypes.Count];

            for (var i = 0; i < plantTypes.Count; i++)
            {
                var type = plantTypes[i];
                var availablePlants = 0;
                for (int a = 0; a < _plants.Count; a++)
                {
                    var plant = _plants[a];
                    if (plant.PlantType == type)
                    {
                        availablePlants++;
                        _plants[a] = null;
                    }
                }
                buyResources[i].PlantType = type;
                buyResources[i].Count = availablePlants;
                
                _plants.RemoveAll(item => item == null);
                totalMoney += availablePlants * _bankSettings.GetResourcePriceByPlantType(type);
            }
            OnBuyPlants?.Invoke(buyResources);
            _bank.MoneyValueChange(totalMoney);
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

        private void ChangeMoney(int from, int to)
        {
            OnChangeMoney?.Invoke(from, to);
        }
    }

    public struct BuyResource
    {
        public PlantType PlantType;
        public int Count;
    }
}