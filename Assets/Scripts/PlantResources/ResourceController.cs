using Scripts.Enums;
using Scripts.UI;
using System;
using System.Collections.Generic;
using Scripts.Plants;
using Scripts.Resources;
using VContainer;

namespace Scripts.Game
{
    public sealed class ResourceController : IResourceController
    {
        private readonly Bank _bank;
        private readonly List<Plant> _plants = new List<Plant>();
        private readonly GameUI _gameUI;
        private readonly BankSettings _bankSettings;

        [Inject]
        public ResourceController(GameUI gameUI, Bank bank, BankSettings bankSettings)
        {
            _gameUI = gameUI;
            _gameUI.SetResourceController(this);
            _bank = bank;
            _bankSettings = bankSettings;
            _bank.OnMoneyChange += DisplayMoney;
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

        public void Add(Plant plant)
        {
            _plants.Add(plant);
            _gameUI.DisplayPlantCount(plant, _plants.Count);
        }

        public void Buy(in List<PlantType> plantTypes)
        {
            foreach (var type in plantTypes)
            {
                var availablePlants = new List<Plant>();
                
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
                    return;
                
                _plants.RemoveAll(item => item == null);
                _bank.MoneyValueChange(availablePlants.Count * _bankSettings.GetResourcePriceByPlantType(type));
                _gameUI.DisplayByuPlants(availablePlants.Count, 0);
            }
        }

        private void DisplayMoney(int from, int to)
        {
            _gameUI.DisplayMoneyCount(from, to);
        }
    }
}