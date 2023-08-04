using System;
using Scripts.Enums;
using Scripts.Game;
using Scripts.Level;
using Scripts.Plants;

namespace Scripts.UI
{
    public interface IGameUIController
    {
        public event Action OnLevelPlay;
        public event Action<Joystick> OnJoystickCreate;
        public void DisplayPlantCount(PlantBlock plantBlock, int count);
        public void DisplayTimer(string textTimer);
        public void CreateEndLevelView(BuyResourceData data, Action addTimeCallBack);
        public void UpdateTimerStyle(bool isDefault);
        public void CreateQuestInfo(LevelQuestData levelQuestData, Action callBack);
        public void ResourceComplete(PlantType type);
    }
}