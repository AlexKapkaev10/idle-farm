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
        private readonly int _maxBlocks = 40;
        
        private float _positionYOffset = 0f;
        private int _positionZCount = 0;

        public ResourceController(GameUI gameUI)
        {
            _gameUI = gameUI;
        }

        public void Add(PlantType type, PlantBlock block, Transform target, PlantCollectType collectType)
        {
            switch (type)
            {
                case PlantType.Wheat:
                    _wheatBlock.Add(block);

                    if (_gameUI)
                        _gameUI.DisplayWheatCount(type, _wheatBlock.Count);

                    block.MoveToTarget(
                        target, 
                        collectType == PlantCollectType.InBag ? CalculateBlockPosition(type) : Vector3.zero, 
                        0.5f,
                        true,
                        collectType == PlantCollectType.InCharacter);

                    if (_wheatBlock.Count >= _maxBlocks)
                    {
                        OnFull?.Invoke(true);
                    }
                    break;
            }
        }

        public void Buy(PlantType type, Transform target)
        {
            switch (type)
            {
                case PlantType.Wheat:
                    _wheatBlock.Reverse();
                    foreach (var block in _wheatBlock)
                    {
                        block.MoveToTarget(target, Vector2.zero, 1f, false);
                    }
                    OnFull?.Invoke(false);
                    if (_gameUI)
                        _gameUI.DisplayByuPlants(_wheatBlock.Count, 0);

                    _wheatBlock.Clear();
                    _positionYOffset = 0f;
                    break;
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