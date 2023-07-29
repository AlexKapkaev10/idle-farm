using System;

namespace Scripts.UI
{
    public interface IGameUIController
    {
        public event Action OnUIesReady;

        public Joystick GetJoystick();
    }
}