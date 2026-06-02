using System;
using BattleBase.AudioService;
using BattleBase.DI;
using BattleBase.SaveService;
using BattleBase.Utils.Constants;
using BattleBase.Utils.Extensions;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using VContainer;

namespace BattleBase.Mediators
{
    public class AudioVolumeMediator : MediatorBase, IInjectable, ISaveable
    {
        [SerializeField] private Slider _generalSlider;
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _sfxSlider;
        [SerializeField] private AudioMixer _mixer;

        private IAudioVolumeSaver _saver;

        [Inject]
        public void Construct(IAudioVolumeSaver saver) =>
            _saver = saver ?? throw new ArgumentNullException(nameof(saver));

        private void OnDestroy()
        {
            _generalSlider.onValueChanged.RemoveListener(OnGeneralSliderChanged);
            _musicSlider.onValueChanged.RemoveListener(OnMusicSliderChanged);
            _sfxSlider.onValueChanged.RemoveListener(OnSfxSliderChanged);
        }

        public override void Init()
        {
            _generalSlider.onValueChanged.AddListener(OnGeneralSliderChanged);
            _musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
            _sfxSlider.onValueChanged.AddListener(OnSfxSliderChanged);

            OnGeneralSliderChanged(_generalSlider.value);
            OnMusicSliderChanged(_musicSlider.value);
            OnSfxSliderChanged(_sfxSlider.value);
        }

        public void Load()
        {
            IVolumeData data = _saver.VolumeData;
            _generalSlider.value = data.GeneralVolume;
            _musicSlider.value = data.MusicVolume;
            _sfxSlider.value = data.SfxVolume;
        }

        public void Save()
        {
            VolumeData data = new(_generalSlider.value, _musicSlider.value, _sfxSlider.value);
            _saver.SetVolumeData(data);
        }

        private void SetVolume(Slider slider, string group)
        {
            float normalized = slider.value.Remap(slider.minValue, slider.maxValue);
            AudioVolumeSetter.SetNormalizedVolume(_mixer, group, normalized);
        }

        private void OnGeneralSliderChanged(float _) =>
            SetVolume(_generalSlider, AudioMixerGroupNames.General);

        private void OnMusicSliderChanged(float _) =>
            SetVolume(_musicSlider, AudioMixerGroupNames.Music);

        private void OnSfxSliderChanged(float _) =>
            SetVolume(_sfxSlider, AudioMixerGroupNames.Sfx);
    }
}