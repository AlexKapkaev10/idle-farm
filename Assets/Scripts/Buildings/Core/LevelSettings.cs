using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Buildings
{
    [CreateAssetMenu(fileName = nameof(LevelSettings), menuName = "Project/SO/Level_Settings")]
    public class LevelSettings : ScriptableObject
    {
        [SerializeField] private List<BuildingData> _buildingsData = new List<BuildingData>();

        public List<BuildingData> BuildingsData => _buildingsData;
    }
}