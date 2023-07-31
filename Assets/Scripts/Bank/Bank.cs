using System;
using Scripts.Game;
using UnityEngine;

namespace Scripts.Resources
{
    public class Bank : IBank
    {
        public event Action<int, int> OnMoneyChange;
        
        private int _money = 0;

        public int Money => _money;

        public void Init(int moneyCount)
        {
            _money = moneyCount;
        }

        public bool IsEnough(int value)
        {
            return value <= _money;
        }

        public void MoneyValueChange(int value)
        {
            _money += value;
            var from = _money - value;
            OnMoneyChange?.Invoke(from, _money);
        }
    }
}