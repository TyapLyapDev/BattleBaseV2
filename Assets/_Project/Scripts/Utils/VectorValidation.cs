using UnityEngine;

namespace BattleBase.Utils
{
    public static class VectorValidation
    {
        public static bool IsValid(Vector3 v)
        {
            return float.IsNaN(v.x) == false
                && float.IsNaN(v.y) == false
                && float.IsNaN(v.z) == false
                && float.IsInfinity(v.x) == false
                && float.IsInfinity(v.y) == false
                && float.IsInfinity(v.z) == false;
        }
    }
}