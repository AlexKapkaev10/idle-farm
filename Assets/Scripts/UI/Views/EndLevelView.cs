using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class EndLevelView : GameUI
    {
        [SerializeField] private TMP_Text _textHeader;
        [SerializeField] private Button _buttonAction;
        [SerializeField] private Button _buttonAddTime;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private CanvasGroup _canvasGroupBG;
        
        [SerializeField] private Vector2 _centerPosition;
        [SerializeField] private Vector2 _cornerPosition;
        
        public Button ButtonAction => _buttonAction;
        public Button ButtonAddTime => _buttonAddTime;

        public void SetResourcesViewParent(RectTransform target, bool isCenter, float duration)
        {
            target.transform.SetParent(_rectTransform);
            
            target.anchorMin = isCenter ? _centerPosition : _cornerPosition;
            target.anchorMax = isCenter ? _centerPosition : _cornerPosition;
            target.pivot = isCenter ? _centerPosition : _cornerPosition;

            target.DOLocalMove(Vector3.zero, duration * 0.5f).SetEase(Ease.Linear).OnComplete(() => DOTween.Kill(target));
        }

        public void SetHeader(string value)
        {
            _textHeader.SetText(value);
        }

        public override void SetVisible(bool isVisible, bool fastSet = false)
        {
            _canvasGroupBG.DOFade(1, FadeDuration).SetEase(Ease.Linear).OnComplete(() => DOTween.Kill(_canvasGroupBG));
        }
    }
}
