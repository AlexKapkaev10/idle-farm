using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Level
{
    [CreateAssetMenu(fileName = nameof(LevelControllerSettings), menuName = "SO/LevelControllerSettings")]
    public class LevelControllerSettings : ScriptableObject
    {
        [SerializeField] private List<Level> _levelPrefabs = new List<Level>();

        public List<Level> LevelPrefabs => _levelPrefabs;
    }
}