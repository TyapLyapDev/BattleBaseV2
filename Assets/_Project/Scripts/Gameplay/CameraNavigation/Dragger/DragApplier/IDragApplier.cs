using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation
{
    public interface IDragApplier
    {
        public void Apply(Vector3 worldDelta);
    }
}