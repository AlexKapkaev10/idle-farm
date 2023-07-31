using Scripts.Game;
using Scripts.Interfaces;
using Scripts.UI;
using UnityEngine;

namespace Scripts.StateMachine
{
    public sealed class CharacterBehaviorRun : ICharacterBehavior
    {
        private readonly ICharacterController _character = default;
        private readonly IGameUIController _gameUIController;
        
        private Joystick _joystick = default;

        private const string ANIMATION_KEY = "Run";
        private readonly float _runSpeed = default;
        
        public CharacterBehaviorRun(ICharacterController characterController, IGameUIController gameUIController, CharacterSettings characterSettings)
        {
            _character = characterController;
            _gameUIController = gameUIController;
            _runSpeed = characterSettings.RunSpeed;
            _gameUIController.OnJoystickCreate += SetJoystick;
        }

        public void Enter()
        {
            _character.SetAnimationForMove(ANIMATION_KEY);
        }

        public void Exit()
        {
            
        }

        public void Update()
        {
            if (!_joystick)
                return;
            
            var direction = Vector3.forward * _joystick.Direction.y + Vector3.right * _joystick.Direction.x;
            if (direction != Vector3.zero)
            {
                var rotate = Quaternion.LookRotation(direction);
                _character.Rotate(rotate);
            }
            
            _character.Move(direction * (_runSpeed * Time.deltaTime));
        }

        private void SetJoystick(Joystick joystick)
        {
            _joystick = joystick;
        }
    }
}