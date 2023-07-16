using System;
using System.Collections.Generic;
using Scripts.Enums;
using UnityEngine;

namespace Scripts.Buildings
{
    [Serializable]
    public class BuildingData
    {
        public List<PlantType> PlantTypes = new List<PlantType>();
        public Build Prefab;
        public Vector3 Position = default;
        public Vector3 Rotation;
    }
}