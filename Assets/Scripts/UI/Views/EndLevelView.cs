using System;
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
        [SerializeField] private Button _buttonShowAdd;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private CanvasGroup _canvasGroupBG;
        
        [SerializeField] private Vector2 _centerPosition;
        [SerializeField] private Vector2 _cornerPosition;

        [SerializeField] private CanvasGroup _canvasGroupButtonAction;
        [SerializeField] private CanvasGroup _canvasGroupButtonShowAdd;
        
        public Button ButtonAction => _buttonAction;
        public Button ButtonShowAdd => _buttonShowAdd;

        protected override void Awake()
        {
            _canvasGroupButtonAction.alpha = 0f;
            _canvasGroupButtonShowAdd.alpha = 0f;
            
            _canvasGroupButtonAction.interactable = false;
            _canvasGroupButtonShowAdd.interactable = false;
            
            _canvasGroupButtonAction.blocksRaycasts = false;
            _canvasGroupButtonShowAdd.blocksRaycasts = false;
        }

        private void OnDestroy()
        {
            _buttonAction.onClick.RemoveAllListeners();
            _buttonShowAdd.onClick.RemoveAllListeners();
        }

        public void SetResourcesViewParent(RectTransform target, bool isCenter, float duration, Action callBack)
        {
            target.transform.SetParent(_rectTransform);
            
            target.anchorMin = isCenter ? _centerPosition : _cornerPosition;
            target.anchorMax = isCenter ? _centerPosition : _cornerPosition;
            target.pivot = isCenter ? _centerPosition : _cornerPosition;

            target.DOLocalMove(Vector3.zero, duration).SetEase(Ease.Linear).OnComplete(() =>
            {
                callBack?.Invoke();
                DOTween.Kill(target);
            });
        }

        public void SetHeader(string value)
        {
            _textHeader.SetText(value);
        }

        public override void SetVisible(bool isVisible, bool fastSet = false)
        {
            _canvasGroupBG.DOFade(1, FadeDuration).SetEase(Ease.Linear).OnComplete(() => DOTween.Kill(_canvasGroupBG));
        }

        public void ButtonActionSetVisible(bool enableShowAdButton)
        {
            _canvasGroupButtonAction.DOFade(1, FadeDuration).SetEase(Ease.Linear).OnComplete(() =>
            {
                DOTween.Kill(_canvasGroupButtonAction);
                
                if (enableShowAdButton)
                {
                    ButtonShowAddSetVisible();
                }
                else
                {
                    Debug.Log("Not has Listener");
                    _canvasGroupButtonAction.interactable = true;
                    _canvasGroupButtonAction.blocksRaycasts = true;
                }
            });
        }
        
        public void ButtonShowAddSetVisible()
        {
            _canvasGroupButtonShowAdd.DOFade(1, FadeDuration).SetEase(Ease.Linear).OnComplete(() =>
            {
                DOTween.Kill(_canvasGroupButtonShowAdd);
                _canvasGroupButtonShowAdd.interactable = true;
                _canvasGroupButtonShowAdd.blocksRaycasts = true;
                _canvasGroupButtonAction.interactable = true;
                _canvasGroupButtonAction.blocksRaycasts = true;
            });
        }
    }
}
