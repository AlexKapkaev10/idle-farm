using Scripts.Buildings;
using Scripts.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Scripts.Game;
using Scripts.Interfaces;
using Scripts.Resources;
using Scripts.ScriptableObjects;

namespace Scripts.Architecture
{
    public class GameScope : LifetimeScope
    {
        [SerializeField] private Character _character;
        [SerializeField] private GameUI _gameUI;
        
        [SerializeField] private ToolsSettings _toolsSettings;
        [SerializeField] private BankSettings _bankSettings;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<Bank>(Lifetime.Singleton);
            builder.Register<ResourceController>(Lifetime.Singleton).WithParameter(_bankSettings);
            builder.RegisterComponentInNewPrefab<GameUI>(_gameUI, Lifetime.Scoped);
            builder.RegisterComponentInNewPrefab<Character>(_character, Lifetime.Scoped).As(typeof(ICharacterController)).WithParameter(_toolsSettings);
            builder.RegisterComponentInHierarchy<BuildingsController>().As(typeof(IBuildingsController));
        }
    }
}
