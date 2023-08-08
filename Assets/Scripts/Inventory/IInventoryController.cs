using System;

namespace Scripts.Inventory
{
    public interface IInventoryController
    {
        public event Action<float> OnRunSpeedChange;
        public event Action<float> OnMowSpeedChange;
        public event Action<int> OnCurrentToolChange;
        
        public void Initialize();
        public void SetRunSpeed(float value);
        public void SetMowSpeed(float value);
        public void SetCurrentToolID(int value);
    }
}