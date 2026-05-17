using System;
using UnityEngine;

namespace BattleBase.AudioService
{
    public class Music : MonoBehaviour, IMusic
    {
        [SerializeField] private AudioSource _source;

        public void Play(AudioClip clip)
        {
            if (clip == null)
                throw new ArgumentNullException(nameof(clip));

            ValidateSource();

            _source.clip = clip;
            _source.Play();
        }

        public void Stop()
        {
            ValidateSource();
            _source.Stop();
        }

        private void ValidateSource()
        {
            if (_source == null)
                throw new InvalidOperationException($"{nameof(_source)} has not been assigned.");
        }
    }
}