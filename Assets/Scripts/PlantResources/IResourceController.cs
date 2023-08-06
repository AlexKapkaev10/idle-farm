using System;
using System.Collections.Generic;
using Scripts.Enums;
using Scripts.Plants;

namespace Scripts.Game
{
    public interface IResourceController
    {
        public event Action<PlantBlock, int> OnAddPlant;
        public event Action<BuyResourceData> OnBuyPlants;
        public event Action<PlantType> OnResourceComplete;
        public event Action<bool> QuestComplete;
        public int Money { get; }
        public int GetSaveMoney();
        public void TryGetMoney(int value, Action<bool> callBack);
        public void CalculateQuestComplete(in SowingField completeField);
        public void Add(PlantBlock plantBlock);
        public void Buy(in List<PlantType> plantTypes);
        public void SetQuestMap(in Dictionary<PlantType, int> questMap);
        public void SetMoneyForReward(int value);
    }
}