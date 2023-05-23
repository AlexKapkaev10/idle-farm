using Scripts.Enums;
using Scripts.UI;
using System;
using System.Collections.Generic;
using Scripts.Plants;
using Scripts.Resources;
using UnityEngine;

namespace Scripts.Game
{
    public class ResourceController
    {
        private readonly Bank _bank;
        private readonly List<Plant> _plants = new List<Plant>();
        private readonly GameUI _gameUI;
        private readonly BankSettings _bankSettings;

        private float _positionYOffset = 0f;
        private int _positionZCount = 0;
        

        public ResourceController(GameUI gameUI, Bank bank, BankSettings bankSettings)
        {
            _gameUI = gameUI;
            _gameUI.SetResourceController(this);
            _bank = bank;
            _bankSettings = bankSettings;
            _bank.Init(gameUI);
        }

        public List<Plant> GetBlocksByType(PlantType plantType)
        {
            switch (plantType)
            {
                case PlantType.Wheat:
                    return _plants;
            }

            return null;
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
            _gameUI.DisplayWheatCount(plant.PlantType, _plants.Count);
        }

        public void Buy(PlantType type)
        {
            var plantsByTypeCount = 0;

            for (int i = 0; i < _plants.Count; i++)
            {
                var plant = _plants[i];

                if (plant.PlantType == type)
                {
                    plantsByTypeCount++;
                    _plants[i] = null;
                }
            }

            if (plantsByTypeCount > 0)
            {
                _plants.RemoveAll(item => item == null);
                _bank.MoneyValueChange(plantsByTypeCount * _bankSettings.GetResourcePriceByPlantType(type));
                _gameUI?.DisplayByuPlants(plantsByTypeCount, 0);
            }
        }

        private Vector3 CalculateBlockPosition(PlantType type)
        {
            switch (type)
            {
                case PlantType.Wheat:
                    float zPosition = 0f;
                    switch (_positionZCount)
                    {
                        case 0:
                        case 1:
                            zPosition = 0f;
                            break;
                        case 2:
                        case 3:
                            zPosition = -0.15f;
                            break;
                    }
                    var checkEven = (_plants.Count % 2) == 0;
                    var value = new Vector3(checkEven ? 0.2f : -0.2f, _positionYOffset, zPosition);
                    _positionZCount++;
                    if (_positionZCount == 4)
                    {
                        _positionZCount = 0;
                        _positionYOffset += 0.15f;
                    }
                    return value;
                default:
                    return Vector2.zero;
            }
        }
    }
}