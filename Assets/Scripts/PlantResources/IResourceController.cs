﻿using System;
using System.Collections.Generic;
using Scripts.Enums;
using Scripts.Plants;

namespace Scripts.Game
{
    public interface IResourceController
    {
        public event Action<PlantBlock, int> OnAddPlant;
        public event Action<int, int> OnChangeMoney;
        public event Action<PlantType, int> OnBuyPlants;
        public int Money { get; }
        public void TryGetMoney(int value, Action<bool> callBack);
        public void Add(PlantBlock plantBlock);
        public void Buy(in List<PlantType> plantTypes);
    }
}