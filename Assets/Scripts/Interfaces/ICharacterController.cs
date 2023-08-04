using Scripts.Enums;
using System;
using System.Collections.Generic;
using Scripts.Plants;
using UnityEngine;

namespace Scripts.Interfaces
{
    public interface ICharacterController
    {
        public event Action<int> OnMow;
        public Animator Animator { get; }
        public ITool CurrentTool { get; }
        public void StartLevel();
        public void EndLevel(bool isWin);
        public void Move(Vector3 velocity, float magnitude);
        public void Rotate(Quaternion rotation);
        public void ChangeMoveState(bool isRun);
        public void SetAnimationForField(FieldStateType fieldState);
        public void AddPlant(in PlantBlock block);
        public void BuyPlants(in List<PlantType> plants);
        public void SetTransform(Vector3 position, Vector3 bodyRotation);
        public Transform GetBodyTransform();
        public GameObject GetGameObject();
    }
}