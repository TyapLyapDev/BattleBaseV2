using System;
using UnityEngine;
using UnityEngine.Audio;

namespace BattleBase.AudioService
{
    public static class AudioVolumeSetter
    {
        private const float MinimumLevel = -80;
        private const float MaximumLevel = 20;

        public static void SetNormalizedVolume(AudioMixer mixer, string group, float normalized)
        {
            if (mixer == null)
                throw new ArgumentNullException(nameof(mixer));

            if (string.IsNullOrEmpty(group))
                throw new ArgumentNullException(nameof(group));

            normalized = Mathf.Clamp01(normalized);
            float level = ConvertNormalizedToLevel(normalized);
            mixer.SetFloat(group, level);
        }

        private static float ConvertNormalizedToLevel(float normalized) =>
            normalized <= 0 ? MinimumLevel : Mathf.Log10(normalized) * MaximumLevel;
    }
}