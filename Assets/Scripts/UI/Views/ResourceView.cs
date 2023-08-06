using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class ResourceView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _textCount;
        [SerializeField] private Image _imageIcon;
        [SerializeField] private Color _colorComplete;
        [SerializeField] private Image _sliderImage;

        private int _questCount;

        public void Init(int questCount, Sprite icon)
        {
            _questCount = questCount;
            _imageIcon.sprite = icon;
            SetSliderProgress(1);
            _textCount.SetText(questCount.ToString());
        }

        public void SetSliderProgress(float value)
        {
            _sliderImage.fillAmount = value;
        }

        public void UpdateProgressCount(int value)
        {
            var progress = $"{value:0}/{_questCount.ToString()}";
            _textCount.SetText(progress);

            if (value > _questCount)
                return;
            var percentage = value / (float)_questCount;
            SetSliderProgress(percentage);
        }

        public void SetColorComplete()
        {
            _textCount.color = _colorComplete;
        }

    }
}
