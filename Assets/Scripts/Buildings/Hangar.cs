using Scripts.Enums;
using UnityEngine;

namespace Scripts.Buildings
{
    public class Hangar : Build
    {
        [SerializeField] private PlantType _plantTypeBarn;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == _characterController.GetGameObject())
            {
                _characterController.BuyPlants(_plantTypeBarn, transform);
            }
        }
    }
}
