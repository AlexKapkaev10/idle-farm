using Cinemachine;
using UnityEngine;
using Zenject;
using Scripts.Game;

namespace Scripts.CameraGame
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CameraController : MonoBehaviour
    {
        private CinemachineVirtualCamera _virtualCamera;

        [Inject]
        private void Construct(Character character)
        {
            _virtualCamera = GetComponent<CinemachineVirtualCamera>();

            _virtualCamera.Follow = character.transform;
            _virtualCamera.LookAt = character.transform;
        }
    }
}
