using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation
{
    public interface IPositionRestrictor
    {
        public Vector3 Restrict(Vector3 desiredPosition, Vector3 currentPosition);
    }
}