using System;
using Scripts.Enums;
using Scripts.Plants;

namespace Scripts.UI
{
    public interface IGameUIController
    {
        public event Action OnUIesReady;
        public void DisplayPlantCount(PlantBlock plantBlock, int count);
        public void DisplayMoneyCount(int from, int to);
        public void DisplayByuPlants(PlantType type, int from);
        public void DisplayTimer(string textTimer);
        public void ChangeTimer(bool isChange);
        public Joystick GetJoystick();
    }
}