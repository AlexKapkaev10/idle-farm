using Scripts.Interfaces;
using UnityEngine;

namespace Scripts
{
    [RequireComponent(typeof(CharacterController))]
    public class Character : MonoBehaviour, IContollable
    {
        [SerializeField]
        private Transform _bodyTransform;
        [SerializeField]
        private float _runSpeed = 2f;
        [SerializeField]
        private Animator _playerAnimator;
        [SerializeField]
        private string _interactableLayer;
        [SerializeField]
        private Transform _rayPoint;
        [SerializeField]
        private LayerMask _layerMaskInteractable;
        [SerializeField]
        private float _sphereCastRadius = 1f;

        private bool _isRun;
        private CharacterController _characterController;
        private Vector3 _moveDirection;

        public void Move(Vector3 direction)
        {
            _moveDirection = direction;
        }

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        private void FixedUpdate()
        {
            MoveInternal();
            FindInteractables();
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

        private void FindInteractables()
        {
            RaycastHit[] _interactableHits = Physics.SphereCastAll(_rayPoint.position, _sphereCastRadius, _rayPoint.TransformDirection(Vector3.down), _sphereCastRadius, _layerMaskInteractable);

            if (_interactableHits.Length > 0)
            {
                _playerAnimator.SetBool("Sowing", _interactableHits.Length > 0);

                for (int i = 0; i < _interactableHits.Length; i++)
                {
                    if (_interactableHits[i].collider.TryGetComponent<IInteractable>(out IInteractable interactable))
                    {
                        interactable.Interact();
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(_rayPoint.position + _rayPoint.TransformDirection(Vector3.down), _sphereCastRadius);
        }
    }
}
