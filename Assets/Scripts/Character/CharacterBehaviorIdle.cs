using System.Collections;
using UnityEngine;
using Scripts.Interfaces;

namespace Scripts
{
    public class CharacterBehaviorIdle : ICharacterBehavior
    {
        public void Enter()
        {
            Debug.Log("Enter Idle State");
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