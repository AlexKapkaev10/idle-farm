using Scripts.Enums;
using Scripts.Interfaces;
using System;
using System.Collections.Generic;
using Scripts.ScriptableObjects;
using Scripts.UI;
using UnityEngine;
using VContainer;

namespace Scripts.Game
{
    [RequireComponent(typeof(CharacterController))]
    public class Character : MonoBehaviour, ICharacterController, IControllable
    {
        public event Action OnMow;

        [SerializeField] private JoystickInputController _joystickInputController;
        [SerializeField] private Transform _bodyTransform;
        [SerializeField] private float _runSpeed = 2f;
        [SerializeField] private Animator _playerAnimator;
        [SerializeField] private Transform _toolPoint;
        [SerializeField] private Transform _blocksPoint;

        private GameUI _gameUI;
        private ResourceController _resourceController;
        private CharacterAnimationEvents _animationEvents;
        private CharacterController _characterController;
        private ToolsSettings _toolsSettings;
        private GameObject _currentTool;
        private bool _isBlocksFull;
        
        private Dictionary<Type, ICharacterBehavior> _behaviorsMap;
        private ICharacterBehavior _behaviorCurrent;

        public CharacterController CharacterController => _characterController;
        public Transform BodyTransform => _bodyTransform;
        
        public float Speed => _runSpeed;
        public Transform Body => _bodyTransform;

        [Inject]
        public void Init(GameUI gameUI, ResourceController resourceController, ToolsSettings toolsSettings)
        {
            _gameUI = gameUI;
            _resourceController = resourceController;
            _toolsSettings = toolsSettings;
        }

        public void SetAnimationForField(FieldStateType fieldState)
        {
            _currentTool.SetActive(fieldState == FieldStateType.Mow);
            switch (fieldState)
            {
                case FieldStateType.Default:
                    _playerAnimator.SetTrigger(Animator.StringToHash("Base"));
                    break;
                case FieldStateType.Mow:
                    _playerAnimator.SetTrigger(Animator.StringToHash("Mow"));
                    break;
            }
        }

        public void SetAnimationForMove(string key)
        {
            if (_playerAnimator)
                _playerAnimator.SetTrigger(Animator.StringToHash(key));
        }

        public Transform GetTransform()
        {
            return _bodyTransform;
        }

        public void AddPlant(PlantType type, PlantBlock block)
        {
            _resourceController.Add(type, block, _blocksPoint);
        }

        public void BuyPlants(PlantType type, Transform blocksTarget)
        {
            _resourceController.Buy(type, blocksTarget);
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
            SetTool();

            _characterController = GetComponent<CharacterController>();
            _resourceController.OnFull += SetFull;
            
            _animationEvents = GetComponentInChildren<CharacterAnimationEvents>();
            _animationEvents.OnMow += InvokeMowAnimation;
            
            _joystickInputController.Init(_gameUI.GetJoystick());
            InitBehaviors();
            SetBehavior(GetBehavior<CharacterBehaviorIdle>());
        }

        private void Update()
        {
            _behaviorCurrent.Update();
        }

        private void FixedUpdate()
        {
            _behaviorCurrent.FixedUpdate();
        }

        private void OnDestroy()
        {
            _animationEvents.OnMow -= InvokeMowAnimation;
        }

        private void SetTool()
        {
            _currentTool = Instantiate(_toolsSettings.GetTool(ToolType.Default), _toolPoint);
            _currentTool.SetActive(false);
        }

        private void InitBehaviors()
        {
            _behaviorsMap = new Dictionary<Type, ICharacterBehavior>
            {
                [typeof(CharacterBehaviorIdle)] = new CharacterBehaviorIdle(this),
                [typeof(CharacterBehaviorRun)] = new CharacterBehaviorRun(this, _gameUI.GetJoystick())
            };
        }

        private void SetBehavior(ICharacterBehavior newBehavior)
        {
            _behaviorCurrent?.Exit();
            _behaviorCurrent = newBehavior;
            _behaviorCurrent?.Enter();
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

        private void InvokeMowAnimation()
        {
            if (_isBlocksFull)
                return;

            OnMow?.Invoke();
        }
    }
}
