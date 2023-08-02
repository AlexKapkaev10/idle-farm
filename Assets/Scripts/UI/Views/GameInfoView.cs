using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Scripts.UI
{
    public class GameInfoView : GameUI
    {
        [SerializeField] private GameInfoSettings _settings;
        [SerializeField] private RectTransform _resourceGroupParent;
        [SerializeField] private CanvasGroup _infoGroup;
        [SerializeField] private TMP_Text _textMoney;
        [SerializeField] private TMP_Text _textTimer;
        
        public TMP_Text TextMoney => _textMoney;

        public override void SetVisible(bool isVisible, bool fastSet = false)
        {
            if (fastSet)
            {
                _infoGroup.alpha = isVisible ? 1f : 0f;
                return;
            }
            
            if (isVisible)
                _infoGroup.DOFade(1, _settings.InfoFadeDuration).SetEase(Ease.Linear).OnComplete(()=> DOTween.Kill(_infoGroup));
            else
                _infoGroup.DOFade(0, _settings.InfoFadeDuration).SetEase(Ease.Linear).OnComplete(Clear);
        }
        
        public void SetTimerText(string value)
        {
            _textTimer.SetText(value);
        }

        public void UpdateTextColor(bool isDefault)
        {
            _textTimer.color = isDefault ? _settings.ColorDefaultTimer : _settings.ColorHurryTimer;
        }

        public void SetResourceGroup(in ResourceGroup resourceGroup)
        {
            resourceGroup.transform.SetParent(_resourceGroupParent);
            resourceGroup.ChangeRectPosition(false);
            SetVisible(true, true);
        }

        private void OnDestroy()
        {
            Clear();
        }

        private void Clear()
        {
            UpdateTextColor(true);
            DOTween.Kill(_infoGroup);
        }
    }
}
