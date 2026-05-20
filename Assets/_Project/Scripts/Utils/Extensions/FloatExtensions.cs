using UnityEngine;

namespace BattleBase.Utils.Extensions
{
    public static class FloatExtensions
    {
        public static float Remap(this float value, float fromMinimum, float fromMaximum, float toMinimum = 0f, float toMaximum = 1f)
        {
            if (Mathf.Approximately(fromMinimum, fromMaximum))
                return toMinimum;

            float ratio = (value - fromMinimum) / (fromMaximum - fromMinimum);

            return toMinimum + ratio * (toMaximum - toMinimum);
        }
    }
}