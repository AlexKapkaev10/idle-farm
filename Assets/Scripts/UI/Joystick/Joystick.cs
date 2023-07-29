using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public sealed class Joystick : GameUI, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        public event Action<bool> OnPress;

        [SerializeField] private float _handleRange = 1;
        [SerializeField] private float _deadZone = 0;
        [SerializeField] private RectTransform _background = null;
        [SerializeField] private RectTransform _handle = null;
        
        private RectTransform _baseRect = null;
        private Vector3 _defaultPosition;
        private Canvas _canvas;
        private Camera _cam;
        private Vector2 _input = Vector2.zero;

        public Vector2 Direction => _input;

        protected override void Awake()
        {
            base.Awake();
            _baseRect = GetComponent<RectTransform>();
            _canvas = GetComponentInParent<Canvas>();
            _defaultPosition = _background.anchoredPosition;
            _handle.anchoredPosition = Vector2.zero;
            _canvasGroup.blocksRaycasts = true;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = false;
            OnPress?.Invoke(true);
            _background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        }

        public void OnDrag(PointerEventData eventData)
        {
            _cam = null;
            if (_canvas.renderMode == RenderMode.ScreenSpaceCamera)
                _cam = _canvas.worldCamera;

            Vector2 position = RectTransformUtility.WorldToScreenPoint(_cam, _background.position);
            Vector2 radius = _background.sizeDelta / 2;
            _input = (eventData.position - position) / (radius * _canvas.scaleFactor);
            HandleInput(_input.magnitude, _input.normalized, radius, _cam);
            _handle.anchoredPosition = _input * radius * _handleRange;
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
            _canvasGroup.blocksRaycasts = true;
            OnPress?.Invoke(false);
            _input = Vector2.zero;
            _background.anchoredPosition = _defaultPosition;
            _handle.anchoredPosition = Vector2.zero;
        }

        private Vector2 ScreenPointToAnchoredPosition(Vector2 screenPosition)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_baseRect, screenPosition, _cam, out Vector2 localPoint))
            {
                Vector2 pivotOffset = _baseRect.pivot * _baseRect.sizeDelta;
                return localPoint - (_background.anchorMax * _baseRect.sizeDelta) + pivotOffset;
            }
            return Vector2.zero;
        }
    }
}