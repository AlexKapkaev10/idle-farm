using System.Collections.Generic;
using Scripts.Enums;
using Scripts.Plants;
using UnityEngine;

namespace Scripts.Resources
{
    public class BlockPositionCalculator
    {
        private float _positionYOffset = 0f;
        private int _positionZCount = 0;

        private Vector3 CalculateBlockPosition(PlantType type, List<PlantBlock> plants)
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
                    var checkEven = (plants.Count % 2) == 0;
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