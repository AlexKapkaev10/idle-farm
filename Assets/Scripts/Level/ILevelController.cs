using System;

namespace Scripts.Level
{
    public interface ILevelController
    {
        public event Action<bool> OnLevelComplete;
    }
}