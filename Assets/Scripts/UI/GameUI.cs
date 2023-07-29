using UnityEngine;

namespace Scripts.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class GameUI : MonoBehaviour
    {
        protected CanvasGroup _canvasGroup;

        public bool IsCheckVisible { get; private set; }

        public void CheckVisible()
        {
            IsCheckVisible = _canvasGroup.alpha < 0.1f;
        }
        
        public virtual void SetVisible(bool isVisible)
        {
            if (!_canvasGroup)
                return;
            
            _canvasGroup.alpha = isVisible ? 1 : 0;
            _canvasGroup.blocksRaycasts = isVisible;
            _canvasGroup.interactable = isVisible;
        }

        protected virtual void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }
    }
}