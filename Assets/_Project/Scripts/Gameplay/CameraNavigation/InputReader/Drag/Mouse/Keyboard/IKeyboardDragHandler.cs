using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation.InputReader
{
    public interface IKeyboardDragHandler
    {
        public Vector3? Update(float deltaTime);
    }
}