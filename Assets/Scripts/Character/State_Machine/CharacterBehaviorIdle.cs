using Scripts.Game;
using Scripts.Interfaces;
using VContainer;

namespace Scripts.StateMachine
{
    public sealed class CharacterBehaviorIdle : ICharacterBehavior
    {
        private readonly ICharacterController _character = default;

        [Inject]
        public CharacterBehaviorIdle(ICharacterController characterController)
        {
            _character = characterController;
        }

        public void Enter()
        {
            _character.Animator.SetBool(AnimatorParameters.IdleBool, true);
        }

        public void Exit()
        {
            
            _character.Animator.SetBool(AnimatorParameters.IdleBool, false);
        }

        public void Update()
        {;
        }
    }
}