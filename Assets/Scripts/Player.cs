using Scripts.Interfaces;
using UnityEngine;

namespace Scripts
{
    [RequireComponent(typeof(CharacterController))]
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private Joystick _joystick;
        [SerializeField]
        private Transform _bodyTransform;
        [SerializeField]
        private float _runSpeed = 2f;
        [SerializeField]
        private Animator _playerAnimator;
        [SerializeField]
        private string _interactableLayer;

        private bool _isRun;
        private CharacterController _characterController;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            Vector3 direction = Vector3.forward * _joystick.Vertical + Vector3.right * _joystick.Horizontal;

            if (direction != Vector3.zero)
            {
                if (!_isRun)
                {
                    _isRun = true;
                    if (_playerAnimator)
                        _playerAnimator.SetTrigger("Run");
                }

                _characterController.Move(direction * (_runSpeed / 10f));
                _bodyTransform.rotation = Quaternion.LookRotation(direction);
            }
            else if (_isRun)
            {
                _isRun = false;
                if (_playerAnimator)
                    _playerAnimator.SetTrigger("Idle");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 6)
            {
                if (other.TryGetComponent<IInteractable>(out IInteractable interactable))
                {
                    interactable.Interact();
                }
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer(_interactableLayer))
            {
                if (collision.collider.TryGetComponent<IInteractable>(out IInteractable interactable))
                {
                    interactable.Interact();
                }
            }
        }
    }
}
