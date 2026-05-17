using System;
using BattleBase.DI;
using BattleBase.Mediators;
using BattleBase.SaveService;
using UnityEngine;
using VContainer;

namespace BattleBase.Commands
{
    public sealed class CommandResetProgress : CommandBase, IInjectable
    {
        [SerializeField] private SavingMediator _savingMediator;

        private ISaver _saver;

        [Inject]
        public void Construct(ISaver saver) =>
            _saver = saver ?? throw new ArgumentNullException(nameof(saver));

        protected override void OnExecute()
        {
            _savingMediator.DisableSaving();
            _saver.ResetProgress();
        }
    }
}