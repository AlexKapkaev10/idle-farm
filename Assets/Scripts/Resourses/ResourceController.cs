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
        private readonly Bank _bank = new Bank();
        private readonly List<Plant> _plants = new List<Plant>();
        private readonly GameUI _gameUI;
        
        private int _money = 0;
        private readonly int _maxBlocks = 40;
        private readonly int _wheatFactor = 10; 
        
        private float _positionYOffset = 0f;
        private int _positionZCount = 0;

        private const string SaveMoneyKey = "money";

        public ResourceController(GameUI gameUI)
        {
            _gameUI = gameUI;
            _gameUI.SetResourceController(this);
            _money = PlayerPrefs.GetInt(SaveMoneyKey, 0);
            MoneyValueChange(0);
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

        public int TryGetMoney(int value, Action callBackFalse)
        {
            if (value > _money)
            {
                callBackFalse?.Invoke();
            }
            else
            {
                MoneyValueChange(-value);
                return value;
            }
            
            return 0;
        }

        public void Add(Plant plant)
        {
            _plants.Add(plant);
            _gameUI.DisplayWheatCount(plant.PlantType, _plants.Count);
        }

        public void Buy(PlantType type)
        {
            MoneyValueChange(_plants.Count);
            
            switch (type)
            {
                case PlantType.Wheat:

                    if (_gameUI)
                    {
                        _gameUI.DisplayByuPlants(_plants.Count, 0);
                    }

                    _plants.Clear();
                    _positionYOffset = 0f;
                    break;
            }
        }

        private void MoneyValueChange(int value)
        {
            Debug.Log($"Change money value {value}");
            PlayerPrefs.SetInt(SaveMoneyKey, _money);
            _money += value;
            _gameUI.DisplayMoneyCount(_money);
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