using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Buildings
{
    [CreateAssetMenu(fileName = nameof(BuildingsSettings), menuName = "Project/SO/Buildings")]
    public class BuildingsSettings : ScriptableObject
    {
        [SerializeField] private List<BuildingData> _buildingsData = new List<BuildingData>();

        public List<BuildingData> BuildingsData => _buildingsData;
    }
}