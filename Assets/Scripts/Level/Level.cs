using System;
using System.Collections.Generic;
using Scripts.Interfaces;
using UnityEngine;

namespace Scripts.Level
{
    public class Level : MonoBehaviour
    {
        public event Action<LevelQuestData> OnQuestReady;
        
        [SerializeField] private LevelSettings _levelSettings;

        public void Init(in ICharacterController characterController)
        {
            CreateLevel(characterController);
            
            var levelQuestData = new LevelQuestData
            {
                QuestTime = _levelSettings.QuestTime,
                QuestPlantsData = _levelSettings.QuestPlantsData
            };
            
            OnQuestReady?.Invoke(levelQuestData);
        }

        private void CreateLevel(in ICharacterController characterController)
        {
            if (_levelSettings == null)
                return;
            
            characterController.SetTransform(_levelSettings.CharacterSpawnPosition, _levelSettings.CharacterSpawnRotation);
            
            foreach (var buildData in _levelSettings.BuildingsData)
            {
                var build = Instantiate(buildData.Prefab, transform);
                build.SetTransform(buildData.Position, buildData.Rotation);
                build.SetCharacterController(characterController);
                build.PlantTypes = buildData.PlantTypes;
            }

            foreach (var fieldData in _levelSettings.FieldsData)
            {
                var field = Instantiate(fieldData.Field, transform);
                field.SetTransform(fieldData.Position, fieldData.Rotation);
            }
        }
    }

    public struct LevelQuestData
    {
        public float QuestTime;
        public List<QuestPlantData> QuestPlantsData;
    }
}
