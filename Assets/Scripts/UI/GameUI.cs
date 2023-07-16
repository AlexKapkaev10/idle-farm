using System;
using Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using Scripts.Game;
using Scripts.Plants;
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
            _resourceController.TryGetMoney(10, OnBuy);
        }

        private void OnBuy(bool isBuy)
        {
            Debug.Log($"Money is enough: {isBuy}");
        }
        
        public Joystick GetJoystick()
        {
            return _joystick;
        }

        public void DisplayPlantCount(in Plant plant, int count)
        {
            switch (plant.PlantType)
            {
                case PlantType.Wheat:
                    _textWheatCount.text = count.ToString();
                    break;
            }
        }

        public void DisplayMoneyCount(int from, int to)
        {
            StartCoroutine(TextCounterCoroutineMoney(_textMoneyCount, from, to));
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
                text.SetText(Mathf.Lerp(from, to, t).ToString("0"));
                yield return null;
            }
        }
        
        private IEnumerator TextCounterCoroutineMoney(TMP_Text text, int from, int to , float time = 1f, string additionalText = null)
        {
            float t = 0f;
            while (t < 1)
            {
                t += Time.unscaledDeltaTime / time;
                text.SetText(Mathf.Lerp(from, to, t).ToString("0"));
                yield return null;
            }
        }
    }
}