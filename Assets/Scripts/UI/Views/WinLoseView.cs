using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class WinLoseView : GameUI
    {
        [SerializeField] private TMP_Text _textHeader;
        [SerializeField] private Button _buttonAction;
        [SerializeField] private Button _buttonAddTime;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private CanvasGroup _canvasGroupBG;

        public float FadeDuration { get; set; } = 0.2f;

        public Button ButtonAction => _buttonAction;
        public Button ButtonAddTime => _buttonAddTime;

        public override void SetVisible(bool isVisible, bool fastSet = false)
        {
            _canvasGroupBG.DOFade(1, FadeDuration).SetEase(Ease.Linear).OnComplete(() => DOTween.Kill(_canvasGroupBG));
        }
        
        public void SetHeader(string value)
        {
            _textHeader.SetText(value);
        }
    }
}
