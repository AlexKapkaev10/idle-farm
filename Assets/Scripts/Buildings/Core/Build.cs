using Scripts.Interfaces;
using UnityEngine;

namespace Scripts.Buildings
{
    public class Build : MonoBehaviour
    {
        public BuildingType BuildingType;
        
        protected ICharacterController _characterController;

        public void SetTransform(Vector3 position, Vector3 rotation)
        {
            transform.position = position;
            transform.eulerAngles = rotation;
        }

        public void SetCharacterController(ICharacterController characterController)
        {
            _characterController = characterController;
        }
    }
}