using Scripts.Enums;
using UnityEngine;

namespace Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "SowingData", menuName = "Sowing Data", order = 51)]
    public class SowingData : ScriptableObject
    {
        [Header("Wheat Setting")]
        [SerializeField]
        private float _cellInteractDistance = 1.2f;
        [SerializeField]
        private float _wheatRipeningTime = 11f;
        [SerializeField]
        private GameObject _wheatPlant;
        [SerializeField]
        private SowingCell _wehathCell;
        [SerializeField]
        private Material _defaultWheatMaterial;
        [SerializeField]
        private Material _wheatSowMaterial;
        [SerializeField]
        private Material _wheatRipeMaterial;
        [SerializeField]
        private PlantBlock _wheatBlock;

        public PlantBlock GetBlockByPlantType(PlantType type)
        {
            switch (type)
            {
                case PlantType.Wheat:
                    return _wheatBlock;
                default:
                    return null;
            }
        }

        public GameObject GetPlantByPlantType(PlantType type)
        {
            switch (type)
            {
                case PlantType.Wheat:
                    return _wheatPlant;
                default:
                    return null;
            }
        }

        public Material GetDefaultMaterialByPlantType(PlantType type)
        {
            switch (type)
            {
                case PlantType.Wheat:
                    return _defaultWheatMaterial;
                default:
                    return null;
            }
        }

        public Material GetSowMaterialByPlantType(PlantType type)
        {
            switch (type)
            {
                case PlantType.Wheat:
                    return _wheatSowMaterial;
                default:
                    return null;
            }
        }

        public Material GetRipeMaterialByPlantType(PlantType type)
        {
            switch (type)
            {
                case PlantType.Wheat:
                    return _wheatRipeMaterial;
                default:
                    return null;
            }
        }

        public float GetCellInteractDistance()
        {
            return _cellInteractDistance;
        }

        public float GetRepeningTimeByPlantType(PlantType type)
        {
            switch (type)
            {
                case PlantType.Wheat:
                    return _wheatRipeningTime;
                default:
                    return 0;
            }
        }

        public SowingCell GetSowCellByPlantType(PlantType type)
        {
            switch (type)
            {
                case PlantType.Wheat:
                    return _wehathCell;
                default:
                    return null;
            }
        }
    }
}
