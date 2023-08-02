using System.Collections;
using UnityEngine;

namespace Scripts.Buildings
{
    public class Hangar : Build
    {
        [SerializeField] private Collider _collider;
        
        private IEnumerator Start()
        {
            yield return null;
            _collider.enabled = true;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == _characterController.GetGameObject())
            {
                _characterController.BuyPlants(PlantTypes);
            }
        }
    }
}
