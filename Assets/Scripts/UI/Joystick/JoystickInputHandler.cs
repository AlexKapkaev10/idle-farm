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
            Initialization();
        }

        private void Initialization()
        {
            _joystick = _gameUIController.GetJoystick();
            _joystick.OnPress += ChangeMoveState;
            _moveControllable = _characterController.GetGameObject().GetComponent<IMoveControllable>();
        }
        
        private void OnDestroy()
        {
            _joystick.OnPress -= ChangeMoveState;
        }

        private void ChangeMoveState(bool value)
        {
            _moveControllable.ChangeMoveState(value);
        }
    }
}
