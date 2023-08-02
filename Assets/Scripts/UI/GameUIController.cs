using System;
using Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using Scripts.Level;
using Scripts.Plants;
using TMPro;
using UnityEngine;

namespace Scripts.UI
{
    public class GameUIController : MonoBehaviour, IGameUIController
    {
        public event Action OnLevelPlay;
        public event Action<Joystick> OnJoystickCreate;

        [SerializeField] private GameUISettings _settings;
        [SerializeField] private GameInfoView _gameInfoView;

        private readonly List<GameUI> _currentGameUIs = new List<GameUI>();
        private Dictionary<PlantType, ResourceView> _resourceViewMap;

        private event Action OnAddTimeClick;
        private Joystick _joystick;
        private QuestInfoView _questInfoView;
        private WinLoseView _winLoseView;
        private ResourceGroup _resourceGroup;
        private WaitForSeconds _displayWinLoseDelay;

        public void DisplayPlantCount(PlantBlock plantBlock, int count)
        {
            _resourceViewMap[plantBlock.PlantType].UpdateProgressCount(count.ToString());
        }
        
        public void DisplayMoneyCount(int from, int to)
        {
            StartCoroutine(TextCounterCoroutineMoney(_gameInfoView.TextMoney, from, to));
        }

        public void DisplayByuPlants(PlantType type, int from)
        {
            var view = _resourceViewMap[type];
            view.StartCoroutine(TextCounterCoroutine(view, from, 0));
        }

        public void DisplayTimer(string textTimer)
        {
            _gameInfoView.SetTimerText(textTimer);
        }

        public void UpdateTimerStyle(bool isDefault)
        {
            _gameInfoView.UpdateTextColor(isDefault);
        }

        public void CreateWinLoseView(bool isWin, Action callBack)
        {
            DestroyJoystick();
            StartCoroutine(DisplayWinLoseViewAsync());
            
            IEnumerator DisplayWinLoseViewAsync()
            {
                yield return _displayWinLoseDelay;
                _winLoseView = Instantiate(_settings.WinLoseViewPrefab, transform) as WinLoseView;
                _winLoseView?.SetHeader(isWin ? _settings.WinHeader : _settings.LoseHeader);

                _gameInfoView.SetVisible(false);
                _winLoseView?.ButtonAction.onClick.AddListener(PlayLevelClick);
            
                if (callBack != null)
                {
                    OnAddTimeClick = callBack;
                    _winLoseView?.ButtonAddTime.onClick.AddListener(AddTimeButtonClick);
                }
                else
                {
                    _winLoseView?.ButtonAddTime.onClick.RemoveAllListeners();
                }
            }
        }

        public void CreateQuestInfo(LevelQuestData levelQuestData, Action callBack)
        {
            _gameInfoView.SetVisible(false, true);
            _questInfoView = Instantiate(_settings.QuestInfoView, transform);
            
            _questInfoView.OnPlayClick += () =>
            {
                _resourceGroup = _questInfoView.ResourceGroup;
                _gameInfoView.SetResourceGroup(_resourceGroup);
                CreateJoystick();
                callBack?.Invoke();
            };
            
            _resourceViewMap ??= new Dictionary<PlantType, ResourceView>();
            
            foreach (var data in levelQuestData.QuestPlantsData)
            {
                var resourceView = Instantiate(_settings.ResourceViewPrefab, _questInfoView.ResourceGroup.RectTransform);
                resourceView.Init(data.Count.ToString(), _settings.GetSpriteByPlantType(data.PlantType));
                _questInfoView.ResourceGroup.AddResourceView(resourceView);
                _resourceViewMap.Add(data.PlantType, resourceView);
            }
        }

        public void ResourceComplete(PlantType type)
        {
            _resourceViewMap[type].SetColorComplete();
        }

        /*public T GetGameUIByType<T>() where T : GameUI
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
        }*/

        private void Awake()
        {
            _displayWinLoseDelay = new WaitForSeconds(_settings.DisplayWinLoseTime);
        }

        private void OnDestroy()
        {
            OnJoystickCreate = null;
        }

        private void AddTimeButtonClick()
        {
            _gameInfoView.SetVisible(true);
            CreateJoystick();
            Destroy(_winLoseView.gameObject);
            OnAddTimeClick?.Invoke();
        }

        private void PlayLevelClick()
        {
            Clear();
            OnLevelPlay?.Invoke();
            Destroy(_winLoseView.gameObject);
        }

        private void Clear()
        {
            if (_resourceGroup)
                Destroy(_resourceGroup.gameObject);
            
            _resourceViewMap.Clear();
        }

        private void OnBuy(bool isBuy)
        {
            Debug.Log($"Money is enough: {isBuy}");
        }

        private void CreateJoystick()
        {
            _joystick = Instantiate(_settings.JoystickPrefab, transform);
            OnJoystickCreate?.Invoke(_joystick);
            _currentGameUIs.Add(_joystick);
        }

        private void DestroyJoystick()
        {
            _currentGameUIs.Remove(_joystick);
            Destroy(_joystick.gameObject);
            OnJoystickCreate?.Invoke(null);
        }

        private IEnumerator TextCounterCoroutine(ResourceView view, int from, int to , float time = 1f, string additionalText = null)
        {
            float t = 0f;
            while (t < 1)
            {
                t += Time.unscaledDeltaTime / time;
                view.UpdateProgressCount(Mathf.Lerp(from, to, t).ToString("0"));
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