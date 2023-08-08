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
        public float RunSpeed = default;
        public float MowSpeed = default;
        public int CurrentToolId = default;
        public int CurrentToolSharp = default;
        public int[] AvailableToolsID = default;
    }
    
    [RequireComponent(typeof(AdController))]
    public class GameController : MonoBehaviour
    {
        [SerializeField] private SaveLoadSettings _saveLoadSettings;
        [SerializeField] private AdController _adController;
        [SerializeField] private ProgressData _progressData;
        [SerializeField] private float _runSpeedDefault;
        [SerializeField] private float _mowSpeedDefault;
        
        public static GameController Instance;
        public AdController AdController => _adController;

        [DllImport("__Internal")]
        private static extern void SaveExtern(string data);
        
        [DllImport("__Internal")]
        private static extern void LoadExtern();
        public int[] AvailableToolsID => _progressData.AvailableToolsID;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                _adController = GetComponent<AdController>();
                LoadProgress();
                Application.targetFrameRate = 100;
                
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void LoadProgress()
        {
#if !UNITY_WEBGL
            _progressData.Money = PlayerPrefs.GetInt(_saveLoadSettings.SaveMoneyKey, 0);
            _progressData.Level = PlayerPrefs.GetInt(_saveLoadSettings.SaveLevelKey, 0);
            _progressData.RunSpeed = PlayerPrefs.GetFloat(_saveLoadSettings.SaveRunSpeed, _runSpeedDefault);
            _progressData.MowSpeed = PlayerPrefs.GetFloat(_saveLoadSettings.SaveMowSpeed, _mowSpeedDefault);
            _progressData.CurrentToolId = PlayerPrefs.GetInt(_saveLoadSettings.SaveCurrentToolID, 0);
#endif
        }
        
        public void SaveLevelProgress(int levelCount)
        {
            _progressData.Level = levelCount;
            
            Debug.Log($"Save Level: {_progressData.Level}");
#if !UNITY_WEBGL
            PlayerPrefs.SetInt(_saveLoadSettings.SaveLevelKey, _progressData.Level);
#endif
        }

        public void SaveMoney(int moneyCount)
        {
            _progressData.Money += moneyCount;
            
#if !UNITY_WEBGL
            PlayerPrefs.SetInt(_saveLoadSettings.SaveMoneyKey, _progressData.Money);
#endif
        }

        public int GetMoney()
        {
            return _progressData.Money;
        }

        public int GetLevel()
        {
            return _progressData.Level;
        }

        public float GetRunSpeed()
        {
            return _progressData.RunSpeed;
        }

        public float GetMowSpeed()
        {
            return _progressData.MowSpeed;
        }

        public int GetCurrentToolID()
        {
            return _progressData.CurrentToolId;
        }

        public void Save()
        {
#if !UNITY_WEBGL
            string jsonData = JsonUtility.ToJson(_progressData);
            SaveExtern(jsonData);
#endif
        }

        public void SetProgress(string value)
        {
            _progressData = JsonUtility.FromJson<ProgressData>(value);
        }
    }
}
