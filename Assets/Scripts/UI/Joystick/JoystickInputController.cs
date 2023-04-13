using Scripts.Interfaces;
using Scripts.UI;
using UnityEngine;
using Zenject;

namespace Scripts
{
    public class JoystickInputController : MonoBehaviour
    {
        private Joystick _joystick;
        private IContollable _iContollable;

        [Inject]
        private void Construct(UIController uIController)
        {
            _joystick = uIController.GetJoystick();
        }

        private void Awake()
        {
            _iContollable = GetComponent<IContollable>();
        }

        private void Update()
        {
            _iContollable.Move(Vector3.forward * _joystick.Vertical + Vector3.right * _joystick.Horizontal);
        }
    }
}
