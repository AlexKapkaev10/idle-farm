using Scripts.Buildings;
using Scripts.Interfaces;
using UnityEngine;

namespace Scripts
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private LevelSettings _levelSettings;

        private ICharacterController _characterController;

        public void Init(ICharacterController characterController)
        {
            _characterController = characterController;
            SpawnBuildings();
        }

        private void SpawnBuildings()
        {
            if (_levelSettings == null)
                return;
            
            foreach (var data in _levelSettings.BuildingsData)
            {
                var build = Instantiate(data.Prefab, transform);
                build.SetTransform(data.Position, data.Rotation);
                build.SetCharacterController(_characterController);
                build.PlantTypes = data.PlantTypes;
            }
        }
    }
}
