using UnityEngine;

namespace Scripts.Store
{
    [RequireComponent(typeof(CanvasGroup))]
    public class StoreController : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;
        private bool _isActive = true;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            SetActiveStore();
        }

        public void SetActiveStore()
        {
            if (_canvasGroup == null)
                return;
            
            _isActive = !_isActive;
            _canvasGroup.alpha = _isActive ? 1f : 0f;
            _canvasGroup.interactable = _isActive;
            _canvasGroup.blocksRaycasts = _isActive;
        }
    }
}
