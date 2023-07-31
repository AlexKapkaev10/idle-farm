using System;
using System.Collections.Generic;
using Scripts.Buildings;
using Scripts.Enums;
using UnityEngine;

namespace Scripts.Level
{
    [CreateAssetMenu(fileName = nameof(LevelSettings), menuName = "Project/SO/Level_Settings")]
    public class LevelSettings : ScriptableObject
    {
        [SerializeField] private List<BuildingData> _buildingsData = new List<BuildingData>();
        [SerializeField] private List<QuestPlantData> _questPlantsData = new List<QuestPlantData>();
        [SerializeField] private int _questTime;
        
        public List<BuildingData> BuildingsData => _buildingsData;
        public List<QuestPlantData> QuestPlantsData => _questPlantsData;
        public int QuestTime => _questTime;
    }

    [Serializable]
    public struct QuestPlantData
    {
        public PlantType PlantType;
        public int Count;
    }
}