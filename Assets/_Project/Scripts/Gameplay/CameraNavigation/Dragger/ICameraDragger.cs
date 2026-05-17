using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation
{
    public interface ICameraDragger
    {
        public void Enable();

        public void Disable();

        public void Update(float deltaTime, Vector3? worldDragDelta);
    }
}