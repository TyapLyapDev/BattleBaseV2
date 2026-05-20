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

        public override void Execute()
        {
            _savingMediator.DisableSaving();
            _saver.ResetProgress();
        }
    }
}