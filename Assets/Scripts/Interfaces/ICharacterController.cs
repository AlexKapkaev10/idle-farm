using Scripts.Enums;
using System;
using UnityEngine;

namespace Scripts.Interfaces
{
    public interface ICharacterController
    {
        public event Action OnMow;
        public void SetAnimationForField(FieldStateType fieldState);
        public Transform GetTransform();
        public void AddPlant(PlantType type, PlantBlock block);
        public void BuyPlants(PlantType type, Transform blocksTarget);
    }
}