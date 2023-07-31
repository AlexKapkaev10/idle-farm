using Scripts.Interfaces;
using Scripts.UI;
using UnityEngine;
using VContainer;

namespace Scripts.Game
{
    public class JoystickInputHandler : MonoBehaviour, IJoystickInputHandler
    {
        private Joystick _joystick;
        
        private IMoveControllable _moveControllable;
        private ICharacterController _characterController;
        private IGameUIController _gameUIController;

        [Inject]
        public void Construct(IObjectResolver resolver)
        {
            _gameUIController = resolver.Resolve<IGameUIController>();
            _characterController = resolver.Resolve<ICharacterController>();
            _gameUIController.OnJoystickCreate += SetJoystick;
        }

        private void SetJoystick(Joystick joystick)
        {
            _joystick = joystick;
            
            if (_joystick)
            {
                _joystick.OnPress += ChangeMoveState;
                _moveControllable ??= _characterController.GetGameObject().GetComponent<IMoveControllable>();
            }
            else
            {
                ChangeMoveState(false);
            }
        }
        
        private void OnDestroy()
        {
            _gameUIController.OnJoystickCreate -= SetJoystick;
            
            if (_joystick)
                _joystick.OnPress -= ChangeMoveState;
        }

        private void ChangeMoveState(bool isMove)
        {
            _moveControllable.ChangeMoveState(isMove);
        }
    }
}
