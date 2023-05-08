using Scripts.Interfaces;
using Scripts.UI;
using UnityEngine;

namespace Scripts.Game
{
    public class CharacterBehaviorRun : ICharacterBehavior
    {
        private readonly Character _character = null;
        private readonly Joystick _joystick = null;

        private Transform _bodyTransform = default;
        private Vector3 _joystickDirection = default;

        public CharacterBehaviorRun(Character character, Joystick joystick)
        {
            _character = character;
            _joystick = joystick;
        }
        public string AnimationKey { get; } = "Run";

        public void Enter()
        {
            _character.SetAnimationForMove(AnimationKey);
            _bodyTransform = _character.BodyTransform;
        }

        public void Exit()
        {
            _bodyTransform = null;
        }

        public void Update()
        {
            _joystickDirection = Vector3.forward * _joystick.Direction.y + Vector3.right * _joystick.Direction.x;
            var smoothDirection = Vector3.Lerp(_bodyTransform.localEulerAngles, _joystickDirection, Time.deltaTime);
                
            if (_joystickDirection != Vector3.zero)
                _bodyTransform.rotation = Quaternion.LookRotation(_joystickDirection);
        }

        public void FixedUpdate()
        {
            _character.CharacterController.Move(_joystickDirection * (_character.Speed * Time.fixedDeltaTime));
        }
    }
}