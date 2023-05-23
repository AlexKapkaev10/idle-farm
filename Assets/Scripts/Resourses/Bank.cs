using Scripts.UI;
using UnityEngine;

namespace Scripts.Resources
{
    public class Bank
    {
        private const string SaveMoneyKey = "money";
        private GameUI _gameUI;
        private int _money = 0;

        public int Money => _money;

        public void Init(GameUI gameUI)
        {
            _gameUI = gameUI;
            _money = PlayerPrefs.GetInt(SaveMoneyKey, 0);
            _gameUI.DisplayMoneyCount(0, _money);
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
            _gameUI.DisplayMoneyCount(from, _money);
        }
    }
}