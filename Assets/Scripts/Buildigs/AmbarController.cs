using Scripts.Enums;
using Scripts.Interfaces;
using UnityEngine;

namespace Scripts
{
    public class AmbarController : MonoBehaviour
    {
        private ICharacterController _iCharacterController;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Character"))
            {
                if (other.gameObject.TryGetComponent<ICharacterController>(out _iCharacterController))
                {
                    _iCharacterController.BuyPlants(PlantType.Wheat, transform);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Character"))
            {
                if (_iCharacterController != null)
                {
                    _iCharacterController = null;
                }
            }
        }
    }
}
