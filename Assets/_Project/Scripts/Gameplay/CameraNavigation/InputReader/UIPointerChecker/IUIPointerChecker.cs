using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation.InputReader
{
    public interface IUIPointerChecker
    {
        public void AddCanvas(Canvas canvas);

        public bool IsPointerOverUI(Vector2 screenPosition);
    }
}