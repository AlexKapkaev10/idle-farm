using System;
using Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using Scripts.Game;
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

        private event Action onEndLevelAnimation;
        
        [SerializeField] private List<GameUI> _currentGameUIs = new List<GameUI>();
        private Dictionary<PlantType, ResourceView> _resourceViewMap;

        private event Action OnShowAd;
        private BuyResourceData _buyResourceData;
        private Joystick _joystick;
        private QuestInfoView _questInfoView;
        private EndLevelView _endLevelView;
        private ResourceGroup _resourceGroup;
        private WaitForSeconds _displayEndLevelDelay;

        public void DisplayPlantCount(PlantBlock plantBlock, int count)
        {
            _resourceViewMap[plantBlock.PlantType].UpdateProgressCount(count.ToString());
        }

        public void DisplayTimer(string textTimer)
        {
            _gameInfoView.SetTimerText(textTimer);
        }

        public void UpdateTimerStyle(bool isDefault)
        {
            _gameInfoView.UpdateTextColor(isDefault);
        }

        public void CreateEndLevelView(BuyResourceData data, Action callBack)
        {
            _buyResourceData = data;

            DestroyJoystick();
            StartCoroutine(DisplayWinLoseViewAsync());
            
            IEnumerator DisplayWinLoseViewAsync()
            {
                yield return _displayEndLevelDelay;
                
                _endLevelView = Instantiate(_settings.EndLevelPrefab, transform) as EndLevelView;
                _currentGameUIs.Add(_endLevelView);

                if (!_endLevelView)
                    yield break;

                _endLevelView.SetHeader(_buyResourceData != null ? _settings.WinHeader : _settings.LoseHeader);
                _endLevelView.ButtonAction.onClick.AddListener(PlayLevelClick);

                _endLevelView.FadeDuration = _settings.FadeDurationView;

                onEndLevelAnimation += EndLevelAnimations;
                
                _endLevelView.SetResourcesViewParent(
                    _gameInfoView.ResourcesView, 
                    true, 
                    _settings.FadeDurationView, 
                    onEndLevelAnimation);
                
                _endLevelView.SetVisible(true);
                _gameInfoView.SetVisible(false);
                OnShowAd = callBack;

                if (OnShowAd != null)
                    _endLevelView?.ButtonShowAdd.onClick.AddListener(_buyResourceData != null ? MultiplyMoneyClick : AddTimeButtonClick);
            }
        }

        public void CreateQuestInfo(LevelQuestData levelQuestData, Action callBack)
        {
            _gameInfoView.SetVisible(false, true);
            _questInfoView = Instantiate(_settings.QuestInfoView, transform);
            _currentGameUIs.Add(_questInfoView);

            _questInfoView.OnPlayClick += () =>
            {
                _currentGameUIs.Remove(_questInfoView);
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

        private void Start()
        {
            _displayEndLevelDelay = new WaitForSeconds(_settings.DisplayWinLoseTime);
            _gameInfoView.FadeDuration = _settings.FadeDurationView;
            _currentGameUIs.Add(_gameInfoView);
        }

        private void OnDestroy()
        {
            OnJoystickCreate = null;
        }

        private void EndLevelAnimations()
        {
            onEndLevelAnimation -= EndLevelAnimations;
            Coroutine moneyCounterRoutine = null;
            
            if (_buyResourceData != null)
            {
                _gameInfoView.ScaleMoneyView(2f, _settings.FadeDurationView);

                StartCoroutine(WaitForMoneyCounter());

                foreach (var buyResource in _buyResourceData.BuyResources)
                {
                    var view = _resourceViewMap[buyResource.PlantType];
                    view.StartCoroutine(TextCounterCoroutine(view, buyResource.Count, 0, _settings.FadeDurationView));
                }
            }
            else
            {
                _endLevelView.ButtonActionSetVisible(OnShowAd != null);
            }

            IEnumerator WaitForMoneyCounter()
            {
               moneyCounterRoutine = StartCoroutine(TextCounterCoroutineMoney(
                    _gameInfoView.TextMoney,
                    _buyResourceData.oldMoneyValue, 
                    _buyResourceData.newMoneyValue,
                    _settings.FadeDurationView));
               
               yield return moneyCounterRoutine;
               if (_resourceGroup)
                   Destroy(_resourceGroup.gameObject);
            
               _endLevelView.ButtonActionSetVisible(OnShowAd != null);
               moneyCounterRoutine = null;
            }
        }

        private void MultiplyMoneyClick()
        {
            Debug.Log("Start Show Ad");
            GameController.Instance.AdController.ShowRewardedVideo();
            
            GameController.Instance.AdController.OnRewardedShow += () =>
            {
                OnShowAd?.Invoke();
                OnShowAd = null;
            };
        }

        private void AddTimeButtonClick()
        {
            GameController.Instance.AdController.ShowRewardedVideo();
            Debug.Log("Start Show Ad");
            GameController.Instance.AdController.OnRewardedShow += () =>
            {
                NextActionClick(false);
            };
        }

        private void PlayLevelClick()
        {
            NextActionClick(true);
        }

        private void NextActionClick(bool isLevelNext)
        {
            if (isLevelNext)
            {
                if (_resourceGroup)
                    Destroy(_resourceGroup.gameObject);
                
                _gameInfoView.ReturnResourcesView(true);
                _resourceViewMap.Clear();
                _gameInfoView.ScaleMoneyView(1, 0);
                OnLevelPlay?.Invoke();
            }
            else
            {
                _gameInfoView.ReturnResourcesView();
                _gameInfoView.SetVisible(true);
                OnShowAd?.Invoke();
                OnShowAd = null;
                CreateJoystick();
            }

            _currentGameUIs.Remove(_endLevelView);
            Destroy(_endLevelView.gameObject);
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

            yield return null;
        }


    }
}