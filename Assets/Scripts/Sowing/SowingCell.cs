using Scripts.ScriptableObjects;
using System;
using System.Collections;
using UnityEngine;

namespace Scripts
{
    public class SowingCell : MonoBehaviour
    {
        public event Action OnMow;
        public event Action OnRipe; 

        [SerializeField]
        private MeshRenderer _meshRenderer;
        [SerializeField]
        private Transform _plantPoint;
        [SerializeField]
        private Transform _blockPoint;

        private SowingData _sowingData;
        private float _ripeTime;
        private int _hp = 2;

        public int HP
        {
            get => _hp;
            set
            {
                _hp = value;
                
                if (_hp <= 0)
                    Interact();
                else
                    _meshRenderer.material.color = _sowingData.ColorHalfMow;
            }
        }

        public Transform GetBlockPoint()
        {
            return _blockPoint;
        }

        public void Init(SowingData sowingData)
        {
            _sowingData = sowingData;
            _ripeTime = sowingData.GetRipeningTime();
            Instantiate(sowingData.GetPlant(), _plantPoint);
            _meshRenderer.material = _sowingData.GetSowMaterial();
            _plantPoint.localScale = Vector3.one;
            _meshRenderer.material = _sowingData.GetRipeMaterial();
        }

        public void StartRipening()
        {
            StartCoroutine(Ripening());
        }

        private void OnDestroy()
        {
            OnMow = null;
            OnRipe = null;
        }

        private void Interact()
        {
            OnMow?.Invoke();
            _plantPoint.localScale = Vector3.zero;
            _meshRenderer.material = _sowingData.GetSowMaterial();
        }

        private IEnumerator Ripening()
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f, 1f));
            float timeElapsed = 0;
            Vector3 startValue = _plantPoint.localScale;
            while (timeElapsed < _ripeTime)
            {
                _plantPoint.localScale = Vector3.Lerp(startValue, Vector3.one, timeElapsed / _ripeTime);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            _hp = 2;
            _meshRenderer.material.color = default;
            _meshRenderer.material = _sowingData.GetRipeMaterial();
            OnRipe?.Invoke();
        }
    }
}
