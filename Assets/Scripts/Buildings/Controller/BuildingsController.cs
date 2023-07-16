using Scripts.Interfaces;
using UnityEngine;
using VContainer;

namespace Scripts.Buildings
{
    public class BuildingsController : MonoBehaviour, IBuildingsController
    {
        [SerializeField] private BuildingsSettings _buildingsSettings;
        
        private ICharacterController _characterController;

        [Inject]
        private void Construct(ICharacterController characterController)
        {
            _characterController = characterController;
        }

        private void Start()
        {
            SpawnBuildings();
        }

        private void SpawnBuildings()
        {
            if (_buildingsSettings == null)
                return;
            
            foreach (var data in _buildingsSettings.BuildingsData)
            {
                var build = Instantiate(data.Prefab, transform);
                build.SetTransform(data.Position, data.Rotation);
                build.SetCharacterController(_characterController);
                build.PlantTypes = data.PlantTypes;
            }
        }
    }
}