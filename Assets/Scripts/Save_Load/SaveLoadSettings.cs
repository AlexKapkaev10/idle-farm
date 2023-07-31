using UnityEngine;

namespace Scripts.Game
{
    [CreateAssetMenu(fileName = nameof(SaveLoadSettings), menuName = "SO/SaveLoad")]
    public class SaveLoadSettings : ScriptableObject
    {
        private const string _saveMoneyKey = "money";
        private const string _saveLevelKey = "level";

        public string SaveMoneyKey => _saveMoneyKey;
        public string SaveLevelKey => _saveLevelKey;
    }
}