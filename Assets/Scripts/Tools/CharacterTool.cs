using Scripts.Enums;
using Scripts.Interfaces;
using UnityEngine;

namespace Scripts.Game
{
    public class CharacterTool : MonoBehaviour, ITool
    {
        [SerializeField] private float _mowSpeed = 0;
        [SerializeField] private ToolType _toolType;

        private bool _dispose; 
        public ToolType ToolType => _toolType;
        public float MowSpeed => _mowSpeed;
        
        public void SetActive(bool value)
        {
            if (_dispose)
                return;
            
            gameObject.SetActive(value);
        }

        public void Clear()
        {
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            _dispose = true;
        }
    }
}