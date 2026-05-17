using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation.InputReader
{
    public interface ICameraInputReader
    {
        public Vector3? WorldDragDelta { get; }

        public float? ZoomDelta { get; }
    }
}