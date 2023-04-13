using Scripts.UI;
using UnityEngine;
using Zenject;

namespace Scripts.Architecture
{
    public class LocationInstaller : MonoInstaller
    {
        public UIController UIControllerPrefab;
        public Transform StartPoint;
        public GameObject PlayerPrefab;

        public override void InstallBindings()
        {
            UIController uiController = Container.InstantiatePrefabForComponent<UIController>(UIControllerPrefab);

            Container
                .Bind<UIController>()
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
