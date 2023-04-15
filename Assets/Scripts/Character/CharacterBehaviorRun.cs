using Scripts.Interfaces;
using UnityEngine;

namespace Scripts
{
    public class CharacterBehaviorRun : ICharacterBehavior
    {
        public Character _character;

        public CharacterBehaviorRun(Character character)
        {
            _character = character;
        }

        public void Enter()
        {
            _character.SetAnimationForMove("Run");
        }

        public void Exit()
        {
            Debug.Log("Exit to Run");
        }

        public void Update()
        {
            Debug.Log("Update Run");
        }
    }
}