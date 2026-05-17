using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation
{
    public interface IResistanceCalculator
    {
        public Vector3 Calculate(Vector3 delta, Vector3 desiredPosition);
    }
}