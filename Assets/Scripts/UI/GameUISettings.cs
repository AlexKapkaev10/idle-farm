using System;
using System.Collections.Generic;
using Scripts.Enums;
using UnityEngine;

namespace Scripts.UI
{
    [CreateAssetMenu(fileName = nameof(GameUISettings), menuName = "SO/GameUISettings")]
    public class GameUISettings : ScriptableObject
    {
        [SerializeField] private List<GameUI> _defaultUIPrefabs = new List<GameUI>();
        [SerializeField] private List<SpriteByPlantType> _spritesByPlantType = new List<SpriteByPlantType>();

        [SerializeField] private Joystick _joystickPrefab;
        [SerializeField] private ResourceView _resourceViewPrefab;
        [SerializeField] private QuestInfoView _questInfoViewPrefab;
        
        [SerializeField] private GameUI _storePrefab;
        [SerializeField] private GameUI _endLevelPrefab;

        [SerializeField] private string[] _questHeaders;
        [SerializeField] private string _winHeader;
        [SerializeField] private string _loseHeader;
        [SerializeField] private string _buttonNextWinHeader;
        [SerializeField] private string _buttonNextLoseHeader;
        [SerializeField] private string _rewardWinHeader;
        [SerializeField] private string _rewardLoseHeader;
        [SerializeField] private string _buttonRewardWinHeader;
        [SerializeField] private string _buttonRewardLoseHeader;

        [SerializeField] private float _moneyCounterTimeDelay;
        [SerializeField] private float _displayWinLoseTime;
        [SerializeField] private float _fadeDurationView = 0.3f;

        public List<GameUI> DefaultUIPrefabs => _defaultUIPrefabs;
        public Joystick JoystickPrefab => _joystickPrefab;
        public ResourceView ResourceViewPrefab => _resourceViewPrefab;
        public QuestInfoView QuestInfoView => _questInfoViewPrefab;
        public GameUI StorePrefab => _storePrefab;
        public GameUI EndLevelPrefab => _endLevelPrefab;
        public string[] QuestHeaders => _questHeaders;
        public string WinHeader => _winHeader;
        public string LoseHeader => _loseHeader;
        public string ButtonNextWinHeader => _buttonNextWinHeader;
        public string ButtonNextLoseHeader => _buttonNextLoseHeader;
        public string WinRewardHeader => _rewardWinHeader;
        public string LoseRewardHeader => _rewardLoseHeader;
        public string ButtonRewardWinHeader => _buttonRewardWinHeader;
        public string ButtonRewardLoseHeader => _buttonRewardLoseHeader;
        public float MoneyCounterTimeDelay => _moneyCounterTimeDelay;
        public float DisplayWinLoseTime => _displayWinLoseTime;
        public float FadeDurationView => _fadeDurationView;

        public Sprite GetSpriteByPlantType(PlantType type)
        {
            foreach (var spriteByPlant in _spritesByPlantType)
            {
                if (spriteByPlant.PlantType == type)
                {
                    return spriteByPlant.Sprite;
                }
            }

            return null;
        }
    }

    [Serializable]
    public struct SpriteByPlantType
    {
        public PlantType PlantType;
        public Sprite Sprite;
    }
}