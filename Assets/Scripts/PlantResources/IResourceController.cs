using System;
using System.Collections.Generic;
using Scripts.Enums;
using Scripts.Plants;

namespace Scripts.Game
{
    public interface IResourceController
    {
        public void TryGetMoney(int value, Action<bool> callBack);
        public void Add(Plant plant);
        public void Buy(in List<PlantType> plantTypes);
    }
}