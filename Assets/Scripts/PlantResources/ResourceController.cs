using Scripts.Enums;
using Scripts.UI;
using System;
using System.Collections.Generic;
using Scripts.Plants;
using Scripts.Resources;
using UnityEngine;
using UnityEngine.Assertions.Must;
using VContainer;

namespace Scripts.Game
{
    public sealed class ResourceController : IResourceController
    {
        public event Action<PlantBlock, int> OnAddPlant;
        public event Action<int, int> OnChangeMoney;
        public event Action<PlantType, int> OnBuyPlants;
        
        private readonly IBank _bank;
        private readonly List<PlantBlock> _plants = new List<PlantBlock>();
        private readonly BankSettings _bankSettings;

        public int Money => _bank.Money;

        [Inject]
        public ResourceController(IBank bank, BankSettings bankSettings)
        {
            _bank = bank;
            _bankSettings = bankSettings;
            _bank.OnMoneyChange += ChangeMoney;
            _bank.Init();
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
            if (_plants.Count <= 0)
                return;
            
            var totalMoney = 0;
            foreach (var type in plantTypes)
            {
                var availablePlants = new List<PlantBlock>();
                
                for (int i = 0; i < _plants.Count; i++)
                {
                    var plant = _plants[i];
                    if (plant.PlantType == type)
                    {
                        availablePlants.Add(plant);
                        _plants[i] = null;
                    }
                }
                
                if (availablePlants.Count <= 0)
                    continue;
                
                _plants.RemoveAll(item => item == null);
                OnBuyPlants?.Invoke(type, availablePlants.Count);
                totalMoney += availablePlants.Count * _bankSettings.GetResourcePriceByPlantType(type);
            }
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
}