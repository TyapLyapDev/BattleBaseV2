using System;
using BattleBase.AdvService;
using BattleBase.DI;
using VContainer;

namespace BattleBase.Commands
{
    public sealed class CommandShowInterstitialAdv : CommandBase, IInjectable
    {
        private IAdvertisingService _advService;

        [Inject]
        public void Construct(IAdvertisingService advService) =>
            _advService = advService ?? throw new ArgumentNullException(nameof(advService));

        public override void Execute() =>
            _advService.ShowInterstitialAdv();
    }
}