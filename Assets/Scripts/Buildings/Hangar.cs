using Scripts.Game;
using UnityEngine;

namespace Scripts.Buildings
{
    public class Hangar : Build
    {
        [SerializeField] private BobController _bobController;

        protected override void LevelComplete(bool isWin)
        {
            _bobController.SwitchAnimation(isWin ? BobAnimationType.Win : BobAnimationType.Lose);
        }

        protected override void QuestNotComplete()
        {
            Debug.Log("Чарли! Этого не достаточно");
            _bobController.SwitchAnimation(BobAnimationType.NotComplete);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == _characterController.GetGameObject())
            {
                _characterController.BuyPlants(PlantTypes);
            }
        }
    }
}
