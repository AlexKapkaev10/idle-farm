using System.Collections.Generic;
using Scripts.Enums;
using UnityEngine;

namespace Scripts.Buildings
{
    public class Hangar : Build
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == _characterController.GetGameObject())
            {
                _characterController.BuyPlants(PlantTypes);
            }
        }
    }
}
