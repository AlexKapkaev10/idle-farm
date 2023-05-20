using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.UI
{
    public sealed class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        public event Action OnDown;
        public event Action OnUp;

        [SerializeField] private float _handleRange = 1;
        [SerializeField] private float _deadZone = 0;
        [SerializeField] private RectTransform background = null;
        [SerializeField] private RectTransform handle = null;
        
        private RectTransform _baseRect = null;
        private Vector3 _defaultPosition;
        private Canvas _canvas;
        private Camera _cam;
        private Vector2 _input = Vector2.zero;

        public Vector2 Direction => _input;

        private void Awake()
        {
            _baseRect = GetComponent<RectTransform>();
            _canvas = GetComponentInParent<Canvas>();
            _defaultPosition = background.anchoredPosition;
            handle.anchoredPosition = Vector2.zero;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnDown?.Invoke();
            background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
            OnDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            _cam = null;
            if (_canvas.renderMode == RenderMode.ScreenSpaceCamera)
                _cam = _canvas.worldCamera;

            Vector2 position = RectTransformUtility.WorldToScreenPoint(_cam, background.position);
            Vector2 radius = background.sizeDelta / 2;
            _input = (eventData.position - position) / (radius * _canvas.scaleFactor);
            HandleInput(_input.magnitude, _input.normalized, radius, _cam);
            handle.anchoredPosition = _input * radius * _handleRange;
        }

        private void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
        {
            if (magnitude > _deadZone)
            {
                if (magnitude > 1)
                    _input = normalised;
            }
            else
                _input = Vector2.zero;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnUp?.Invoke();
            _input = Vector2.zero;
            background.anchoredPosition = _defaultPosition;
            handle.anchoredPosition = Vector2.zero;
        }

        private Vector2 ScreenPointToAnchoredPosition(Vector2 screenPosition)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_baseRect, screenPosition, _cam, out Vector2 localPoint))
            {
                Vector2 pivotOffset = _baseRect.pivot * _baseRect.sizeDelta;
                return localPoint - (background.anchorMax * _baseRect.sizeDelta) + pivotOffset;
            }
            return Vector2.zero;
        }
    }
}