using Scripts.Enums;
using Scripts.Interfaces;
using System;
using System.Collections.Generic;
using Scripts.Plants;
using Scripts.ScriptableObjects;
using Scripts.UI;
using UnityEngine;
using VContainer;

namespace Scripts.Game
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class Character : MonoBehaviour, ICharacterController, IControllable
    {
        public event Action OnMow;

        [SerializeField] private ToolType _toolType;
        [SerializeField] private PlantCollectType _plantCollectType;
        [SerializeField] private JoystickInputController _joystickInputController;
        [SerializeField] private Transform _bodyTransform;
        [SerializeField] private Animator _playerAnimator;
        [SerializeField] private Transform _toolPoint;
        [SerializeField] private GameObject _bagObj;
        [SerializeField] private Transform _transformCollectPoint;
        [SerializeField] private float _runSpeed = 2f;

        private readonly AnimatorParameters _animatorParameters = new AnimatorParameters();

        private Dictionary<Type, ICharacterBehavior> _behaviorsMap;
        private ICharacterBehavior _behaviorCurrent;

        private GameUI _gameUI;
        private ResourceController _resourceController;
        private CharacterAnimationEvents _eventFromAnimation;
        private ToolsSettings _toolsSettings;
        private ITool _currentTool;
        
        private CharacterController _characterController;

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

            _bagObj.SetActive(_plantCollectType == PlantCollectType.InBag);
            _characterController = GetComponent<CharacterController>();

            _eventFromAnimation = GetComponentInChildren<CharacterAnimationEvents>();
            _eventFromAnimation.OnMow += InvokeMowEventFromAnimation;
            
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
