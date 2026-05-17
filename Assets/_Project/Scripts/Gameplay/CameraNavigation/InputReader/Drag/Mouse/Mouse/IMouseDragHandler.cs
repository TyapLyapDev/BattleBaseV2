using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation.InputReader
{
    public interface IMouseDragHandler
    {
        public Vector3? Update();
    }
}