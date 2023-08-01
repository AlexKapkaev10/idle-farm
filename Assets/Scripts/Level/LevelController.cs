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
        private event Action onStartPlay;
        private event Action onAddQuestTime;

        [SerializeField] private LevelControllerSettings _controllerSettings;

        private ICharacterController _characterController = default;
        private IGameUIController _gameUIController = default;
        private IResourceController _resourceController = default;
        
        private Level _currentLevel = default;
        private List<QuestPlantData> _questPlantsData = default;

        private Coroutine _updateTimerRoutine;
        private readonly bool _isHurry = default;
        
        private int _levelsComplete = 0;
        private float _questTime = 0.0f;

        private List<Level> _levelPrefabs;
        private Dictionary<PlantType, int> _questMap; 

        [Inject]
        private void Construct(ICharacterController characterController, IGameUIController gameUIController, IResourceController resourceController)
        {
            _characterController = characterController;
            _gameUIController = gameUIController;
            _resourceController = resourceController;
            
            _levelsComplete = SaveLoadService.Instance.Level;
            _gameUIController.OnLevelPlay += InitializeLevel;
        }

        private void Start()
        {
            Application.targetFrameRate = 100;
            _levelPrefabs = _controllerSettings.LevelPrefabs;
            InitializeLevel();
        }

        private void OnDestroy()
        {
            _gameUIController.OnLevelPlay -= InitializeLevel;
        }

        private void InitializeLevel()
        {
            if (_currentLevel)
                ClearLevel();
            
            _resourceController.OnAddPlant += AddResources;
            _resourceController.OnBuyPlants += BuyResources;
            _resourceController.OnChangeMoney += MoneyChange;

            var index = _levelsComplete > (_levelPrefabs.Count - 1)
                ? Random.Range(0, _levelPrefabs.Count)
                : _levelsComplete;

            _currentLevel = Instantiate(_levelPrefabs[index]);
            _currentLevel.OnQuestReady += InitializeQuest;
            _currentLevel.Init(_characterController);
        }

        private void ClearLevel()
        {
            _resourceController.OnAddPlant -= AddResources;
            _resourceController.OnBuyPlants -= BuyResources;
            _resourceController.OnChangeMoney -= MoneyChange;
            _gameUIController.ChangeTimerStyle(false);
            Destroy(_currentLevel.gameObject);
            _currentLevel = null;
        }

        private void MoneyChange(int oldValue, int newValue)
        {
            _gameUIController.DisplayMoneyCount(oldValue, newValue);
        }
        private void BuyResources(PlantType type, int countFrom)
        {
            _gameUIController.DisplayByuPlants(type, countFrom);
            _questMap[type] -= countFrom;

            if (IsQuestComplete())
            {
                EndLevel(true);
            }
        }

        private void EndLevel(bool isWin)
        {
            onAddQuestTime = null;
            
            if (isWin)
            {
                _levelsComplete++;
                SaveLoadService.Instance.SaveLevelProgress(_resourceController.Money, _levelsComplete);
            }
            else
            {
                onAddQuestTime += AddQuestTimeForReward;
            }
            
            _gameUIController.CreateWinLoseView(isWin, isWin ? null : onAddQuestTime);
            
            if (_updateTimerRoutine != null)
            {
                StopCoroutine(_updateTimerRoutine);
                _updateTimerRoutine = null;
            }
        }

        private void AddQuestTimeForReward()
        {
            onAddQuestTime -= AddQuestTimeForReward;
            _questTime = 20f;
            _updateTimerRoutine = StartCoroutine(UpdateTimer());
        }

        private void AddResources(PlantBlock plantBlock, int count)
        {
            _gameUIController.DisplayPlantCount(plantBlock, count);
        }
        
        private void InitializeQuest(LevelQuestData levelData)
        {
            _currentLevel.OnQuestReady -= InitializeQuest;
            
            _questTime = levelData.QuestTime;
            _questPlantsData = levelData.QuestPlantsData;
            
            _questMap = null;
            _questMap = new Dictionary<PlantType, int>();
            
            foreach (var plantData in _questPlantsData)
            {
                _questMap.Add(plantData.PlantType, plantData.Count);
            }
            onStartPlay += StartTimer;
            _gameUIController.SetQuestInfo(levelData, onStartPlay);
        }

        private void StartTimer()
        {
            onStartPlay -= StartTimer;
            _updateTimerRoutine = StartCoroutine(UpdateTimer());
        }

        private IEnumerator UpdateTimer()
        {
            while (_questTime > 0.9f)
            {
                _questTime -= Time.deltaTime;
                float minutes = Mathf.FloorToInt(_questTime / 60);
                float seconds = Mathf.FloorToInt(_questTime % 60);
                var time = $"{minutes:00}:{seconds:00}";
                _gameUIController.DisplayTimer(time);
                
                if (_questTime < 6f && !_isHurry)
                {
                    _gameUIController.ChangeTimerStyle(true);
                }
                yield return null;
            }
            
            if (!IsQuestComplete())
                EndLevel(false);

            _updateTimerRoutine = null;
        }

        private bool IsQuestComplete()
        {
            foreach (var map in _questMap)
            {
                if (map.Value > 0)
                    return false;
            }
            
            return true;
        }
    }
}