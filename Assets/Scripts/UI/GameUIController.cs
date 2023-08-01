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
        [SerializeField] private RectTransform _resourceGroupParent;

        [SerializeField] private TMP_Text _textMoneyCount;
        [SerializeField] private TMP_Text _textTimer;

        private readonly List<GameUI> _currentGameUIs = new List<GameUI>();
        private Dictionary<PlantType, ResourceView> _resourceViewMap;

        private event Action OnAddTimeClick;
        private Joystick _joystick;
        private QuestInfoView _questInfoView;
        private WinLoseView _winLoseView;
        private ResourceGroup _resourceGroup;

        public void DisplayPlantCount(PlantBlock plantBlock, int count)
        {
            _resourceViewMap[plantBlock.PlantType].UpdateProgressCount(count.ToString());
        }
        
        public void DisplayMoneyCount(int from, int to)
        {
            StartCoroutine(TextCounterCoroutineMoney(_textMoneyCount, from, to));
        }

        public void DisplayByuPlants(PlantType type, int from)
        {
            StartCoroutine(TextCounterCoroutine(_resourceViewMap[type], from, 0));
        }

        public void DisplayTimer(string textTimer)
        {
            _textTimer.SetText(textTimer);
        }

        public void ChangeTimerStyle(bool isChange)
        {
            _textTimer.color = isChange ? _settings.TimerColorHurry : _settings.TimerColorDefault;
        }

        public void CreateWinLoseView(bool isWin, Action callBack)
        {
            DestroyJoystick();
            _winLoseView = Instantiate(_settings.WinLoseViewPrefab, transform) as WinLoseView;
            _winLoseView?.SetHeader(isWin ? _settings.WinHeader : _settings.LoseHeader);
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

        public void SetQuestInfo(LevelQuestData levelQuestData, Action callBack)
        {
            _questInfoView = Instantiate(_settings.QuestInfoView, transform);
            
            _questInfoView.OnPlayClick += () =>
            {
                callBack?.Invoke();
                _resourceGroup = _questInfoView.ResourceGroup;
                _resourceGroup.RectTransform.SetParent(_resourceGroupParent);
                _resourceGroup.ChangeRectPosition(false);
                CreateJoystick();
            };
            
            if (_resourceViewMap == null)
                _resourceViewMap = new Dictionary<PlantType, ResourceView>();
            
            foreach (var data in levelQuestData.QuestPlantsData)
            {
                var resourceView = Instantiate(_settings.ResourceViewPrefab, _questInfoView.ResourceGroup.RectTransform);
                resourceView.Init(data.Count.ToString(), _settings.GetSpriteByPlantType(data.PlantType));
                _questInfoView.ResourceGroup.AddResourceView(resourceView);
                _resourceViewMap.Add(data.PlantType, resourceView);
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

        private void OnDestroy()
        {
            OnJoystickCreate = null;
        }

        private void AddTimeButtonClick()
        {
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