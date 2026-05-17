using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation
{
    public interface IInertiaSnapbackApplier
    {
        public void UpdateInertia(Vector3 worldDragDelta, float deltaTime);

        public void Apply(float deltaTime);

        public void ResetInertia();
    }
}