using Scripts.Enums;
using Scripts.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Game
{
    public class ResourceController
    {
        public event Action<bool> OnFull;

        private readonly List<PlantBlock> _wheatBlock = new List<PlantBlock>();
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
            SetMoneyValueChange(0);
        }

        public List<PlantBlock> GetBlocksByType(PlantType plantType)
        {
            switch (plantType)
            {
                case PlantType.Wheat:
                    return _wheatBlock;
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
                SetMoneyValueChange(-value);
                return value;
            }
            
            return 0;
        }

        public void Add(PlantType type, PlantBlock block)
        {
            switch (type)
            {
                case PlantType.Wheat:
                    if (_wheatBlock.Count >= _maxBlocks)
                    {
                        OnFull?.Invoke(true);
                        break;
                    }
                    
                    _wheatBlock.Add(block);
                    if (_gameUI)
                        _gameUI.DisplayWheatCount(type, _wheatBlock.Count);
                    break;
            }
        }

        public void Buy(PlantType type)
        {
            switch (type)
            {
                case PlantType.Wheat:
                    SetMoneyValueChange(_wheatBlock.Count * _wheatFactor);

                    OnFull?.Invoke(false);
                    if (_gameUI)
                    {
                        _gameUI.DisplayByuPlants(_wheatBlock.Count, 0);
                    }

                    _wheatBlock.Clear();
                    _positionYOffset = 0f;
                    break;
            }
        }

        private void SetMoneyValueChange(int value)
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
                    var checkEven = (_wheatBlock.Count % 2) == 0;
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