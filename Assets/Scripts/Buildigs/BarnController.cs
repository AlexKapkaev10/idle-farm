using Scripts.Enums;
using Scripts.Interfaces;
using UnityEngine;
using VContainer;

namespace Scripts
{
    public class BarnController : MonoBehaviour
    {
        [SerializeField] private PlantType _plantTypeBarn;
        private ICharacterController _characterController;

        [Inject]
        private void Inject(ICharacterController characterController)
        {
            _characterController = characterController;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == _characterController.GetGameObject())
            {
                _characterController.BuyPlants(_plantTypeBarn, transform);
            }
        }
    }
}
