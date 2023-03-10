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

        private CharacterAnimationEvents _animationEvents;
        private bool _isRun;
        private CharacterController _characterController;
        private Vector3 _moveDirection;

        public void SetAnimationState(FieldStateType fuildState)
        {
            switch (fuildState)
            {
                case FieldStateType.Default:
                case FieldStateType.EndSow:
                case FieldStateType.EndRipe:
                    _playerAnimator.SetTrigger("Base");
                    break;
                case FieldStateType.Sow:
                    _playerAnimator.SetTrigger("Sow");
                    break;
                case FieldStateType.Ripe:
                    _playerAnimator.SetTrigger("Mow");
                    break;
            }
        }

        public Transform GetTransform()
        {
            return _bodyTransform;
        }

        public void Move(Vector3 direction)
        {
            _moveDirection = direction;
        }

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _animationEvents = GetComponentInChildren<CharacterAnimationEvents>();
            _animationEvents.OnMow += InvokeMowAnimation;
        }

        private void FixedUpdate()
        {
            MoveInternal();
        }

        private void OnDestroy()
        {
            _animationEvents.OnMow -= InvokeMowAnimation;
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
            OnMow?.Invoke();
        }
    }
}
