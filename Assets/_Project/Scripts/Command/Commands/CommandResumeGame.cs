using System;
using BattleBase.DI;
using BattleBase.PauseService;
using VContainer;

namespace BattleBase.Commands
{
    public sealed class CommandResumeGame : CommandBase, IInjectable
    {
        private IPauseSwitcher _pauseSwitcher;

        [Inject]
        public void Construct(IPauseSwitcher pauseSwitcher) =>
            _pauseSwitcher = pauseSwitcher ?? throw new ArgumentNullException(nameof(pauseSwitcher));

        protected override void OnExecute() =>
            _pauseSwitcher.Resume();
    }
}