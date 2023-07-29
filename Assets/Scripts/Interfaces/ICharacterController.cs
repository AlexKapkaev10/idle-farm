﻿using Scripts.Enums;
using System;
using System.Collections.Generic;
using Scripts.Plants;
using UnityEngine;

namespace Scripts.Interfaces
{
    public interface ICharacterController
    {
        public event Action OnMow;
        public void Move(Vector3 velocity);
        public void Rotate(Quaternion rotation);
        public void SetAnimationForField(FieldStateType fieldState);
        public void SetAnimationForMove(string key);
        public Transform GetBodyTransform();
        public GameObject GetGameObject();
        public void AddPlant(in Plant block);
        public void BuyPlants(in List<PlantType> plants);
    }
}