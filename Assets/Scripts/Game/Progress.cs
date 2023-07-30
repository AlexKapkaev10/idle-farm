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
        public int ToolId = default;
        public int[] AvailableToolsID = default;
    }
    
    public class Progress : MonoBehaviour
    {
        public ProgressData ProgressData;
        
        public static Progress Instance;

        [DllImport("__Internal")]
        private static extern void SaveExtern(string data);
        
        [DllImport("__Internal")]
        private static extern void LoadExtern();

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

        public void Save()
        {
            string jsonData = JsonUtility.ToJson(ProgressData);
            SaveExtern(jsonData);
        }

        public void SetProgress(string value)
        {
            ProgressData = JsonUtility.FromJson<ProgressData>(value);
        }
    }
}
