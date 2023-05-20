using System.Collections.Generic;
using Scripts.Enums;
using Scripts.Game;
using UnityEngine;

namespace Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ToolsSettings", menuName = "Tools Settings", order = 51)]
    public class ToolsSettings : ScriptableObject
    {
        [SerializeField] private List<CharacterTool> _characterTools = new List<CharacterTool>();

        public CharacterTool GetTool(ToolType toolType)
        {
            for (int i = 0; i < _characterTools.Count; i++)
            {
                var tool = _characterTools[i];

                if (tool.ToolType == toolType)
                {
                    return tool;
                }
            }
            return null;
        }
    }
}