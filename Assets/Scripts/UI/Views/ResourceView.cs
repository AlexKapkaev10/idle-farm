using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class ResourceView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _textCount;
        [SerializeField] private Image _imageIcon;

        private string _questCount;

        public void Init(string questCount, Sprite icon)
        {
            _questCount = questCount;
            _imageIcon.sprite = icon;
            _textCount.SetText(questCount);
        }

        public void UpdateProgressCount(string value)
        {
            string progress = $"{value}/{_questCount}";
            _textCount.SetText(progress);
        }

    }
}
