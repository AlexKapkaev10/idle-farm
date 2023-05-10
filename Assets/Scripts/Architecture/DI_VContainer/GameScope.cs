using Scripts.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Scripts.Game;
using Scripts.ScriptableObjects;

namespace Scripts.Architecture
{
    public class GameScope : LifetimeScope
    {
        [SerializeField] private Character _character;
        [SerializeField] private GameUI _gameUI;
        [SerializeField] private ToolsSettings _toolsSettings;

        protected override void Configure(IContainerBuilder builder)
        {
            RegisterScriptableObjects(builder);
            RegisterResourceController(builder);
            RegisterGameUI(builder);
            RegisterCharacter(builder);
        }

        private void RegisterScriptableObjects(IContainerBuilder builder)
        {
            builder.RegisterInstance(_toolsSettings);
        }

        private void RegisterResourceController(IContainerBuilder builder)
        {
            builder.Register<ResourceController>(Lifetime.Singleton);
        }
        
        private void RegisterGameUI(IContainerBuilder builder)
        {
            builder.RegisterComponentInNewPrefab<GameUI>(_gameUI, Lifetime.Scoped);
        }

        private void RegisterCharacter(IContainerBuilder builder)
        {
            builder.RegisterComponentInNewPrefab<Character>(_character, Lifetime.Scoped);
        }
    }
}
