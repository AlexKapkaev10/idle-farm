using Scripts.Enums;
using Scripts.Interfaces;
using Scripts.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Scripts
{
    [RequireComponent(typeof(CharacterController))]
    public class Character : MonoBehaviour, ICharacterController, IContollable
    {
        public event Action OnMow;

        [SerializeField]
        private Transform _bodyTransform;
        [SerializeField]
        private float _runSpeed = 2f;
        [SerializeField]
        private Animator _playerAnimator;
        [SerializeField]
        private GameObject _tool;
        [SerializeField]
        private Transform _blocksPoint;

        private bool _isBlocksFull;
        private ResourceController _resourceController;
        private CharacterAnimationEvents _animationEvents;
        private CharacterController _characterController;
        private Vector3 _moveDirection;

        private Dictionary<Type, ICharacterBehavior> _behaviorsMap;
        private ICharacterBehavior _behaviorCurrent;

        [Inject]
        private void Construct(UIController uIController)
        {
            _resourceController = new ResourceController(uIController);
        }

        public void SetAnimationForField(FieldStateType type)
        {
            _tool.SetActive(type == FieldStateType.Mow);
            switch (type)
            {
                case FieldStateType.Default:
                    _playerAnimator.SetTrigger("Base");
                    break;
                case FieldStateType.Mow:
                    _playerAnimator.SetTrigger("Mow");
                    break;
            }
        }

        public void SetAnimationForMove(string key)
        {
            if (_playerAnimator)
                _playerAnimator.SetTrigger(key);
        }

        public Transform GetTransform()
        {
            return _bodyTransform;
        }

        public void SetPlant(PlantType type, PlantBlock block)
        {
            switch (type)
            {
                case PlantType.Wheat:
                    _resourceController.Add(type, block, _blocksPoint);
                    break;
            }
        }

        public void BuyPlants(PlantType type, Transform blocksTarget)
        {
            switch (type)
            {
                case PlantType.Wheat:
                    _resourceController.Buy(type, blocksTarget);
                    break;
                default:
                    break;
            }
        }

        public void UpdateMove(Vector3 direction)
        {
            _moveDirection = direction;
        }

        public void StartMove()
        {
            SetBehavior(GetBehavior<CharacterBehaviorRun>());
        }

        public void StopMove()
        {
            SetBehavior(GetBehavior<CharacterBehaviorIdle>());
        }

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _animationEvents = GetComponentInChildren<CharacterAnimationEvents>();

            if (_resourceController == null)
                Construct(null);

            _resourceController.OnFull += SetFull;
            _animationEvents.OnMow += InvokeMowAnimation;
            InitBehaviors();
            _tool.SetActive(false);
        }

        private void FixedUpdate()
        {
            MoveInternal();
        }

        private void OnDestroy()
        {
            _animationEvents.OnMow -= InvokeMowAnimation;
        }

        private void InitBehaviors()
        {
            _behaviorsMap = new Dictionary<Type, ICharacterBehavior>();

            _behaviorsMap[typeof(CharacterBehaviorIdle)] = new CharacterBehaviorIdle(this);
            _behaviorsMap[typeof(CharacterBehaviorRun)] = new CharacterBehaviorRun(this);
        }

        private void SetBehavior(ICharacterBehavior newBehavior)
        {
            if (_behaviorCurrent != null)
            {
                _behaviorCurrent.Exit();
            }

            _behaviorCurrent = newBehavior;
            _behaviorCurrent.Enter();
        }

        private ICharacterBehavior GetBehavior<T>() where T : ICharacterBehavior
        {
            var type = typeof(T);
            return _behaviorsMap[type];
        }

        private void SetFull(bool value)
        {
            _isBlocksFull = value;
        }

        private void MoveInternal()
        {
            if (_moveDirection != Vector3.zero)
            {
                _characterController.Move(_moveDirection * _runSpeed * Time.fixedDeltaTime);
                _bodyTransform.rotation = Quaternion.LookRotation(_moveDirection);
            }
        }

        private void InvokeMowAnimation()
        {
            if (_isBlocksFull)
                return;

            OnMow?.Invoke();
        }
    }
}
