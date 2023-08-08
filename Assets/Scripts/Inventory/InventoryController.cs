using System;
using Scripts.Game;
using Scripts.UI;
using VContainer;

namespace Scripts.Inventory
{
    public sealed class InventoryController : IInventoryController
    {
        public event Action<float> OnRunSpeedChange;
        public event Action<float> OnMowSpeedChange;
        public event Action<int> OnCurrentToolChange;

        private IGameUIController _gameUIController;

        private InventorySettingsData _inventorySettingsData;
        private GameController _gameController;
        
        [Inject]
        public InventoryController(IGameUIController gameUIController)
        {
            _gameUIController = gameUIController;
        }

        public void Initialize()
        {
            _inventorySettingsData = new InventorySettingsData();
            _gameController = GameController.Instance;

            SetRunSpeed(_gameController.GetRunSpeed());
            SetMowSpeed(_gameController.GetMowSpeed());
            SetCurrentToolID(_gameController.GetCurrentToolID());
        }

        public void SetRunSpeed(float value)
        {
            _inventorySettingsData.RunSpeed = value;
            OnRunSpeedChange?.Invoke(value);
        }

        public void SetMowSpeed(float value)
        {
            _inventorySettingsData.MowSpeed = value;
            OnMowSpeedChange?.Invoke(value);
        }

        public void SetCurrentToolID(int value)
        {
            _inventorySettingsData.CurrentToolID = value;
            OnCurrentToolChange?.Invoke(value);
        }
    }

    public sealed class InventorySettingsData
    {
        public float RunSpeed;
        public float MowSpeed;
        public int CurrentToolID;
    }
}