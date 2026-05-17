using System;
using UnityEngine;

namespace BattleBase.SaveService
{
    [Serializable]
    public class VolumeData : IVolumeData
    {
        [SerializeField] private float _generalVolume = 1f;
        [SerializeField] private float _musicVolume = 0.6f;
        [SerializeField] private float _sfxVolume = 0.8f;

        public VolumeData() { }

        public VolumeData(float generalVolume, float musicVolume, float sfxVolume)
        {
            _generalVolume = generalVolume;
            _musicVolume = musicVolume;
            _sfxVolume = sfxVolume;
        }

        public float GeneralVolume => _generalVolume;

        public float MusicVolume => _musicVolume;

        public float SfxVolume => _sfxVolume;

        public void SetData(IVolumeData data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            _generalVolume = data.GeneralVolume;
            _musicVolume = data.MusicVolume;
            _sfxVolume = data.SfxVolume;
        }
    }
}