using System;
using UnityEngine;

namespace Scripts.Resources
{
    public class Bank : IBank
    {
        public event Action<int, int> OnMoneyChange;
        
        private const string SaveMoneyKey = "money";
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
            PlayerPrefs.SetInt(SaveMoneyKey, _money);
            var from = _money - value;
            OnMoneyChange?.Invoke(from, _money);
        }
    }
}