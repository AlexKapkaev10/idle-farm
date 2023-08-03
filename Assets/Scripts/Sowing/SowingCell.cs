using Scripts.Enums;
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

        private float _ripeirTime;
        private SowingData _sowingData;
        private PlantType _plantType;
        private GameObject _plant;
        private bool _isMow;

        public bool IsMow
        {
            get => _isMow;
            set
            {
                _isMow = value;
                Interact();
            }
        }

        public Transform GetBlockPoint()
        {
            return _blockPoint;
        }

        public void Init(SowingData sowingData, PlantType type)
        {
            _sowingData = sowingData;
            _plantType = type;
            _ripeirTime = sowingData.GetRipeningTime();
            _plant = Instantiate(sowingData.GetPlant(), _plantPoint);
            _meshRenderer.material = _sowingData.GetSowMaterial();
            _plantPoint.localScale = Vector3.one;
            _isMow = false;
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
            while (timeElapsed < _ripeirTime)
            {
                _plantPoint.localScale = Vector3.Lerp(startValue, Vector3.one, timeElapsed / _ripeirTime);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            
            _isMow = false;
            _meshRenderer.material = _sowingData.GetRipeMaterial();
            OnRipe?.Invoke();
            
        }
    }
}
