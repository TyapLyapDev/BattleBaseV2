using UnityEngine;

namespace BattleBase.Utils
{
    public static class VectorTool
    {
        public static bool IsWithinDistance(Vector3 startVector, Vector3 finishVector, float distance)
        {
            Vector3 direction = startVector - finishVector;
            float sqrMagnitude = direction.sqrMagnitude;

            return sqrMagnitude <= distance * distance;
        }
    }
}