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
            RegisterScriptableObjects(builder);
            RegisterResourceController(builder);
            RegisterGameUI(builder);
            RegisterCharacter(builder);
            builder.RegisterComponentInHierarchy<BarnController>();
        }

        private void RegisterScriptableObjects(IContainerBuilder builder)
        {
            builder.RegisterInstance(_toolsSettings);
            builder.RegisterInstance(_bankSettings);
        }

        private void RegisterResourceController(IContainerBuilder builder)
        {
            builder.Register<Bank>(Lifetime.Singleton);
            builder.Register<ResourceController>(Lifetime.Singleton);
        }
        
        private void RegisterGameUI(IContainerBuilder builder)
        {
            builder.RegisterComponentInNewPrefab<GameUI>(_gameUI, Lifetime.Scoped);
        }

        private void RegisterCharacter(IContainerBuilder builder)
        {
            builder.RegisterComponentInNewPrefab<Character>(_character, Lifetime.Scoped).As(typeof(ICharacterController));
        }
    }
}
