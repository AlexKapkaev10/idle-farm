using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class EndLevelView : GameUI
    {
        [SerializeField] private RectTransform _resourcesViewParent;
        [SerializeField] private Button _buttonAction;
        [SerializeField] private Button _buttonShowAdd;
        [SerializeField] private CanvasGroup _canvasGroupBG;
        [SerializeField] private CanvasGroup _canvasGroupButtonAction;
        [SerializeField] private CanvasGroup _canvasGroupButtonShowAdd;
        
        [SerializeField] private TMP_Text _textRewardHeader;
        [SerializeField] private TMP_Text _textButtonReward;
        [SerializeField] private TMP_Text _textMoneyCounterPrefab;
        [SerializeField] private TMP_Text _textHeader;
        [SerializeField] private TMP_Text _textButtonNextAction;

        private TMP_Text _textMoneyCounter;
        
        public Button ButtonAction => _buttonAction;
        public Button ButtonShowAdd => _buttonShowAdd;

        protected override void Awake()
        {
            SetActiveCanvasGroup(_canvasGroupBG, false);
            SetActiveCanvasGroup(_canvasGroupButtonAction, false);
            SetActiveCanvasGroup(_canvasGroupButtonShowAdd, false);
        }

        public TMP_Text GetTextMoneyCounter()
        {
            return _textMoneyCounter 
                ? _textMoneyCounter 
                : _textMoneyCounter = Instantiate(_textMoneyCounterPrefab, _resourcesViewParent);
        }

        public void SetResourcesViewParent(RectTransform target, bool isCenter, float duration, Action callBack)
        {
            target.SetParent(_resourcesViewParent);
            
            StartCoroutine(MoveToAsync());
            
            IEnumerator MoveToAsync()
            {
                float t = 0f;
                var from = target.anchoredPosition;
                var to = Vector2.zero;
            
                while (t < 1)
                {
                    t += Time.unscaledDeltaTime / duration;
                
                    target.anchoredPosition = Vector2.Lerp(from, to, t);

                    yield return null;
                }
                yield return null;
                callBack?.Invoke();
            }
        }

        public void SetContent(bool isWin, in GameUISettings settings)
        {
            _textHeader.SetText(isWin ? settings.WinHeader : settings.LoseHeader);
            _textButtonNextAction.SetText(isWin ? settings.ButtonNextWinHeader : settings.ButtonNextLoseHeader);
            _textRewardHeader.SetText(isWin ? settings.WinRewardHeader : settings.LoseRewardHeader);
            _textButtonReward.SetText(isWin ? settings.ButtonRewardWinHeader : settings.ButtonRewardLoseHeader);
        }
        
        public override void SetVisible(bool isVisible, bool fastSet = false)
        {
            if (isVisible)
            {
                _canvasGroupBG.DOFade(1, FadeDuration * 0.5f).SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        SetActiveCanvasGroup(_canvasGroupBG, true);
                        DOTween.Kill(_canvasGroupBG);
                    });
            }
            else
            {
                _canvasGroupBG.interactable = false;
                _canvasGroupBG.blocksRaycasts = false;
                _canvasGroupBG.DOFade(0, FadeDuration * 0.5f).SetEase(Ease.Linear).OnComplete(() => Destroy(gameObject));
            }
        }

        public void ButtonShowAddSetVisible(bool enableShowAdButton)
        {
            if (enableShowAdButton)
            {
                _canvasGroupButtonShowAdd.DOFade(1, FadeDuration).SetEase(Ease.Linear).OnComplete(() =>
                {
                    DOTween.Kill(_canvasGroupButtonShowAdd);
                    
                    ButtonActionSetVisible();
                    SetActiveCanvasGroup(_canvasGroupButtonShowAdd, true);
                });
            }
            else
            {
                ButtonActionSetVisible();
            }
        }

        private void OnDestroy()
        {
            _buttonAction.onClick.RemoveAllListeners();
            _buttonShowAdd.onClick.RemoveAllListeners();
            DOTween.Kill(_canvasGroupBG);
        }

        private void ButtonActionSetVisible()
        {
            _canvasGroupButtonAction.DOFade(1, FadeDuration).SetEase(Ease.Linear).OnComplete(() =>
            {
                DOTween.Kill(_canvasGroupButtonAction);
                SetActiveCanvasGroup(_canvasGroupButtonAction, true);
            });
        }

        private void SetActiveCanvasGroup(in CanvasGroup canvasGroup, bool value)
        {
            canvasGroup.alpha = value ? 1f :  0f;
            canvasGroup.interactable = value;
            canvasGroup.blocksRaycasts = value;
        }
    }
}
