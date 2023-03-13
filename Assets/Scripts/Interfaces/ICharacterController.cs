using Scripts.Enums;
using System;
using UnityEngine;

namespace Scripts.Interfaces
{
    public interface ICharacterController
    {
        public event Action OnMow;
        public void SetAnimationState(FieldStateType fuildState);
        public Transform GetTransform();
        public void SetPlant(PlantType type, PlantBlock block);
        public void BuyPlants(PlantType type, Transform blocksTarget);
    }
}