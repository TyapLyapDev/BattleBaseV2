using System;
using BattleBase.AdvService;
using BattleBase.DI;
using VContainer;

namespace BattleBase.Commands
{
    public sealed class CommandShowInterstitialAdv : CommandBase, IInjectable
    {
        private IAdvService _advService;

        [Inject]
        public void Construct(IAdvService advService) =>
            _advService = advService ?? throw new ArgumentNullException(nameof(advService));

        protected override void OnExecute() =>
            _advService.ShowInterstitialAdv();
    }
}