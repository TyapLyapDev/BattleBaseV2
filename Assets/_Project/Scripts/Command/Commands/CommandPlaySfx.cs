using System;
using BattleBase.AudioService;
using BattleBase.DI;
using UnityEngine;
using VContainer;

namespace BattleBase.Commands
{
    public sealed class CommandPlaySfx : CommandBase, IInjectable
    {
        [SerializeField] private AudioClip _audioClip;

        private ISfx _sfx;

        [Inject]
        public void Construct(ISfx sfx) =>
            _sfx = sfx ?? throw new ArgumentNullException(nameof(sfx));

        public override void Execute() =>
            _sfx.PlayOneShot(_audioClip);
    }
}