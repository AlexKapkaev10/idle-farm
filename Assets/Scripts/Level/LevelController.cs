using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Enums;
using Scripts.Game;
using Scripts.Interfaces;
using Scripts.Plants;
using Scripts.UI;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

namespace Scripts.Level
{
    public sealed class LevelController : MonoBehaviour, ILevelController
    {
        public event Action<bool> OnLevelComplete;
        private event Action onStartPlay;
        private event Action onGetReward;

        [SerializeField] private LevelControllerSettings _controllerSettings;

        private ICharacterController _characterController = default;
        private IBobController _bobController = default;
        private IGameUIController _gameUIController = default;
        private IResourceController _resourceController = default;

        private List<Level> _levelPrefabs;
        private Level _currentLevel = default;
        private List<QuestPlantData> _questPlantsData = default;
        private Dictionary<PlantType, int> _questMap;
        private Coroutine _updateTimerRoutine = default;

        private bool _isHurry = default;
        private int _currentBuyMoneyValue;
        private int _completeLevelsCount = 0;
        private float _questTime = 0.0f;


        [Inject]
        private void Construct(
            ICharacterController characterController,
            IBobController bobController,
            IGameUIController gameUIController, 
            IResourceController resourceController)
        {
            _characterController = characterController;
            _bobController = bobController;
            _gameUIController = gameUIController;
            _resourceController = resourceController;
        }
        
        private void Start()
        {
            SetLevelPrefabs();
            _completeLevelsCount = GameController.Instance.GetLevel();
            
            _gameUIController.DisplayMoneyCount(_resourceController.GetSaveMoney());
            
            _gameUIController.OnLevelPlay += InitializeLevel;
            _resourceController.OnAddPlant += AddResources;
            _resourceController.OnBuyPlants += BuyResources;
            _resourceController.OnResourceComplete += ResourceComplete;
            
            InitializeLevel();
        }

        private void OnDestroy()
        {
            _gameUIController.OnLevelPlay -= InitializeLevel;
            _resourceController.OnAddPlant -= AddResources;
            _resourceController.OnBuyPlants -= BuyResources;
            _resourceController.OnResourceComplete -= ResourceComplete;
            onGetReward = null;
            onStartPlay = null;
        }

        private void SetLevelPrefabs()
        {
            _levelPrefabs = _controllerSettings.LevelPrefabs;
        }

        private void InitializeLevel()
        {
            if (_currentLevel)
                ClearLevel();
            
            _currentLevel = Instantiate(_levelPrefabs[_completeLevelsCount < _levelPrefabs.Count
                ? _completeLevelsCount
                : Random.Range(0, _levelPrefabs.Count)]);
            
            _currentLevel.OnQuestReady += InitializeQuest;
            _currentLevel.OnFieldClear += FieldClear;
            _resourceController.QuestComplete += CheckQuestComplete;
            _currentLevel.Init(_characterController, _bobController);
        }

        private void InitializeQuest(LevelQuestData levelData)
        {
            _currentLevel.OnQuestReady -= InitializeQuest;

            onGetReward = null;
            _questTime = levelData.QuestTime;
            _questPlantsData = levelData.QuestPlantsData;
            
            _questMap = null;
            _questMap = new Dictionary<PlantType, int>();
            
            foreach (var plantData in _questPlantsData)
            {
                _questMap.Add(plantData.PlantType, plantData.Count);
            }
            
            _resourceController.SetQuestMap(_questMap);
            
            onStartPlay += StartTimer;
            _gameUIController.CreateQuestInfo(levelData, onStartPlay);
        }

        private void FieldClear(SowingField field)
        {
            _resourceController.CalculateQuestComplete(field);
        }

        private void CheckQuestComplete(bool isComplete)
        {
            if (isComplete)
            {
                _resourceController.QuestComplete -= CheckQuestComplete;
                _currentLevel.OnFieldClear -= FieldClear;
            }
            else
            {
                _bobController.SwitchAnimation(BobAnimationType.NotComplete);
            }
        }

        private void ResourceComplete(PlantType type)
        {
            _gameUIController.ResourceComplete(type);
        }

        private void ClearLevel()
        {
            GameController.Instance.AdController.ShowRewardedCount = 0;
            _currentLevel.OnFieldClear -= FieldClear;
            Destroy(_currentLevel.gameObject);
            _currentLevel = null;
        }
        
        private void BuyResources(BuyResourceData data)
        {
            EndLevel(data);
        }

        private void EndLevel(in BuyResourceData data)
        {
            if (_updateTimerRoutine != null)
            {
                StopCoroutine(_updateTimerRoutine);
                _updateTimerRoutine = null;
            }

            var isWin = data != null;

            if (isWin)
            {
                var hasNextLevel = GetHasNextLevel();
                _completeLevelsCount = hasNextLevel ? _completeLevelsCount +1 : _completeLevelsCount;
                
                if (hasNextLevel)
                    GameController.Instance.SaveLevelProgress(_completeLevelsCount);
            }
            
            _bobController.SwitchAnimation(isWin ? BobAnimationType.Win : BobAnimationType.Lose);
            _characterController.EndLevel(isWin);
            _currentBuyMoneyValue = isWin ? data.addMoneyValue : 0;
            var canShowAdd = GameController.Instance.AdController.ShowRewardedCount < 1;

            if (isWin)
                onGetReward += MultiplyMoneyForReward;
            else if (canShowAdd)
                onGetReward += AddQuestTimeForReward;
            else
                onGetReward = null;

            _gameUIController.CreateEndLevelView(data, onGetReward);
            OnLevelComplete?.Invoke(isWin);
        }

        private bool GetHasNextLevel()
        {
            return _completeLevelsCount < _levelPrefabs.Count;
        }

        private void AddQuestTimeForReward()
        {
            onGetReward -= AddQuestTimeForReward;
            _questTime = _controllerSettings.AddTimeCount;
            StartTimer();
        }

        private void MultiplyMoneyForReward()
        {
            _resourceController.SetMoneyForReward(_currentBuyMoneyValue);
            _currentBuyMoneyValue = 0;
            onGetReward -= MultiplyMoneyForReward;
        }

        private void AddResources(PlantBlock plantBlock, int count)
        {
            _gameUIController.DisplayPlantCount(plantBlock, count);
        }

        private void StartTimer()
        {
            _isHurry = false;
            onStartPlay -= StartTimer;
            
            if (_updateTimerRoutine != null)
            {
                StopCoroutine(_updateTimerRoutine);
                _updateTimerRoutine = null;
            }
            _updateTimerRoutine = StartCoroutine(UpdateTimer());
        }

        private IEnumerator UpdateTimer()
        {
            _characterController.StartLevel();
            while (_questTime > 0.9f)
            {
                _questTime -= Time.deltaTime;
                float minutes = Mathf.FloorToInt(_questTime / 60);
                float seconds = Mathf.FloorToInt(_questTime % 60);
                var time = $"{minutes:00}:{seconds:00}";
                _gameUIController.DisplayTimer(time);
                
                if (_questTime < 6f && !_isHurry)
                {
                    _isHurry = true;
                    _gameUIController.UpdateTimerStyle(false);
                }
                
                yield return null;
            }
            
            EndLevel(null);
            _updateTimerRoutine = null;
        }
    }
}