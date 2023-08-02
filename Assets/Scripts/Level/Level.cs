using System;
using System.Collections.Generic;
using System.Linq;
using Scripts.Enums;
using Scripts.Game;
using Scripts.Interfaces;
using UnityEngine;

namespace Scripts.Level
{
    public class Level : MonoBehaviour
    {
        public event Action<LevelQuestData> OnQuestReady;
        public event Action<SowingField> OnFieldClear;
        
        [SerializeField] private LevelSettings _levelSettings;

        public void Init(in ICharacterController characterController, in IBobController bobController)
        {
            CreateLevel(characterController, bobController);
            
            var levelQuestData = new LevelQuestData
            {
                QuestTime = _levelSettings.QuestTime,
                QuestPlantsData = _levelSettings.QuestPlantsData
            };
            
            OnQuestReady?.Invoke(levelQuestData);
        }

        private void CreateLevel(in ICharacterController characterController, in IBobController bobController)
        {
            if (_levelSettings == null)
                return;
            
            characterController.SetTransform(_levelSettings.CharacterSpawnPosition, _levelSettings.CharacterSpawnRotation);
            bobController.SetTransform(_levelSettings.BobSpawnPosition, _levelSettings.BobSpawnRotation);
            
            var questPlantsData = _levelSettings.QuestPlantsData;
            var plantTypes = new PlantType[questPlantsData.Count];

            for (var i = 0; i < questPlantsData.Count; i++)
            {
                var data = questPlantsData[i];
                plantTypes[i] = data.PlantType;
            }

            foreach (var buildData in _levelSettings.BuildingsData)
            {
                var build = Instantiate(buildData.Prefab, transform);
                build.SetTransform(buildData.Position, buildData.Rotation);
                build.SetDependency(characterController);
                build.PlantTypes = plantTypes.ToList();
            }

            foreach (var fieldData in _levelSettings.FieldsData)
            {
                var field = Instantiate(fieldData.Field, transform);
                field.SetTransform(fieldData.Position, fieldData.Rotation);
                field.AutoRepair = fieldData.AutoRepair;
                field.OnFieldClear += FieldClear;
            }
        }

        private void OnDestroy()
        {
            OnQuestReady = null;
            OnFieldClear = null;
        }

        private void FieldClear(SowingField sowingField)
        {
            sowingField.OnFieldClear -= FieldClear;
            OnFieldClear?.Invoke(sowingField);
        }
    }

    public struct LevelQuestData
    {
        public float QuestTime;
        public List<QuestPlantData> QuestPlantsData;
    }
}
