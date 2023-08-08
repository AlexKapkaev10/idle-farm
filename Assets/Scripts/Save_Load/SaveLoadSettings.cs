using UnityEngine;

namespace Scripts.Game
{
    [CreateAssetMenu(fileName = nameof(SaveLoadSettings), menuName = "SO/SaveLoad")]
    public class SaveLoadSettings : ScriptableObject
    {
        private const string _saveMoneyKey = "money";
        private const string _saveLevelKey = "level";
        private const string _saveRunSpeed = "runSpeed";
        private const string _saveMowSpeed = "mowSpeed";
        private const string _saveCurrentToolID = "toolID";
        private const string _saveToolSharp = "toolSharp";

        public string SaveMoneyKey => _saveMoneyKey;
        public string SaveLevelKey => _saveLevelKey;
        public string SaveRunSpeed => _saveRunSpeed;
        public string SaveMowSpeed => _saveMowSpeed;
        public string SaveToolSharp => _saveToolSharp;
        public string SaveCurrentToolID => _saveCurrentToolID;

    }
}