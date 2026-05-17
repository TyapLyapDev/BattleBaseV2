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

        private IAudioService _audioService;

        [Inject]
        public void Construct(IAudioService audioService) =>
            _audioService = audioService ?? throw new ArgumentNullException(nameof(audioService));

        protected override void OnExecute() =>
            _audioService.Sfx.PlayOneShot(_audioClip);
    }
}