using System;
using Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using Scripts.Game;
using Scripts.Level;
using Scripts.Plants;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts.UI
{
    public class GameUIController : MonoBehaviour, IGameUIController
    {
        public event Action OnLevelPlay;
        public event Action<Joystick> OnJoystickCreate;

        [SerializeField] private GameUISettings _settings;
        [SerializeField] private GameInfoView _gameInfoView;

        private event Action onEndLevelAnimation;
        private event Action OnShowAd;
        private List<GameUI> _currentGameUIs = new List<GameUI>();
        private Dictionary<PlantType, ResourceView> _resourceViewMap = new Dictionary<PlantType, ResourceView>();

        private BuyResourceData _buyResourceData;
        private Joystick _joystick;
        private QuestInfoView _questInfoView;
        private EndLevelView _endLevelView;
        private ResourceGroup _resourceGroup;
        private WaitForSeconds _displayEndLevelDelay;
        private WaitForSeconds _moneyCounterDelay;

        private int _moneyMultiplyValue;

        public void DisplayPlantCount(PlantBlock plantBlock, int count)
        {
            _resourceViewMap[plantBlock.PlantType].UpdateProgressCount(count);
        }

        public void DisplayTimer(string textTimer)
        {
            _gameInfoView.SetTimerText(textTimer);
        }

        public void UpdateTimerStyle(bool isDefault)
        {
            _gameInfoView.UpdateTextColor(isDefault);
        }

        public void DisplayMoneyCount(int value)
        {
            _gameInfoView.TextMoney.SetText($"{value.ToString()}$");
        }

        public void CreateQuestInfo(LevelQuestData levelQuestData, Action callBack)
        {
            _gameInfoView.SetVisible(false, true);
            _questInfoView = Instantiate(_settings.QuestInfoView, transform);
            _currentGameUIs.Add(_questInfoView);
            _questInfoView.SetHeader(_settings.QuestHeaders[Random.Range(0, _settings.QuestHeaders.Length - 1)]);

            _questInfoView.OnPlayClick += () =>
            {
                _currentGameUIs.Remove(_questInfoView);
                _resourceGroup = _questInfoView.ResourceGroup;
                _gameInfoView.SetResourceGroup(_resourceGroup);
                CreateJoystick();
                callBack?.Invoke();
            };

            foreach (var data in levelQuestData.QuestPlantsData)
            {
                var resourceView = Instantiate(_settings.ResourceViewPrefab, _questInfoView.ResourceGroup.RectTransform);
                resourceView.Init(data.Count, _settings.GetSpriteByPlantType(data.PlantType));
                _questInfoView.ResourceGroup.AddResourceView(resourceView);
                _resourceViewMap.Add(data.PlantType, resourceView);
            }
        }

        public void CreateEndLevelView(BuyResourceData data, Action callBack)
        {
            _buyResourceData = data;
            var isWin = _buyResourceData != null;

            DestroyJoystick();
            StartCoroutine(DisplayWinLoseViewAsync());

            IEnumerator DisplayWinLoseViewAsync()
            {
                yield return _displayEndLevelDelay;
                
                _endLevelView = Instantiate(_settings.EndLevelPrefab, transform) as EndLevelView;
                _currentGameUIs.Add(_endLevelView);

                if (!_endLevelView)
                    yield break;

                _endLevelView.SetContent(isWin, _settings);
                _endLevelView.ButtonAction.onClick.AddListener(PlayLevelClick);
                _endLevelView.FadeDuration = _settings.FadeDurationView;
                
                onEndLevelAnimation += EndLevelAnimations;

                _endLevelView.SetVisible(true);
                _gameInfoView.SetVisible(false);
                
                OnShowAd = callBack;

                if (OnShowAd != null)
                    _endLevelView?.ButtonShowAdd.onClick.AddListener(isWin ? MultiplyMoneyClick : AddTimeButtonClick);
                
                if (isWin)
                {
                    _gameInfoView.SetMoneyTextParent(_endLevelView?.transform);
                    
                    _endLevelView?.SetResourcesViewParent(
                        _gameInfoView.ResourcesView,
                        true,
                        _settings.FadeDurationView,
                        onEndLevelAnimation);
                }
                else
                {
                    onEndLevelAnimation?.Invoke();
                }
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
            _moneyCounterDelay = new WaitForSeconds(_settings.MoneyCounterTimeDelay);
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
                StartCoroutine(WaitForMoneyCounter());

                foreach (var buyResource in _buyResourceData.BuyResources)
                {
                    var view = _resourceViewMap[buyResource.PlantType];
                    view.StartCoroutine(ResourceCounterAsync(view, buyResource.Count, 0, _settings.FadeDurationView));
                }
            }
            else
            {
                _endLevelView.ButtonShowAddSetVisible(OnShowAd != null);
            }

            IEnumerator WaitForMoneyCounter()
            {
                moneyCounterRoutine = StartCoroutine(MoneyCounterAsync(
                    _endLevelView.GetTextMoneyCounter(),
                    0, 
                    _buyResourceData.addMoneyValue,
                    _settings.FadeDurationView,
                    "+",
                    "$"));
               
               yield return moneyCounterRoutine;
               
               if (_resourceGroup)
                   Destroy(_resourceGroup.gameObject);
            
               _endLevelView.ButtonShowAddSetVisible(OnShowAd != null);
               moneyCounterRoutine = null;
            }
        }

        private void MultiplyMoneyClick()
        {
            Debug.Log("Start Show Ad");
            _endLevelView?.ButtonShowAdd.gameObject.SetActive(false);
            _moneyMultiplyValue = 2;
            GameController.Instance.AdController.ShowRewardedVideo();
            GameController.Instance.AdController.OnRewardedShow += DisplayMultiPlayMoney;
        }

        private void DisplayMultiPlayMoney()
        {
            GameController.Instance.AdController.OnRewardedShow -= DisplayMultiPlayMoney;
            
            Coroutine moneyCounterRoutine = null;

            StartCoroutine(WaitForMoneyCounter());

            IEnumerator WaitForMoneyCounter()
            {
                int multiplyMoneyCount = _buyResourceData.addMoneyValue * _moneyMultiplyValue;
                TMP_Text textMultiplyMoney = _endLevelView.GetTextMoneyCounter();
                moneyCounterRoutine = StartCoroutine(MoneyCounterAsync(
                    textMultiplyMoney,
                    _buyResourceData.addMoneyValue,
                    multiplyMoneyCount,
                    _settings.FadeDurationView,
                    "+",
                    "$"));

                yield return moneyCounterRoutine;

                OnShowAd?.Invoke();
                OnShowAd = null;

                yield return _moneyCounterDelay;

                moneyCounterRoutine = null;
                int money = GameController.Instance.GetMoney();
                moneyCounterRoutine = StartCoroutine(MoneyCounterAsync(
                    _gameInfoView.TextMoney,
                    money - multiplyMoneyCount,
                    money,
                    _settings.FadeDurationView,
                    null,
                    "$"
                ));
                
                moneyCounterRoutine = StartCoroutine(MoneyCounterAsync(
                    textMultiplyMoney,
                    multiplyMoneyCount,
                    0,
                    _settings.FadeDurationView,
                    "+",
                    "$"));
                
                yield return moneyCounterRoutine;
                Destroy(textMultiplyMoney.gameObject);

                yield return _moneyCounterDelay;
                
                NextActionClick(true);
            }
        }

        private void AddTimeButtonClick()
        {
            GameController.Instance.AdController.ShowRewardedVideo();
            Debug.Log("Start Show Ad");
            GameController.Instance.AdController.OnRewardedShow += () =>
            {
                OnShowAd?.Invoke();
                OnShowAd = null;
                
                NextActionClick(false);
            };
        }

        private void PlayLevelClick()
        {
            if (_buyResourceData != null)
            {
                Coroutine moneyCounterRoutine = null;
                StartCoroutine(WaitForMoneyCounter());
                IEnumerator WaitForMoneyCounter()
                {
                    TMP_Text textMultiplyMoney = _endLevelView.GetTextMoneyCounter();
                    moneyCounterRoutine = null;
                    int money = GameController.Instance.GetMoney();
                    moneyCounterRoutine = StartCoroutine(MoneyCounterAsync(
                        _gameInfoView.TextMoney,
                        money - _buyResourceData.addMoneyValue,
                        money,
                        _settings.FadeDurationView,
                        null,
                        "$"
                    ));
                
                    moneyCounterRoutine = StartCoroutine(MoneyCounterAsync(
                        textMultiplyMoney,
                        _buyResourceData.addMoneyValue,
                        0,
                        _settings.FadeDurationView,
                        "+",
                        "$"));
                
                    yield return moneyCounterRoutine;
                    Destroy(textMultiplyMoney.gameObject);

                    yield return _moneyCounterDelay;
                
                    NextActionClick(true);
                }
            }
            else
            {
                NextActionClick(true);
            }

        }

        private void NextActionClick(bool isLevelNext)
        {
            if (isLevelNext)
            {
                if (_resourceGroup)
                    Destroy(_resourceGroup.gameObject);
                
                _gameInfoView.SetMoneyTextParent(null);
                _gameInfoView.ReturnResourcesView(true);
                _resourceViewMap.Clear();
                OnLevelPlay?.Invoke();
            }
            else
            {
                //_gameInfoView.ReturnResourcesView();
                _gameInfoView.SetVisible(true);
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

        private IEnumerator ResourceCounterAsync(ResourceView view, int from, int to , float time = 1f, string additionalText = null)
        {
            float t = 0f;
            while (t < 1)
            {
                t += Time.unscaledDeltaTime / time;
                view.UpdateProgressCount((int)Mathf.Lerp(from, to, t));
                yield return null;
            }
        }
        
        private IEnumerator MoneyCounterAsync(TMP_Text text, int from, int to , float time = 1f, string beginText = null, string finalText = null)
        {
            float t = 0f;
            while (t < 1)
            {
                t += Time.unscaledDeltaTime / time;
                text.SetText($"{beginText}{Mathf.Lerp(from, to, t):0}{finalText}");
                yield return null;
            }

            yield return null;
        }
    }
}