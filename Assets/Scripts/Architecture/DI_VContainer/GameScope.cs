using Scripts.CameraGame;
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
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private ToolsSettings _toolsSettings;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_toolsSettings);
            builder.RegisterComponent(_gameUI);
            builder.Register<ResourceController>(Lifetime.Singleton);
            builder.RegisterComponentInNewPrefab<Character>(_character, Lifetime.Scoped);
            builder.RegisterComponent(_cameraController);
        }
    }
}
