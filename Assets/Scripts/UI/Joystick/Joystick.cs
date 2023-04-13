using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.UI
{
    public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [SerializeField]
        private float handleRange = 1;
        [SerializeField]
        private float deadZone = 0;

        [SerializeField] protected RectTransform background = null;
        [SerializeField] private RectTransform handle = null;
        private RectTransform baseRect = null;

        private Canvas canvas;
        private Camera cam;
        private Vector2 input = Vector2.zero;

        public float Horizontal { get => input.x; }
        public float Vertical { get => input.y; }
        public Vector2 Direction { get => new Vector2(Horizontal, Vertical); }

        private void Start()
        {
            baseRect = GetComponent<RectTransform>();
            canvas = GetComponentInParent<Canvas>();
            background.gameObject.SetActive(false);
            handle.anchoredPosition = Vector2.zero;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
            background.gameObject.SetActive(true);
            OnDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            cam = null;
            if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
                cam = canvas.worldCamera;

            Vector2 position = RectTransformUtility.WorldToScreenPoint(cam, background.position);
            Vector2 radius = background.sizeDelta / 2;
            input = (eventData.position - position) / (radius * canvas.scaleFactor);
            HandleInput(input.magnitude, input.normalized, radius, cam);
            handle.anchoredPosition = input * radius * handleRange;
        }

        private void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
        {
            if (magnitude > deadZone)
            {
                if (magnitude > 1)
                    input = normalised;
            }
            else
                input = Vector2.zero;
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            background.gameObject.SetActive(false);
            input = Vector2.zero;
            handle.anchoredPosition = Vector2.zero;
        }

        private Vector2 ScreenPointToAnchoredPosition(Vector2 screenPosition)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(baseRect, screenPosition, cam, out Vector2 localPoint))
            {
                Vector2 pivotOffset = baseRect.pivot * baseRect.sizeDelta;
                return localPoint - (background.anchorMax * baseRect.sizeDelta) + pivotOffset;
            }
            return Vector2.zero;
        }
    }
}