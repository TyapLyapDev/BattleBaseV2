using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation
{
    public interface ICameraSnapBack
    {
        public float Speed { get; }

        public void ClampByOvershoot();

        public Vector3 GetCorrectionAreaBounds(Vector3 position);
    }
}