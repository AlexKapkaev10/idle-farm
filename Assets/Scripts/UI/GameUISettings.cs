using System.Collections.Generic;
using UnityEngine;

namespace Scripts.UI
{
    [CreateAssetMenu(fileName = nameof(GameUISettings), menuName = "SO/GameUISettings")]
    public class GameUISettings : ScriptableObject
    {
        [SerializeField] private List<GameUI> _defaultUIPrefabs = new List<GameUI>();
        [SerializeField] private GameUI _storePrefab;

        [SerializeField] private Color _timerColorDefault;
        [SerializeField] private Color _timerColorHurry;

        public List<GameUI> DefaultUIPrefabs => _defaultUIPrefabs;
        public GameUI StorePrefab => _storePrefab;
        public Color TimerColorDefault => _timerColorDefault;
        public Color TimerColorHurry => _timerColorHurry;
    }
}