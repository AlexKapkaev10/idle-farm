﻿using System;
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
        private event Action onAddQuestTime;

        [SerializeField] private LevelControllerSettings _controllerSettings;

        private ICharacterController _characterController = default;
        private IBobController _bobController = default;
        private IGameUIController _gameUIController = default;
        private IResourceController _resourceController = default;
        
        private Level _currentLevel = default;
        private List<QuestPlantData> _questPlantsData = default;

        private Coroutine _updateTimerRoutine;
        private readonly bool _isHurry = default;
        
        private int _completeLevels = 0;
        private float _questTime = 0.0f;

        private List<Level> _levelPrefabs;
        private Dictionary<PlantType, int> _questMap; 

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
            
            _completeLevels = SaveLoadService.Instance.Level;
            _gameUIController.OnLevelPlay += InitializeLevel;
        }
        
        private void Start()
        {
            Application.targetFrameRate = 100;
            _levelPrefabs = _controllerSettings.LevelPrefabs;
            
            _resourceController.OnAddPlant += AddResources;
            _resourceController.OnBuyPlants += BuyResources;
            _resourceController.OnChangeMoney += MoneyChange;
            _resourceController.QuestNotComplete += QuestNotComplete;
            InitializeLevel();
        }

        private void OnDestroy()
        {
            _gameUIController.OnLevelPlay -= InitializeLevel;
            _resourceController.OnAddPlant -= AddResources;
            _resourceController.OnBuyPlants -= BuyResources;
            _resourceController.OnChangeMoney -= MoneyChange;
            _resourceController.QuestNotComplete -= QuestNotComplete;
        }

        private void InitializeLevel()
        {
            if (_currentLevel)
                ClearLevel();
            
            var index = _completeLevels > (_levelPrefabs.Count - 1)
                ? Random.Range(0, _levelPrefabs.Count)
                : _completeLevels;

            _currentLevel = Instantiate(_levelPrefabs[index]);
            _currentLevel.OnQuestReady += InitializeQuest;
            _currentLevel.Init(_characterController, _bobController);
            _gameUIController.UpdateTimerStyle(true);
        }

        private void QuestNotComplete()
        {
            Debug.Log("Чарли! Этого не достаточно");
            _bobController.SwitchAnimation(BobAnimationType.NotComplete);
        }

        private void ClearLevel()
        {
            Destroy(_currentLevel.gameObject);
            _currentLevel = null;
        }

        private void MoneyChange(int oldValue, int newValue)
        {
            _gameUIController.DisplayMoneyCount(oldValue, newValue);
        }

        private void BuyResources(BuyResource[] buyResources)
        {
            foreach (var buyResource in buyResources)
            {
                _gameUIController.DisplayByuPlants(buyResource.PlantType, buyResource.Count);
            }
            
            EndLevel(true);
        }

        private void EndLevel(bool isWin)
        {
            if (_updateTimerRoutine != null)
            {
                StopCoroutine(_updateTimerRoutine);
                _updateTimerRoutine = null;
            }

            onAddQuestTime = null;
            if (isWin)
            {
                _completeLevels++;
                _gameUIController.UpdateTimerStyle(true);
                SaveLoadService.Instance.SaveLevelProgress(_resourceController.Money, _completeLevels);
            }
            else
            {
                onAddQuestTime += AddQuestTimeForReward;
            }

            _gameUIController.CreateWinLoseView(isWin, isWin ? null : onAddQuestTime);
            _bobController.SwitchAnimation(isWin ? BobAnimationType.Win : BobAnimationType.Lose);
            OnLevelComplete?.Invoke(isWin);
        }

        private void AddQuestTimeForReward()
        {
            onAddQuestTime -= AddQuestTimeForReward;
            _questTime = _controllerSettings.AddTimeCount;
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
            
            _resourceController.SetQuestMap(_questMap);
            onStartPlay += StartTimer;
            _gameUIController.CreateQuestInfo(levelData, onStartPlay);
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
                    _gameUIController.UpdateTimerStyle(false);
                }
                yield return null;
            }
            
            EndLevel(false);
            _updateTimerRoutine = null;
        }
    }
}