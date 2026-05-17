using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation
{
    public interface ICameraBoundsLimiter
    {
        public bool IsWithinBoundsX(Vector3 position);

        public bool IsWithinBoundsZ(Vector3 position);

        public float GetOvershootX(Vector3 position);

        public float GetOvershootZ(Vector3 position);
    }
}