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
        
        private readonly float _rotateSpeed = default;
        
        public CharacterBehaviorRun(ICharacterController characterController, IGameUIController gameUIController, CharacterSettings characterSettings)
        {
            _character = characterController;
            _gameUIController = gameUIController;
            _rotateSpeed = characterSettings.RotateSpeed;
            
            _gameUIController.OnJoystickCreate += SetJoystick;
        }

        public void Enter()
        {
            _character.Animator.SetBool(AnimatorParameters.RunBool, true);
        }

        public void Exit()
        {
            
            _character.Animator.SetBool(AnimatorParameters.RunBool, false);
        }

        public void Update()
        {
            if (!_joystick)
                return;
            
            var direction = Vector3.forward * _joystick.Direction.y + Vector3.right * _joystick.Direction.x;

            if (direction != Vector3.zero)
            {
                var rotate = Quaternion.LookRotation(direction);
                var slerpRotate = Quaternion.Slerp(_character.GetBodyTransform().rotation, rotate, Time.deltaTime * _rotateSpeed);
                _character.Rotate(slerpRotate);
            }
            
            _character.Move(direction * (_character.RunSpeed * Time.deltaTime), direction.magnitude);
        }

        private void SetJoystick(Joystick joystick)
        {
            _joystick = joystick;
        }
    }
}