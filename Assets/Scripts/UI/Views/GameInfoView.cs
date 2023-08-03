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
            _resourceGroup.transform.SetParent(_resourcesView);
            _resourceGroup.ChangeRectPosition(false, FadeDuration);
            SetVisible(true, true);
        }

        public void ReturnResourcesView(bool isClear = false)
        {
            _resourcesView.transform.SetParent(_resourcesParent);
            _resourcesView.anchorMin = _cornerValue;
            _resourcesView.anchorMax = _cornerValue;
            _resourcesView.pivot = _cornerValue;

            if (isClear)
            {
                _resourcesView.transform.localPosition = Vector3.zero;
                Destroy(_resourceGroup.gameObject);
            }
            else
            {
                _resourcesView.transform.DOLocalMove(Vector3.zero, FadeDuration * 0.5f).SetEase(Ease.Linear);
            }
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
        }
    }
}
