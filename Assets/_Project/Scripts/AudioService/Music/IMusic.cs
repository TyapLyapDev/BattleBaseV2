using UnityEngine;

namespace BattleBase.AudioService
{
    public interface IMusic
    {
        public void Play(AudioClip clip);

        public void Stop();
    }
}