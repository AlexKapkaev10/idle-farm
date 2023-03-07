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

        public SowingCell GetSowingCell()
        {
            if (_sowingCell)
            {
                return _sowingCell;
            }

            return null;
        }
    }
}
