using System.Collections;
using System.Collections.Generic;
using Scripts.Enums;
using Scripts.Game;
using Scripts.Interfaces;
using Scripts.Plants;
using Scripts.UI;
using UnityEngine;
using VContainer;

namespace Scripts.Level
{
    public sealed class LevelController : MonoBehaviour, ILevelController
    {
        [SerializeField] private List<Level> _levelPrefabs = new List<Level>();
        
        private ICharacterController _characterController = default;
        private IGameUIController _gameUIController = default;
        private IResourceController _resourceController = default;
        
        private Level _currentLevel = default;

        private readonly bool _isHurry = default;
        private float _questTime = 0.0f;

        [Inject]
        private void Construct(ICharacterController characterController, IGameUIController gameUIController, IResourceController resourceController)
        {
            _characterController = characterController;
            _gameUIController = gameUIController;
            _resourceController = resourceController;
        }

        private void Start()
        {
            StartLevel(Progress.Instance.Level);
        }

        private void StartLevel(int index)
        {
            if (index > _levelPrefabs.Count)
                return;
            
            if (_currentLevel)
            {
                _resourceController.OnAddPlant -= AddResources;
                _resourceController.OnBuyPlants -= BuyResources;
                _resourceController.OnChangeMoney -= MoneyChange;
                
                _gameUIController.ChangeTimer(false);
                Destroy(_currentLevel);
            }
            
            _resourceController.OnAddPlant += AddResources;
            _resourceController.OnBuyPlants += BuyResources;
            _resourceController.OnChangeMoney += MoneyChange;

            _currentLevel = Instantiate(_levelPrefabs[index]);
            _currentLevel.OnLevelQuestReady += StartQuest;
            _currentLevel.Init(_characterController);
        }

        private void MoneyChange(int oldValue, int newValue)
        {
            _gameUIController.DisplayMoneyCount(oldValue, newValue);
        }
        private void BuyResources(PlantType type, int countFrom)
        {
            _gameUIController.DisplayByuPlants(type, countFrom);
        }

        private void AddResources(PlantBlock plantBlock, int count)
        {
            _gameUIController.DisplayPlantCount(plantBlock, count);
            Debug.Log($"Type {plantBlock.PlantType} \n Count: {count}");
        }
        
        private void StartQuest(LevelQuestData data)
        {
            _currentLevel.OnLevelQuestReady -= StartQuest;
            _questTime = data.QuestTime;
            StartCoroutine(UpdateTimer());

            /*foreach (var plantData in data.QuestPlantsData)
            {
                Debug.Log($"Type: {plantData.PlantType} \n Count: {plantData.Count}");
            }*/
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
                    _gameUIController.ChangeTimer(true);
                }
                yield return null;
            }
                
            Debug.Log("End Time");
        }
    }
}