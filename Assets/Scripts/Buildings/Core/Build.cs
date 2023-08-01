using System.Collections.Generic;
using Scripts.Enums;
using Scripts.Interfaces;
using Scripts.Level;
using UnityEngine;

namespace Scripts.Buildings
{
    public class Build : MonoBehaviour
    {
        protected ICharacterController _characterController;
        private ILevelController _levelController;

        public List<PlantType> PlantTypes { get; set; }

        protected virtual void LevelComplete(bool isWin)
        {
            
        }
        
        protected virtual void QuestNotComplete()
        {
            
        }

        public void SetTransform(Vector3 position, Vector3 rotation)
        {
            transform.position = position;
            transform.eulerAngles = rotation;
        }

        public void SetDependency(ICharacterController characterController, ILevelController levelController)
        {
            _characterController = characterController;
            _levelController = levelController;

            _levelController.OnLevelComplete += LevelComplete;
            _levelController.OnQuestNotComplete += QuestNotComplete;
        }

        private void OnDestroy()
        {
            _levelController.OnLevelComplete -= LevelComplete;
            _levelController.OnQuestNotComplete -= QuestNotComplete;
        }
    }
}