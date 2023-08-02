using System.Collections.Generic;
using Scripts.Game;
using Scripts.Interfaces;
using Scripts.UI;
using UnityEngine;
using VContainer;

namespace Scripts.StateMachine
{
    public sealed class CharacterStateMachine : ICharacterStateMachine
    {
        private readonly IGameUIController _gameUIController = default;
        private readonly CharacterSettings _characterSettings = default;
        
        private Dictionary<CharacterStateType, ICharacterBehavior> _behaviorsMap;
        private ICharacterBehavior _currentBehavior;
        private CharacterStateType _characterStateType;

        public ICharacterBehavior CurrentBehavior => _currentBehavior;
        public CharacterStateType CharacterStateType => _characterStateType;

        [Inject]
        public CharacterStateMachine(IObjectResolver resolver, CharacterSettings characterSettings)
        {
            _gameUIController = resolver.Resolve<IGameUIController>();
            _characterSettings = characterSettings;
        }

        public void InitBehaviors(ICharacterController characterController)
        {
            _behaviorsMap = new Dictionary<CharacterStateType, ICharacterBehavior>
            {
                [CharacterStateType.Idle] = new CharacterBehaviorIdle(characterController),
                [CharacterStateType.Run] = new CharacterBehaviorRun(characterController, _gameUIController, _characterSettings)
            };
        }

        public void SetBehaviorByType(CharacterStateType type)
        {
            if (_currentBehavior == GetBehavior<ICharacterBehavior>(type))
                return;
            
            _characterStateType = type;
            _currentBehavior?.Exit();
            _currentBehavior = GetBehavior<ICharacterBehavior>(type);
            _currentBehavior?.Enter();
        }

        private ICharacterBehavior GetBehavior<T>(CharacterStateType stateType) where T : ICharacterBehavior
        {
            return _behaviorsMap[stateType];
        }
    }
}