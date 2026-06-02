using UnityEngine;

namespace BattleBase.Utils.Extensions
{
    public static class Vector3Extensions
    {
        public static bool IsValid(this Vector3 v)
        {
            return float.IsNaN(v.x) == false
                && float.IsNaN(v.y) == false
                && float.IsNaN(v.z) == false
                && float.IsInfinity(v.x) == false
                && float.IsInfinity(v.y) == false
                && float.IsInfinity(v.z) == false;
        }

        public static bool IsWithinDistance(this Vector3 start, Vector3 end, float distance)
        {
            float sqrMagnitude = (start - end).sqrMagnitude;

            return sqrMagnitude <= distance * distance;
        }
    }
}