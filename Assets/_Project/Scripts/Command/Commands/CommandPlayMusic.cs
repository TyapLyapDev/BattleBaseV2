using System;
using BattleBase.AudioService;
using BattleBase.DI;
using UnityEngine;
using VContainer;

namespace BattleBase.Commands
{
    public sealed class CommandPlayMusic : CommandBase, IInjectable
    {
        [SerializeField] private AudioClip _audioClip;

        private IMusic _music;

        [Inject]
        public void Construct(IMusic music) =>
            _music = music ?? throw new ArgumentNullException(nameof(music));

        public override void Execute() =>
            _music.Play(_audioClip);
    }
}