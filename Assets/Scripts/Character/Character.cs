using Scripts.Enums;
using Scripts.Interfaces;
using System;
using System.Collections.Generic;
using Scripts.Plants;
using Scripts.ScriptableObjects;
using Scripts.StateMachine;
using UnityEngine;
using VContainer;

namespace Scripts.Game
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class Character : MonoBehaviour, ICharacterController, IMoveControllable
    {
        public event Action OnMow;

        [SerializeField] private ToolType _toolType;
        [SerializeField] private PlantCollectType _plantCollectType;
        [SerializeField] private Transform _bodyTransform;
        [SerializeField] private Animator _playerAnimator;
        [SerializeField] private Transform _toolPoint;
        [SerializeField] private GameObject _bagObj;
        [SerializeField] private Transform _transformCollectPoint;
        [SerializeField] private float _runRunSpeed = 2f;

        private readonly AnimatorParameters _animatorParameters = new AnimatorParameters();

        private ICharacterStateMachine _characterStateMachine;

        private IResourceController _resourceController;
        private ITool _currentTool;

        private CharacterAnimationEvents _eventFromAnimation;
        private ToolsSettings _toolsSettings;

        private CharacterController _characterController;

        [Inject]
        public void Init(
            IResourceController resourceController, 
            ToolsSettings toolsSettings, 
            ICharacterStateMachine characterStateMachine)
        {
            _resourceController = resourceController;
            _toolsSettings = toolsSettings;
            _characterStateMachine = characterStateMachine;
        }

        public void Move(Vector3 velocity)
        {
            _characterController.Move(velocity);
        }

        public void Rotate(Quaternion rotation)
        {
            _bodyTransform.rotation = rotation;
        }

        public void SetAnimationForField(FieldStateType fieldState)
        {
            _currentTool.SetActive(fieldState == FieldStateType.Mow);
            switch (fieldState)
            {
                case FieldStateType.Default:
                    _playerAnimator.SetTrigger(_animatorParameters.Base);
                    break;
                case FieldStateType.Mow:
                    _playerAnimator.SetTrigger(_animatorParameters.Mow);
                    break;
            }
        }

        public void SetAnimationForMove(string key)
        {
            if (_playerAnimator)
                _playerAnimator.SetTrigger(Animator.StringToHash(key));
        }

        public Transform GetBodyTransform()
        {
            return _bodyTransform;
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        public void AddPlant(in Plant plant)
        {
            _resourceController.Add(plant);
            plant.MoveToTarget(_transformCollectPoint, 0.5f);
        }
        
        public void BuyPlants(in List<PlantType> plants)
        {
            _resourceController.Buy(plants);
        }

        public void ChangeMoveState(bool isMove)
        {
            _characterStateMachine.SetBehaviorByType(isMove ? CharacterStateType.Run : CharacterStateType.Idle);
        }

        private void Awake()
        {
            SetTool();

            _bagObj.SetActive(_plantCollectType == PlantCollectType.InBag);
            
            _characterController = GetComponent<CharacterController>();
            _eventFromAnimation = GetComponentInChildren<CharacterAnimationEvents>();
            
            _eventFromAnimation.OnMow += InvokeMowEventFromAnimation;
            
            _characterStateMachine.InitBehaviors(this);
            _characterStateMachine.SetBehaviorByType(CharacterStateType.Idle);
        }

        private void Update()
        {
            _characterStateMachine.CurrentBehavior?.Update();
        }

        private void OnDestroy()
        {
            _eventFromAnimation.OnMow -= InvokeMowEventFromAnimation;
        }

        private void SetTool()
        {
            _currentTool?.Clear();

            var tool = Instantiate(_toolsSettings.GetTool(_toolType), _toolPoint);
            _currentTool = tool;

            if (_currentTool == null) 
                return;
            
            _playerAnimator.SetFloat(_animatorParameters.MowSpeed, _currentTool.MowSpeed);
            _currentTool.SetActive(false);
        }

        private void InvokeMowEventFromAnimation()
        {
            OnMow?.Invoke();
        }
    }
    
    public partial class AnimatorParameters
    {
        public readonly int Base = Animator.StringToHash("Base");
        public readonly int Mow = Animator.StringToHash("Mow");
        public readonly int MowSpeed = Animator.StringToHash("mowSpeed");
    }
}
