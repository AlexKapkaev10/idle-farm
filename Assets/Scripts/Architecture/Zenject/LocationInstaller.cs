using Scripts.Game;
using Scripts.UI;
using UnityEngine;
using Zenject;

namespace Scripts.Architecture
{
    public class LocationInstaller : MonoInstaller
    {
        public BankService UIControllerPrefab;
        public Transform StartPoint;
        public GameObject PlayerPrefab;

        public override void InstallBindings()
        {
            BankService uiController = Container.InstantiatePrefabForComponent<BankService>(UIControllerPrefab);

            Container
                .Bind<BankService>()
                .FromInstance(uiController)
                .AsSingle();

            Character character = Container
                .InstantiatePrefabForComponent<Character>(PlayerPrefab, StartPoint.position, Quaternion.identity, null);

            Container
                .Bind<Character>()
                .FromInstance(character)
                .AsSingle();
        }
    }
}
