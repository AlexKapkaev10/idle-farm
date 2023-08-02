using System;
using System.Collections.Generic;
using Scripts.Enums;
using Scripts.Plants;

namespace Scripts.Game
{
    public interface IResourceController
    {
        public event Action QuestNotComplete;
        public event Action<PlantBlock, int> OnAddPlant;
        public event Action<int, int> OnChangeMoney;
        public event Action<BuyResource[]> OnBuyPlants;
        public event Action<PlantType> OnResourceComplete;
        public int Money { get; }
        public void TryGetMoney(int value, Action<bool> callBack);
        public void CalculateQuestComplete(in SowingField completeField);
        public void Add(PlantBlock plantBlock);
        public void Buy(in List<PlantType> plantTypes);
        public void SetQuestMap(in Dictionary<PlantType, int> questMap);
    }
}