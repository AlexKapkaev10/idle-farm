using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Scripts.UI
{
    public class ResourceGroup : MonoBehaviour
    {
        [SerializeField] private Vector2 _centerPosition = new Vector2();
        [SerializeField] private Vector2 _cornerPosition = new Vector2();
        [SerializeField] private RectTransform _rectTransform;
        
        private readonly List<ResourceView> _resourceViews = new List<ResourceView>();
        private Transform _transform;
        
        public RectTransform RectTransform => _rectTransform;

        private void Awake()
        {
            _transform = transform;
            _rectTransform = GetComponent<RectTransform>();
            ChangeRectPosition(true);
        }

        public void AddResourceView(ResourceView resourceView)
        {
            _resourceViews.Add(resourceView);
        }

        public void ChangeRectPosition(bool isCenter, float duration = 0.3f)
        {
            _rectTransform.anchorMin = isCenter ? _centerPosition : _cornerPosition;
            _rectTransform.anchorMax = isCenter ? _centerPosition : _cornerPosition;
            _rectTransform.pivot = isCenter ? _centerPosition : _cornerPosition;
            
            if (!isCenter)
            {
                _transform.DOLocalMove(Vector3.zero, duration * 0.5f).SetEase(Ease.Linear).OnComplete(ChangeResourceViewStringFormat);
            }
            else
            {
                _transform.DOLocalMove(Vector3.zero, duration * 0.5f).SetEase(Ease.Linear).OnComplete(() => DOTween.Kill(_transform));
            }
        }

        private void ChangeResourceViewStringFormat()
        {
            DOTween.Kill(_transform);
            
            foreach (var view in _resourceViews)
            {
                view.UpdateProgressCount("0");
            }
        }
    }
}
