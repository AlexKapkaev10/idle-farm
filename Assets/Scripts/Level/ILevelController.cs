using System;

namespace Scripts.Level
{
    public interface ILevelController
    {
        public event Action<bool> OnLevelComplete;
        public event Action OnQuestNotComplete;
    }
}