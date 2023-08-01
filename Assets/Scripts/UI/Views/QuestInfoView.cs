using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Scripts.UI
{
    public class QuestInfoView : GameUI
    {
        public event Action OnPlayClick;
        
        [SerializeField] private TMP_Text _textHeader;
        [SerializeField] private ResourceGroup _resourceGroup;
        [SerializeField] private float _fadeDurationTime = 0.2f;

        public ResourceGroup ResourceGroup => _resourceGroup;

        public void PlayClick()
        {
            OnPlayClick?.Invoke();
            Disable();
        }

        private void Disable()
        {
            _canvasGroup.DOFade(0, _fadeDurationTime).SetEase(Ease.Linear).OnComplete(()=> Destroy(gameObject));
        }

        private void OnDestroy()
        {
            DOTween.Kill(_canvasGroup);
            OnPlayClick = null;
        }

        public void SetHeader(string value)
        {
            _textHeader.SetText(value);
        }
    }
}