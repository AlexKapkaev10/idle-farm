using Scripts.Enums;
using Scripts.ScriptableObjects;
using System;
using System.Collections;
using UnityEngine;

namespace Scripts
{
    public class SowingCell : MonoBehaviour
    {
        public event Action OnRipe;
        public event Action OnMow;

        [SerializeField]
        private MeshRenderer _meshRenderer;
        [SerializeField]
        private Transform _plantPoint;
        [SerializeField]
        private Transform _blockPoint;

        private float _ripeinigTime;
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

        public void Interact()
        {
            OnMow?.Invoke();
            _plantPoint.localScale = Vector3.zero;
            _meshRenderer.material = _sowingData.GetSowMaterialByPlantType(_plantType);
        }

        public void Init(SowingData sowingData, PlantType type)
        {
            _sowingData = sowingData;
            _plantType = type;
            _ripeinigTime = sowingData.GetRepeningTimeByPlantType(type);
            _plant = Instantiate(sowingData.GetPlantByPlantType(type), _plantPoint);
            _meshRenderer.material = _sowingData.GetSowMaterialByPlantType(_plantType);
            StartRipening();
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

        private IEnumerator Ripening()
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f, 1f));
            float timeElapsed = 0;
            Vector3 startValue = _plantPoint.localScale;
            while (timeElapsed < _ripeinigTime)
            {
                _plantPoint.localScale = Vector3.Lerp(startValue, Vector3.one, timeElapsed / _ripeinigTime);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            OnRipe?.Invoke();
            _isMow = false;
            _meshRenderer.material = _sowingData.GetRipeMaterialByPlantType(_plantType);
        }
    }
}
