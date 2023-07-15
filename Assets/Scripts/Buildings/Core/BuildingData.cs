using System;
using UnityEngine;

namespace Scripts.Buildings
{
    [Serializable]
    public class BuildingData
    {
        public Build Prefab;
        public Vector3 Position = default;
        public Vector3 Rotation;
    }
}