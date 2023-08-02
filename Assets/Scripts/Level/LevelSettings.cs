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
        [SerializeField] private List<BuildingData> _buildingsData = new ();
        [SerializeField] private List<FieldData> _fieldsData = new ();
        [SerializeField] private List<QuestPlantData> _questPlantsData = new ();
        [Header("Character")]
        [SerializeField] private Vector3 _characterSpawnPosition = new (0f, 0.65f, 0f);
        [SerializeField] private Vector3 _characterSpawnRotation = new (0f, 180.0f, 0f);
        [Header("Bob")]
        [SerializeField] private Vector3 _bobSpawnPosition = new (11.5f, 0.5f, 5.0f);
        [SerializeField] private Vector3 _bobSpawnRotation = new (0f, 215.0f, 0f);
        [Space]
        [SerializeField] private int _questTime;
        
        public List<BuildingData> BuildingsData => _buildingsData;
        public List<FieldData> FieldsData => _fieldsData;
        public List<QuestPlantData> QuestPlantsData => _questPlantsData;
        public Vector3 CharacterSpawnPosition => _characterSpawnPosition;
        public Vector3 CharacterSpawnRotation => _characterSpawnRotation;
        public Vector3 BobSpawnRotation => _bobSpawnRotation;
        public Vector3 BobSpawnPosition => _bobSpawnPosition;
        public int QuestTime => _questTime;
    }

    [Serializable]
    public struct QuestPlantData
    {
        public PlantType PlantType;
        public int Count;
    }

    [Serializable]
    public struct FieldData
    {
        public SowingField Field;
        public Vector3 Position;
        public Vector3 Rotation;
        public bool AutoRepair;
    }
}