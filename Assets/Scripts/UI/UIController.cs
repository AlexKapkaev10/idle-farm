using Scripts.Enums;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Scripts.UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField]
        private Joystick _joystick;
        [SerializeField]
        private TMP_Text _textWheatCount;

        public Joystick GetJoystick()
        {
            return _joystick;
        }

        public void DisplayWheatCount(PlantType plantType, int count)
        {
            switch (plantType)
            {
                case PlantType.Wheat:
                    _textWheatCount.text = count.ToString();
                    break;
            }
        }

        public void DisplayByuPlants(int from, int to)
        {
            StartCoroutine(TextCounterCoroutine(_textWheatCount, from, to));
        }

        private IEnumerator TextCounterCoroutine(TMP_Text text, int from, int to , float time = 1f, string additionalText = null)
        {
            float t = 0f;
            while (t < 1)
            {
                t += Time.unscaledDeltaTime / time;
                text.text = Mathf.Lerp(from, to, t).ToString("0");
                yield return null;
            }
        }
    }
}