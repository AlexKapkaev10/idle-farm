using DG.Tweening;
using UnityEngine;

namespace Scripts.UI
{
    public class ResourceGroup : MonoBehaviour
    {
        [SerializeField] private float _moveDuration;
        [SerializeField] private Vector2 _centerPosition = new Vector2();
        [SerializeField] private Vector2 _cornerPosition = new Vector2();

        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            ChangeRectPosition(true);
        }

        public void ChangeRectPosition(bool isCenter)
        {
            _rectTransform.anchorMin = isCenter ? _centerPosition : _cornerPosition;
            _rectTransform.anchorMax = isCenter ? _centerPosition : _cornerPosition;
            _rectTransform.pivot = isCenter ? _centerPosition : _cornerPosition;
            
            if (!isCenter)
                transform.DOLocalMove(Vector3.zero, _moveDuration).SetEase(Ease.Linear);
            else
                transform.localPosition = Vector3.zero;
        }
    }
}
