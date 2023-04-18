using Scripts.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Scripts.Game;

namespace Scripts.Architecture
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private BankService _bankService;
        [SerializeField]
        private Character _character;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_character);
            builder.RegisterComponent(_bankService);
        }
    }
}
