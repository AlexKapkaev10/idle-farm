using Scripts.Enums;
using Scripts.Interfaces;
using UnityEngine;

namespace Scripts.Game
{
    public class CharacterTool : MonoBehaviour, ITool
    {
        [SerializeField] private float _mowSpeed = 0;
        [SerializeField] private ToolType _toolType;
        [SerializeField] private int _maxSharpCount = 2;
        
        private int _currentSharpCount;
        private bool _dispose;

        public int CurrentSharpCount
        {
            get => _currentSharpCount; 
            set
            {
                if (_currentSharpCount > 0)
                {
                    _currentSharpCount = value;
                }
            }
        }
        
        public ToolType ToolType => _toolType;
        public float MowSpeed => _mowSpeed;

        public bool IsSharp()
        {
            return CurrentSharpCount > 0;
        }

        public void SetActive(bool value)
        {
            if (_dispose)
                return;
            
            gameObject.SetActive(value);
        }

        public void SetMaxSharpCount()
        {
            _currentSharpCount = _maxSharpCount;
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