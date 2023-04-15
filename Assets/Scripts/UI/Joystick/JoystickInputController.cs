using Scripts.Interfaces;
using Scripts.UI;
using UnityEngine;
using Zenject;

namespace Scripts
{
    public class JoystickInputController : MonoBehaviour
    {
        [SerializeField] bool _isLocal;

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
            if (_isLocal)
            {
                _joystick = FindObjectOfType<UIController>().GetJoystick();
            }
        }

        private void Update()
        {
            _iContollable.Move(Vector3.forward * _joystick.Vertical + Vector3.right * _joystick.Horizontal);
        }
    }
}
