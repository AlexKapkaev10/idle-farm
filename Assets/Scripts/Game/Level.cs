using System;
using System.Collections.Generic;
using Scripts.Interfaces;
using UnityEngine;

namespace Scripts.Level
{
    public class Level : MonoBehaviour
    {
        public event Action<LevelQuestData> OnLevelQuestReady;
        
        [SerializeField] private LevelSettings _levelSettings;

        public void Init(in ICharacterController characterController)
        {
            SpawnBuildings(characterController);
            
            var levelQuestData = new LevelQuestData
            {
                QuestTime = _levelSettings.QuestTime,
                QuestPlantsData = _levelSettings.QuestPlantsData
            };
            
            OnLevelQuestReady?.Invoke(levelQuestData);
        }

        private void SpawnBuildings(in ICharacterController characterController)
        {
            if (_levelSettings == null)
                return;
            
            foreach (var data in _levelSettings.BuildingsData)
            {
                var build = Instantiate(data.Prefab, transform);
                build.SetTransform(data.Position, data.Rotation);
                build.SetCharacterController(characterController);
                build.PlantTypes = data.PlantTypes;
            }
        }
    }

    public struct LevelQuestData
    {
        public float QuestTime;
        public List<QuestPlantData> QuestPlantsData;
    }
}
