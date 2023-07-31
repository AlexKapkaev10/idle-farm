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

        public Button ButtonAction => _buttonAction;
        public Button ButtonAddTime => _buttonAddTime;

        public void SetHeader(string value)
        {
            _textHeader.SetText(value);
        }
    }
}
