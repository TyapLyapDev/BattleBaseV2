using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation
{
    public interface IResistanceCalculator
    {
        public float Resistance { get; }

        public float GetMaximumOvershoot(ResistanceAxis axis);

        public float GetOvershoot(ResistanceAxis axis);

        public Vector3 Calculate(Vector3 delta, Vector3 desiredPosition);
    }
}