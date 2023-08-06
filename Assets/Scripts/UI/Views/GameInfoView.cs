using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Scripts.UI
{
    public class GameInfoView : GameUI
    {
        [SerializeField] private GameInfoSettings _settings;
        [SerializeField] private RectTransform _resourcesView;
        [SerializeField] private RectTransform _resourcesParent;
        [SerializeField] private CanvasGroup _infoGroup;
        [SerializeField] private RectTransform _moneyRectTransform;
        [SerializeField] private TMP_Text _textMoney;
        [SerializeField] private TMP_Text _textTimer;

        private ResourceGroup _resourceGroup;
        private Vector2 _cornerValue = new (0f, 1f);

        private Transform _transform;
        public RectTransform ResourcesView => _resourcesView;

        public TMP_Text TextMoney => _textMoney;

        protected override void Awake()
        {
            _transform = transform;
        }

        public void SetMoneyTextParent(Transform parent)
        {
            _moneyRectTransform.SetParent(parent ? parent : transform, true);
        }

        public override void SetVisible(bool isVisible, bool fastSet = false)
        {
            if (fastSet)
            {
                _infoGroup.alpha = isVisible ? 1f : 0f;
                return;
            }
            
            if (isVisible)
                _infoGroup.DOFade(1, FadeDuration).SetEase(Ease.Linear).OnComplete(()=> DOTween.Kill(_infoGroup));
            else
                _infoGroup.DOFade(0, FadeDuration).SetEase(Ease.Linear).OnComplete(Clear);
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
            _resourceGroup = resourceGroup;
            _resourceGroup.transform.SetParent(_resourcesView, true);
            _resourceGroup.ChangeRectPosition(false, FadeDuration);
            SetVisible(true, true);
        }

        public void ReturnResourcesView(bool isClear = false)
        {
            _resourcesView.anchorMin = _cornerValue;
            _resourcesView.anchorMax = _cornerValue;
            _resourcesView.pivot = _cornerValue;
            _resourcesView.transform.SetParent(_resourcesParent, true);

            if (isClear)
            {
                _resourcesView.transform.localPosition = Vector3.zero;
            }
            else
            {
                _resourcesView.transform.DOLocalMove(Vector3.zero, FadeDuration * 0.5f).SetEase(Ease.Linear);
            }
        }

        public void ScaleMoneyView(float scaleValue, float duration)
        {
            _moneyRectTransform.DOScale(scaleValue, duration).SetEase(Ease.Linear).OnComplete(() => DOTween.Kill(_moneyRectTransform));
        }

        private void OnDestroy()
        {
            Clear();
        }

        private void Clear()
        {
            UpdateTextColor(true);
            DOTween.Kill(_infoGroup);
            DOTween.Kill(_transform);
            DOTween.Kill(_moneyRectTransform);
        }
    }
}
