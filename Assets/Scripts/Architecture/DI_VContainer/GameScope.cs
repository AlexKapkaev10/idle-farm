using Scripts.Buildings;
using Scripts.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Scripts.Game;
using Scripts.Interfaces;
using Scripts.Resources;
using Scripts.ScriptableObjects;
using Scripts.StateMachine;

namespace Scripts.Architecture
{
    public sealed class GameScope : LifetimeScope
    {
        [SerializeField] private Character _character;
        [SerializeField] private ToolsSettings _toolsSettings;
        [SerializeField] private BankSettings _bankSettings;
        [SerializeField] private CharacterSettings _characterSettings;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<Bank>(Lifetime.Singleton).As<IBank>();
            builder.Register<ResourceController>(Lifetime.Singleton)
                .As<IResourceController>()
                .WithParameter(_bankSettings);
            builder.RegisterComponentInHierarchy<GameUIController>().As<IGameUIController>();
            RegisterCharacterStateMachine(builder);
            builder.RegisterComponentInNewPrefab<Character>(_character, Lifetime.Scoped)
                .As<ICharacterController>()
                .WithParameter(_toolsSettings);
            builder.RegisterComponentInHierarchy<BuildingsController>().As<IBuildingsController>();
            builder.RegisterComponentInHierarchy<JoystickInputHandler>().As<IJoystickInputHandler>();
        }

        private void RegisterCharacterStateMachine(IContainerBuilder builder)
        {
            builder.Register<CharacterBehaviorIdle>(Lifetime.Singleton);
            builder.Register<CharacterBehaviorRun>(Lifetime.Singleton).As<ICharacterBehavior>();
            builder.Register<CharacterStateMachine>(Lifetime.Singleton).As<ICharacterStateMachine>().WithParameter(_characterSettings);
        }
    }
}
