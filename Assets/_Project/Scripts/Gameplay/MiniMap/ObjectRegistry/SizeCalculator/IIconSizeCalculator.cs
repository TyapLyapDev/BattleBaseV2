using UnityEngine;

namespace BattleBase.Gameplay.MiniMap
{
    public interface IIconSizeCalculator
    {
        public Vector2 WorldToMiniMapPosition(Vector3 worldPos, Bounds areaBounds, Rect miniMapRect);

        public Vector2 GetIconSize(Vector2 worldSize, Bounds areaBounds, Rect miniMapRect);
    }
}