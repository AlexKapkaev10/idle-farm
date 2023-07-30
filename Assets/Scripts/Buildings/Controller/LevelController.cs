using System.Collections.Generic;
using Scripts.Game;
using Scripts.Interfaces;
using Scripts.UI;
using UnityEngine;
using VContainer;

namespace Scripts.Buildings
{
    public sealed class LevelController : MonoBehaviour, ILevelController
    {
        [SerializeField] private List<Level> _levelPrefabs = new List<Level>();

        private Level _currentLevel;
        
        private ICharacterController _characterController;

        [Inject]
        private void Construct(ICharacterController characterController, IGameUIController gameUIController)
        {
            _characterController = characterController;
        }

        private void Start()
        {
            StartLevel(Progress.Instance.ProgressData.Level);
        }

        private void StartLevel(int index)
        {
            if (index > _levelPrefabs.Count)
                return;

            var level = Instantiate(_levelPrefabs[index]);
            level.Init(_characterController);
        }
    }
}