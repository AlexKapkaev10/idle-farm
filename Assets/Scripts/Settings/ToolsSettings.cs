using Scripts.Enums;
using UnityEngine;

namespace Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ToolsSettings", menuName = "Tools Settings", order = 51)]
    public class ToolsSettings : ScriptableObject
    {
        [SerializeField] private GameObject _defaultTool;

        public GameObject GetTool(ToolType toolType)
        {
            return toolType switch
            {
                ToolType.Default => _defaultTool,
                _ => null
            };
        }
    }
}