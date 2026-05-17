using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace BattleBase.AudioService
{
    public class VolumeModifier : IDisposable
    {
        private const float MinimumLevel = -80;
        private const float MaximumLevel = 20;

        private readonly AudioMixer _mixer;
        private readonly Slider _slider;
        private readonly string _group;

        private float _minimumValueSlider;
        private float _maximumValueSlider;

        public VolumeModifier(AudioMixer mixer, Slider slider, string group)
        {
            _mixer = mixer != null ? mixer : throw new ArgumentNullException(nameof(mixer));
            _slider = slider != null ? slider : throw new ArgumentNullException(nameof(slider));

            if (string.IsNullOrEmpty(group))
                throw new ArgumentNullException($"{nameof(group)} is null or empty");

            _group = group;

            Init();
        }

        public void Dispose()
        {
            if (_slider != null)
                _slider.onValueChanged.RemoveListener(OnChanged);
        }

        private void Init()
        {
            _minimumValueSlider = _slider.minValue;
            _maximumValueSlider = _slider.maxValue;
            SetLevel(_slider.value);
            _slider.onValueChanged.AddListener(OnChanged);
        }

        private void SetLevel(float value)
        {
            float level = ConvertVolumeToLevel(NormalizeValue(value));
            _mixer.SetFloat(_group, level);
        }

        private float ConvertVolumeToLevel(float value) =>
            value == 0 ? MinimumLevel : Mathf.Log10(value) * MaximumLevel;

        private float NormalizeValue(float value) =>
            Mathf.InverseLerp(_minimumValueSlider, _maximumValueSlider, value);

        private void OnChanged(float value) =>
            SetLevel(value);
    }
}