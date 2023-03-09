using Scripts.Enums;
using Scripts.Interfaces;
using System;
using System.Collections;
using UnityEngine;

namespace Scripts
{
    public class SowingCell : MonoBehaviour, IInteractable
    {
        public event Action OnSowing;

        [SerializeField]
        private Collider _collider;
        [SerializeField]
        private MeshRenderer _meshRenderer;
        [SerializeField]
        private Transform _plantPoint;
        [SerializeField]
        private float _ripeinigTime;

        private bool _isRipe;
        private SowingType _sowingType;

        private Material _defaultMaterial;
        private Material _sowingMaterial;
        private Material _ripeMaterial;

        public bool IsRipe
        {
            get => _isRipe;
            set
            {
                _isRipe = value;
                _collider.enabled = true;
            }
        }

        public void Init(SowingType type, Material sowingMaterial, Material ripeMaterial)
        {
            _defaultMaterial = _meshRenderer.material;
            _sowingMaterial = sowingMaterial;
            _ripeMaterial = ripeMaterial;
            _sowingType = type;
        }

        public void Interact()
        {
            _collider.enabled = false;

            if (!_isRipe)
            {
                _plantPoint.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                _meshRenderer.material = _sowingMaterial;
            }
            else
            {
                _meshRenderer.material = _defaultMaterial;
                Debug.Log("Add");
            }

            OnSowing?.Invoke();
        }

        public void StartRipening()
        {
            StartCoroutine(Ripening());
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
            _meshRenderer.material = _ripeMaterial;
        }
    }
}
