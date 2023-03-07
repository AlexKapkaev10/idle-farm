using Scripts.Interfaces;
using UnityEngine;

namespace Scripts
{
    public class JoystickInputController : MonoBehaviour
    {
        [SerializeField]
        private Joystick _joystick;
        private IContollable _iContollable;

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
