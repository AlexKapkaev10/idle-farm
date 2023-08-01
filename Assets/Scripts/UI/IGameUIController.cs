using System;
using Scripts.Enums;
using Scripts.Level;
using Scripts.Plants;

namespace Scripts.UI
{
    public interface IGameUIController
    {
        public event Action OnLevelPlay;
        public event Action<Joystick> OnJoystickCreate;
        public void DisplayPlantCount(PlantBlock plantBlock, int count);
        public void DisplayMoneyCount(int from, int to);
        public void DisplayByuPlants(PlantType type, int from);
        public void DisplayTimer(string textTimer);
        public void CreateWinLoseView(bool isWin, Action callBack);
        public void UpdateTimerStyle(bool isDefault);
        public void CreateQuestInfo(LevelQuestData levelQuestData, Action callBack);
    }
}