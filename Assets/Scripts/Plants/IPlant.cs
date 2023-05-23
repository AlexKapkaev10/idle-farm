using System;
using Scripts.Enums;
using UnityEngine;

namespace Scripts.Plants
{
    public interface IPlant
    {
        public event Action<IPlant> OnBlockReturn;
        public PlantType PlantType { get; }
        public Transform BlockTransform { get; }
        public MonoBehaviour GetMonoBehaviour { get; }
        public void MoveToTarget(Transform parent, float duration, bool isCharacterTarget);
    }
}
