using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class QuestInfoView : MonoBehaviour
    {
        public event Action OnPlayClick;
        [SerializeField] private TMP_Text _textHeader;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Button _buttonPlay;

        public RectTransform RectTransform => _rectTransform;
        public Button ButtonPlay => _buttonPlay;

        public void PlayClick()
        {
            OnPlayClick?.Invoke();
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            OnPlayClick = null;
        }

        public void SetHeader(string value)
        {
            _textHeader.SetText(value);
        }
    }
}