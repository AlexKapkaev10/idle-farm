using System;
using System.Collections.Generic;
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
    
    public class Progress : MonoBehaviour
    {
        [SerializeField] private ProgressData _progressData;
        
        public static Progress Instance;

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
/*#if UNITY_EDITOR
                _progressData = new ProgressData();
#endif*/
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void SaveMoney(int count)
        {
            
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
