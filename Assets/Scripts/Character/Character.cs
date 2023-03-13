using Scripts.Enums;
using Scripts.Interfaces;
using System;
using UnityEngine;

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
        private ResourceController _resourceController = new ResourceController();
        private CharacterAnimationEvents _animationEvents;
        private bool _isRun;
        private CharacterController _characterController;
        private Vector3 _moveDirection;

        public void SetAnimationState(FieldStateType type)
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

        public void Move(Vector3 direction)
        {
            _moveDirection = direction;
        }

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _animationEvents = GetComponentInChildren<CharacterAnimationEvents>();
            _resourceController.OnFull += SetFull;
            _animationEvents.OnMow += InvokeMowAnimation;
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

        private void SetFull(bool value)
        {
            _isBlocksFull = value;
        }

        private void MoveInternal()
        {
            if (_moveDirection != Vector3.zero)
            {
                if (!_isRun)
                {
                    _isRun = true;
                    if (_playerAnimator)
                        _playerAnimator.SetTrigger("Run");
                }

                _characterController.Move(_moveDirection * _runSpeed * Time.fixedDeltaTime);
                _bodyTransform.rotation = Quaternion.LookRotation(_moveDirection);
            }
            else if (_isRun)
            {
                _isRun = false;
                if (_playerAnimator)
                    _playerAnimator.SetTrigger("Idle");
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
