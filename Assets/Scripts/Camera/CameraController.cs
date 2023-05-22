using System;
using Cinemachine;
using UnityEngine;
using Scripts.Interfaces;
using VContainer;

namespace Scripts.CameraGame
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CameraController : MonoBehaviour
    { 
        private CinemachineVirtualCamera _virtualCamera;
        private ICharacterController _characterController;

        [Inject]
        private void Construct(ICharacterController characterController)
        {
            _characterController = characterController;
        }

        private void Start()
        {
            _virtualCamera = GetComponent<CinemachineVirtualCamera>();
            _virtualCamera.Follow = _characterController.GetGameObject().transform;
            _virtualCamera.LookAt = _characterController.GetGameObject().transform;
        }
    }
}
