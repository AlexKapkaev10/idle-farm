using Scripts.Plants;
using UnityEngine;

namespace Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "SowingData", menuName = "SO/Sowing Data")]
    public class SowingData : ScriptableObject
    {
        [SerializeField] private SowingCell _sowingCell;
        [SerializeField] private PlantBlock _plantBlock;
        [SerializeField] private GameObject _wheatPlant;
        
        [SerializeField] private Material _sowMaterial;
        [SerializeField] private Material _ripeMaterial;
        
        [SerializeField] private float _cellInteractDistance = 1.2f;
        [SerializeField] private float _wheatRipeningTime = 11f;

        public SowingCell GetSowCell()
        {
            return _sowingCell;
        }

        public PlantBlock GetPlantBlock()
        {
            return _plantBlock;
        }

        public GameObject GetPlant()
        {
            return _wheatPlant;
        }
        
        public Material GetSowMaterial()
        {
            return _sowMaterial;
        }

        public Material GetRipeMaterial()
        {
            return _ripeMaterial;
        }

        public float GetCellInteractDistance()
        {
            return _cellInteractDistance;
        }

        public float GetRipeningTime()
        {
            return _wheatRipeningTime;
        }
    }
}
