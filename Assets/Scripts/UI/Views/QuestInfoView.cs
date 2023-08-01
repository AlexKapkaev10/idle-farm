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
        [SerializeField] private ResourceGroup _resourceGroup;
        [SerializeField] private Button _buttonPlay;
        
        public ResourceGroup ResourceGroup => _resourceGroup;
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