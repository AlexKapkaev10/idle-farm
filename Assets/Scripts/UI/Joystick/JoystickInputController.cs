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

            _joystick.OnDown += StartMove;
            _joystick.OnUp += StopMove;
        }

        private void OnDestroy()
        {
            _joystick.OnDown -= StartMove;
            _joystick.OnUp -= StopMove;
        }

        private void StartMove()
        {
            _iContollable.StartMove();
        }

        private void StopMove()
        {
            _iContollable.StopMove();
        }

        private void Update()
        {
            _iContollable.UpdateMove(Vector3.forward * _joystick.Vertical + Vector3.right * _joystick.Horizontal);
        }
    }
}
