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
        private readonly IBank _bank;
        private readonly List<Plant> _plants = new List<Plant>();
        private readonly IGameUIController _gameUIController;
        private readonly BankSettings _bankSettings;

        [Inject]
        public ResourceController(IGameUIController gameUIController, IBank bank, BankSettings bankSettings)
        {
            _gameUIController = gameUIController;
            //_gameUIController.SetResourceController(this);
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
            //_gameUIController.DisplayPlantCount(plant, _plants.Count);
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
                //_gameUIController.DisplayByuPlants(availablePlants.Count, 0);
            }
        }

        private void DisplayMoney(int from, int to)
        {
            //_gameUIController.DisplayMoneyCount(from, to);
        }
    }
}