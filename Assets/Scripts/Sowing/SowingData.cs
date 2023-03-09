using Scripts.Enums;
using UnityEngine;

namespace Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "SowingData", menuName = "Sowing Data", order = 51)]
    public class SowingData : ScriptableObject
    {
        [SerializeField]
        private float wheatRipeningTime = 11f;
        [SerializeField]
        private SowingCell _sowingCell;
        [SerializeField]
        private Material _wheatSowingMaterial;
        [SerializeField]
        private Material _wheatRipeMaterial;

        public Material GetSowingMaterialBySowingType(SowingType type)
        {
            switch (type)
            {
                case SowingType.Wheat:
                    return _wheatSowingMaterial;
                default:
                    return null;
            }
        }

        public Material GetRipeMaterialBySowingType(SowingType type)
        {
            switch (type)
            {
                case SowingType.Wheat:
                    return _wheatRipeMaterial;
                default:
                    return null;
            }
        }

        public float GetRepeningTimeBySowingType(SowingType type)
        {
            float value = 0;
            switch (type)
            {
                case SowingType.Wheat:
                    value = wheatRipeningTime;
                    break;
            }

            return value;
        }

        public SowingCell GetSowingCellBySwowimgType(SowingType type)
        {
            switch (type)
            {
                case SowingType.Wheat:
                    return _sowingCell;
                default:
                    return null;
            }
        }
    }
}
