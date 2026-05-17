using UnityEngine;

namespace BattleBase.AudioService
{
    public interface ISfx
    {
        public void PlayOneShot(AudioClip clip);
    }
}