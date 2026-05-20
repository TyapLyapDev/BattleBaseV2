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

            if (_source == null)
                throw new InvalidOperationException($"{nameof(_source)} has not been assigned");

            _source.clip = clip;
            _source.Play();
        }

        public void Stop()
        {
            if (_source == null)
                return;

            _source.Stop();
        }
    }
}