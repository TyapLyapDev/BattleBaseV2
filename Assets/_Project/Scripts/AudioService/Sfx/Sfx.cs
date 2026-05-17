using System;
using UnityEngine;

namespace BattleBase.AudioService
{
    public class Sfx : MonoBehaviour, ISfx
    {
        [SerializeField] private AudioSource _source;

        public void PlayOneShot(AudioClip clip)
        {
            if (clip == null)
                throw new ArgumentNullException(nameof(clip));

            _source.PlayOneShot(clip);
        }
    }
}