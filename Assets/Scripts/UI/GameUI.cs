using System;
using Scripts.Enums;
using System.Collections;
using Scripts.Game;
using TMPro;
using UnityEngine;
using VContainer;

namespace Scripts.UI
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private Joystick _joystick;
        [SerializeField] private TMP_Text _textWheatCount;
        [SerializeField] private TMP_Text _textMoneyCount;

        private ResourceController _resourceController;
        
        public void SetResourceController(ResourceController resourceController)
        {
            _resourceController = resourceController;
        }

        public void TryGetMoney()
        {
            _resourceController.TryGetMoney(10, () =>  Debug.Log("Money is not enouth"));
        }
        
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

        public void DisplayMoneyCount(int value)
        {
            _textMoneyCount.SetText(value.ToString());
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