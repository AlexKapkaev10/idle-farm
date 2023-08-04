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
        public event Action<int> OnMow;

        [SerializeField] private ToolType _toolType;
        [SerializeField] private Transform _bodyTransform;
        [SerializeField] private Animator _playerAnimator;
        [SerializeField] private Transform _toolPoint;
        [SerializeField] private Transform _transformCollectPoint;

        private ICharacterStateMachine _characterStateMachine;

        private IResourceController _resourceController;
        private ITool _currentTool;

        private CharacterAnimationEvents _eventFromAnimation;
        private ToolsSettings _toolsSettings;

        private CharacterController _characterController;

        public Animator Animator => _playerAnimator;

        public ITool CurrentTool => _currentTool;

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

        public void StartLevel()
        {
            _characterController.center = new Vector3(0, 1, 0);
        }

        public void EndLevel(bool isWin)
        {
            _characterController.center = new Vector3(0, 10, 0);
            _playerAnimator.SetTrigger(isWin ? AnimatorParameters.Win : AnimatorParameters.Lose);
        }

        public void Move(Vector3 velocity, float magnitude)
        {
            _characterController.Move(velocity);
            _playerAnimator.SetFloat(AnimatorParameters.RunSpeed, magnitude);
        }

        public void Rotate(Quaternion rotation)
        {
            _bodyTransform.rotation = rotation;
        }

        public void SetAnimationForField(FieldStateType fieldState)
        {
            _currentTool?.SetActive(fieldState == FieldStateType.Mow);
            
            if (!_playerAnimator)
                return;
            _playerAnimator?.SetTrigger(fieldState == FieldStateType.Ripe
                ? AnimatorParameters.Base
                : AnimatorParameters.Mow);
        }

        public void AddPlant(in PlantBlock plantBlock)
        {
            _resourceController.Add(plantBlock);
            plantBlock.MoveToTarget(_transformCollectPoint, 0.5f);
        }

        public void BuyPlants(in List<PlantType> plants)
        {
            _resourceController.Buy(plants);
        }

        public void SetTransform(Vector3 position, Vector3 bodyRotation)
        {
            transform.position = position;
            _bodyTransform.localEulerAngles = bodyRotation;
        }

        public void ChangeMoveState(bool isRun)
        {
            _characterStateMachine.SetBehaviorByType(isRun ? CharacterStateType.Run : CharacterStateType.Idle);
        }


        public Transform GetBodyTransform()
        {
            return _bodyTransform;
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        private void Awake()
        {
            CreateTool();
            _characterController = GetComponent<CharacterController>();
            _eventFromAnimation = GetComponentInChildren<CharacterAnimationEvents>();
            
            _eventFromAnimation.OnMow += InvokeMowEventFromAnimation;
        }

        private void Start()
        {
            _characterStateMachine.InitBehaviors(this);
            ChangeMoveState(false);
        }

        private void Update()
        {
            _characterStateMachine.CurrentBehavior?.Update();
        }

        private void OnDestroy()
        {
            _eventFromAnimation.OnMow -= InvokeMowEventFromAnimation;
        }

        private void CreateTool()
        {
            _currentTool?.Clear();

            var tool = Instantiate(_toolsSettings.GetTool(_toolType), _toolPoint);
            _currentTool = tool;
            _currentTool.SetMaxSharpCount();

            if (_currentTool == null) 
                return;
            
            _playerAnimator.SetFloat(AnimatorParameters.MowSpeed, _currentTool.MowSpeed);
            _currentTool?.SetActive(false);
        }

        private void InvokeMowEventFromAnimation()
        {
            OnMow?.Invoke(_currentTool.CurrentSharpCount);
        }
    }
    
    public static partial class AnimatorParameters
    {
        public static readonly int Base = Animator.StringToHash("Base");
        public static readonly int IdleBool = Animator.StringToHash("IdleBool");
        public static readonly int RunBool = Animator.StringToHash("RunBool");
        public static readonly int Mow = Animator.StringToHash("Mow");
        public static readonly int Win = Animator.StringToHash("Win");
        public static readonly int Lose = Animator.StringToHash("Lose");
        public static readonly int MowSpeed = Animator.StringToHash("mowSpeed");
        public static readonly int RunSpeed = Animator.StringToHash("runSpeed");
    }
}
