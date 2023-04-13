using Scripts.Enums;
using Scripts.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class ResourceController
    {
        public event Action<bool> OnFull;

        private UIController _uIController;
        private int _maxBlocks = 40;
        private float _positionYOffset = 0f;
        private int _positionZCount = 0;
        private List<PlantBlock> _wheatBlock = new List<PlantBlock>();

        public ResourceController(UIController uIController)
        {
            _uIController = uIController;
        }

        public void Add(PlantType type, PlantBlock block, Transform target)
        {
            switch (type)
            {
                case PlantType.Wheat:
                    _wheatBlock.Add(block);
                    _uIController.DisplayWheatCount(type, _wheatBlock.Count);
                    block.MoveToTarget(target, BlockPosition(type), 0.5f, true);
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
                    for (int i = 0; i < _wheatBlock.Count; i++)
                    {
                        _wheatBlock[i].MoveToTarget(target, Vector2.zero, 1f, false);
                    }
                    OnFull?.Invoke(false);
                    _uIController.DisplayByuPlants(_wheatBlock.Count, 0);
                    _wheatBlock.Clear();
                    _positionYOffset = 0f;
                    break;
                default:
                    break;
            }
        }

        private Vector3 BlockPosition(PlantType type)
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
                    bool checkEven = (_wheatBlock.Count % 2) == 0;
                    Vector3 value = new Vector3(checkEven ? 0.2f : -0.2f, _positionYOffset, zPosition);
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