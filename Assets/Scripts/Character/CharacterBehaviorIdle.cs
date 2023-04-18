using System.Collections;
using UnityEngine;
using Scripts.Interfaces;

namespace Scripts.Game
{
    public class CharacterBehaviorIdle : ICharacterBehavior
    {
        public Character _character;

        public CharacterBehaviorIdle(Character character)
        {
            _character = character;
        }

        public void Enter()
        {
            _character.SetAnimationForMove("Idle");
        }

        public void Exit()
        {
            Debug.Log("Exit Idle State");
        }

        public void Update()
        {
            Debug.Log("Update Idle State");
        }
    }
}