using Scripts.Interfaces;
using Scripts.UI;
using UnityEngine;

namespace Scripts.Game
{
    public class JoystickInputController : MonoBehaviour
    {
        private Joystick _joystick;
        private IControllable _iControllable;

        public void Init(Joystick joystick)
        {
            _joystick = joystick;
            _joystick.OnDown += StartMove;
            _joystick.OnUp += StopMove;
            
            _iControllable = GetComponent<IControllable>();
        }
        
        private void OnDestroy()
        {
            _joystick.OnDown -= StartMove;
            _joystick.OnUp -= StopMove;
        }

        private void StartMove()
        {
            _iControllable.StartMove();
        }

        private void StopMove()
        {
            _iControllable.StopMove();
        }
    }
}
