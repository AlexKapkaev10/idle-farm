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

        public Button ButtonAction => _buttonAction;
        public Button ButtonAddTime => _buttonAddTime;

        public void SetResourceGroup(ResourceGroup resourceGroup)
        {
            resourceGroup.transform.SetParent(_rectTransform);
            resourceGroup.ChangeRectPosition(true, FadeDuration);
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
