using System.Collections.Generic;
using Scripts.Enums;
using Scripts.Game;
using UnityEngine;

namespace Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ToolsSettings", menuName = "SO/Tools Settings", order = 51)]
    public sealed class ToolsSettings : ScriptableObject
    {
        [SerializeField] private List<CharacterTool> _characterTools;

        public CharacterTool GetTool(ToolType toolType)
        {
            foreach (var tool in _characterTools)
            {
                if (tool.ToolType == toolType)
                {
                    return tool;
                }
            }

            return null;
        }
    }
}