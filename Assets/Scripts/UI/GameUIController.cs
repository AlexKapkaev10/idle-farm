using System;
using Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using Scripts.Game;
using Scripts.Plants;
using TMPro;
using UnityEngine;

namespace Scripts.UI
{
    public class GameUIController : MonoBehaviour, IGameUIController
    {
        public event Action OnUIesReady;
        
        [SerializeField] private GameUISettings _settings;

        [SerializeField] private Joystick _joystick;
        [SerializeField] private TMP_Text _textWheatCount;
        [SerializeField] private TMP_Text _textMoneyCount;

        private readonly List<GameUI> _currentGameUIs = new List<GameUI>();
        
        private IResourceController _resourceController;

        private void Awake()
        {
            CreateGameUI();
        }

        private void CreateGameUI()
        {
            _currentGameUIs.Add(_joystick);
            
            foreach (var prefab in _settings.DefaultUIPrefabs)
            {
                var ui = Instantiate(prefab);
                _currentGameUIs.Add(ui);
            }
            
            OnUIesReady?.Invoke();
        }

        public void DisplayUI(bool value, GameUI exclude = null)
        {
            if (!value)
            {
                foreach (var ui in _currentGameUIs)
                {
                    if (exclude != null && ui == exclude)
                        continue;
                    
                    ui.CheckVisible();
                    ui.SetVisible(false);
                }
            }
            else
            {
                foreach (var ui in _currentGameUIs)
                {
                    if (ui.IsCheckVisible || exclude != null && ui == exclude)
                        continue;
                    
                    ui.SetVisible(true);
                }
            }
        }
        
        public T GetGameUIByType<T>() where T : GameUI
        {
            foreach (var gameUI in _currentGameUIs)
            {
                if (gameUI is T t)
                    return t;
            }
            return null;
        }
        
        public void SetResourceController(IResourceController resourceController)
        {
            _resourceController = resourceController;
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
            return;
            switch (plant.PlantType)
            {
                case PlantType.Wheat:
                    _textWheatCount.text = count.ToString();
                    break;
            }
        }

        public void DisplayMoneyCount(int from, int to)
        {
            return;
            StartCoroutine(TextCounterCoroutineMoney(_textMoneyCount, from, to));
        }

        public void DisplayByuPlants(int from, int to)
        {
            return;
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