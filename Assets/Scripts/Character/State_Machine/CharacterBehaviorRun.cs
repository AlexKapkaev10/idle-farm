using Scripts.Game;
using Scripts.Interfaces;
using Scripts.UI;
using UnityEngine;

namespace Scripts.StateMachine
{
    public sealed class CharacterBehaviorRun : ICharacterBehavior
    {
        private readonly ICharacterController _character = default;
        private readonly Joystick _joystick = default;

        private const string ANIMATION_KEY = "Run";
        private readonly float _runSpeed = default;
        
        public CharacterBehaviorRun(ICharacterController characterController, Joystick joystick, CharacterSettings characterSettings)
        {
            _character = characterController;
            _joystick = joystick;
            _runSpeed = characterSettings.RunSpeed;
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
            var direction = Vector3.forward * _joystick.Direction.y + Vector3.right * _joystick.Direction.x;

            if (direction != Vector3.zero)
            {
                var rotate = Quaternion.LookRotation(direction);
                _character.Rotate(rotate);
            }
            
            _character.Move(direction * (_runSpeed * Time.deltaTime));
        }
    }
}