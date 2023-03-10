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
        [SerializeField]
        private MeshRenderer _meshRenderer;
        [SerializeField]
        private Transform _plantPoint;

        private float _ripeinigTime;
        private SowingData _sowingData;
        private PlantType _plantType;
        private GameObject _plant;
        private bool _hasInteract = true;
        private bool _isRipe;

        public void Interact()
        {
            if (!_isRipe)
            {
                _plantPoint.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                _meshRenderer.material = _sowingData.GetSowMaterialByPlantType(_plantType);
            }
            else
            {
                _plantPoint.localScale = Vector3.zero;
                _meshRenderer.material = _sowingData.GetDefaultMaterialByPlantType(_plantType);
            }
        }

        public bool HasInteract
        {
            get => _hasInteract;
            set
            {
                _hasInteract = value;
                Interact();
            }
        }

        public void Init(SowingData sowingData, PlantType type)
        {
            _sowingData = sowingData;
            _plantType = type;
            _ripeinigTime = sowingData.GetRepeningTimeByPlantType(type);
            _plant = Instantiate(sowingData.GetPlantByPlantType(type), _plantPoint);
        }

        public void StartRipening()
        {
            if (!_isRipe)
            {
                StartCoroutine(Ripening());
            }
            else
            {
                _isRipe = false;
                _hasInteract = true;
            }
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

            _isRipe = true;
            _hasInteract = true;
            _meshRenderer.material = _sowingData.GetRipeMaterialByPlantType(_plantType);
        }
    }
}
