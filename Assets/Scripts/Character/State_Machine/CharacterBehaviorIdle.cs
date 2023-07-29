using Scripts.Interfaces;
using UnityEngine;
using VContainer;

namespace Scripts.StateMachine
{
    public sealed class CharacterBehaviorIdle : ICharacterBehavior
    {
        private readonly ICharacterController _character = default;
        private const string ANIMATION_KEY = "Idle";

        [Inject]
        public CharacterBehaviorIdle(ICharacterController characterController)
        {
            _character = characterController;
        }

        public void Enter()
        {
            _character.SetAnimationForMove(ANIMATION_KEY);
        }

        public void Exit()
        {
        }

        public void Update()
        {;
        }
    }
}