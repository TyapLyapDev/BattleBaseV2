using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation
{
    public interface IDragApplier
    {
        public void Apply(Vector3 worldDelta);

        public void RestorePosition(Vector3 currentPosition, Vector3 positionToRestore);
    }
}