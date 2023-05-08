using Scripts.Interfaces;

namespace Scripts.Game
{
    public class CharacterBehaviorIdle : ICharacterBehavior
    {
        private readonly Character _character;
        
        public string AnimationKey { get; } = "Idle";

        public CharacterBehaviorIdle(Character character)
        {
            _character = character;
        }

        public void Enter()
        {
            _character.SetAnimationForMove(AnimationKey);
        }

        public void Exit()
        {
        }

        public void Update()
        {;
        }

        public void FixedUpdate()
        {
        }
    }
}