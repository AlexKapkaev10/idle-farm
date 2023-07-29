using System.Collections.Generic;
using Scripts.Enums;
using Scripts.Interfaces;
using UnityEngine;

namespace Scripts.Buildings
{
    public class Build : MonoBehaviour
    {
        protected ICharacterController _characterController;

        public List<PlantType> PlantTypes { get; set; }

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