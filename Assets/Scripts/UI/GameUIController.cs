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
    public class GameUIController : MonoBehaviour, IGameUIController
    {
        public event Action OnUIesReady;
        
        [SerializeField] private GameUISettings _settings;
        [SerializeField] private Joystick _joystick;
        
        [SerializeField] private TMP_Text _textPeasCount;
        [SerializeField] private TMP_Text _textWheatCount;
        [SerializeField] private TMP_Text _textMoneyCount;
        [SerializeField] private TMP_Text _textTimer;

        private Dictionary<PlantType, TMP_Text> _textsByPlantsType;

        private readonly List<GameUI> _currentGameUIs = new List<GameUI>();

        [Inject]
        private void Construct()
        {
            
        }

        public void DisplayPlantCount(PlantBlock plantBlock, int count)
        {
            _textsByPlantsType[plantBlock.PlantType].SetText(count.ToString());
        }
        
        public void DisplayMoneyCount(int from, int to)
        {
            StartCoroutine(TextCounterCoroutineMoney(_textMoneyCount, from, to));
        }

        public void DisplayByuPlants(PlantType type, int from)
        {
            StartCoroutine(TextCounterCoroutine(_textsByPlantsType[type], from, 0));
        }

        public void DisplayTimer(string textTimer)
        {
            _textTimer.SetText(textTimer);
        }

        public void ChangeTimer(bool isChange)
        {
            _textTimer.color = isChange ? _settings.TimerColorHurry : _settings.TimerColorDefault;
        }

        public Joystick GetJoystick()
        {
            return _joystick;
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

        private void OnBuy(bool isBuy)
        {
            Debug.Log($"Money is enough: {isBuy}");
        }

        private void Awake()
        {
            CreateGameUI();

            _textsByPlantsType = new Dictionary<PlantType, TMP_Text>
            {
                [PlantType.Peas] = _textPeasCount,
                [PlantType.Wheat] = _textWheatCount
            };
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