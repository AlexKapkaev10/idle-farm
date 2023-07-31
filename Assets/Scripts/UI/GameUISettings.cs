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
        [SerializeField] private GameUI _winLoseViewPrefab;

        [SerializeField] private string _winHeader;
        [SerializeField] private string _loseHeader;

        [SerializeField] private Color _timerColorDefault;
        [SerializeField] private Color _timerColorHurry;

        public List<GameUI> DefaultUIPrefabs => _defaultUIPrefabs;
        public Joystick JoystickPrefab => _joystickPrefab;
        public ResourceView ResourceViewPrefab => _resourceViewPrefab;
        public QuestInfoView QuestInfoView => _questInfoViewPrefab;
        public GameUI StorePrefab => _storePrefab;
        public GameUI WinLoseViewPrefab => _winLoseViewPrefab;
        public string WinHeader => _winHeader;
        public string LoseHeader => _loseHeader;
        public Color TimerColorDefault => _timerColorDefault;
        public Color TimerColorHurry => _timerColorHurry;

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