using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Scripts.Game
{
    [Serializable]
    public class ProgressData
    {
        public int Level = default;
        public int Money = default;
        public int CurrentToolId = default;
        public int[] AvailableToolsID = default;
    }
    
    public class SaveLoadService : MonoBehaviour
    {
        [SerializeField] private SaveLoadSettings _saveLoadSettings;
        [SerializeField] private ProgressData _progressData;
        public static SaveLoadService Instance;

        [DllImport("__Internal")]
        private static extern void SaveExtern(string data);
        
        [DllImport("__Internal")]
        private static extern void LoadExtern();

        public int Level => _progressData.Level;
        public int Money => _progressData.Money;
        public int CurrentTool => _progressData.CurrentToolId;
        public int[] AvailableToolsID => _progressData.AvailableToolsID;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void LoadProgress()
        {
#if !UNITY_WEBGL
            _progressData.Money = PlayerPrefs.GetInt(_saveLoadSettings.SaveMoneyKey);
#endif
        }
        
        public void SaveLevelProgress(int moneyCount, int levelCount)
        {
            _progressData.Money = moneyCount;
            _progressData.Level = levelCount;
            
#if !UNITY_WEBGL
            PlayerPrefs.SetInt(_saveLoadSettings.SaveMoneyKey, Money);
            PlayerPrefs.SetInt(_saveLoadSettings.SaveLevelKey, Level);
#endif
        }

        public void Save()
        {
            string jsonData = JsonUtility.ToJson(_progressData);
            SaveExtern(jsonData);
        }

        public void SetProgress(string value)
        {
            _progressData = JsonUtility.FromJson<ProgressData>(value);
        }
    }
}
