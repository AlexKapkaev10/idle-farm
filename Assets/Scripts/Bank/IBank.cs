﻿using System;

namespace Scripts.Resources
{
    public interface IBank
    {
        public event Action<int, int> OnMoneyChange;
        public int Money { get; }
        public bool IsEnough(int value);
        public void Init(int moneyCount);
        public void MoneyValueChange(int value);

    }
}