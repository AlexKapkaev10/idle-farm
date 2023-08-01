using UnityEngine;

namespace Scripts.CameraGame
{
    public class MapCamera : MonoBehaviour
    {
        [SerializeField] private Vector3 _offSet = new Vector3(0, 20, 0);
        
        private Transform _transform;
        private Transform _target;

        private void Awake()
        {
            _transform = transform;
        }

        public void SetFollowTarget(Transform target)
        {
            _target = target;
        }

        private void LateUpdate()
        {
            if (_target)
                _transform.position = _target.position + _offSet;
        }
    }
}