using UnityEngine;

namespace BattleBase.Gameplay.MiniMap
{
    public class PortraitIconSizeCalculator : IIconSizeCalculator
    {
        public Vector2 WorldToMiniMapPosition(Vector3 worldPos, Bounds areaBounds, Rect miniMapRect)
        {
            if (miniMapRect.width <= 0f || miniMapRect.height <= 0f)
                return Vector2.zero;

            float normX = (worldPos.x - areaBounds.min.x) / areaBounds.size.x;
            float normY = (worldPos.z - areaBounds.min.z) / areaBounds.size.z;

            float x = (normX - 0.5f) * miniMapRect.width;
            float y = (normY - 0.5f) * miniMapRect.height;

            return new Vector2(x, y);
        }

        public Vector2 GetIconSize(Vector2 worldSize, Bounds areaBounds, Rect miniMapRect)
        {
            if (miniMapRect.width <= 0f || miniMapRect.height <= 0f)
                return Vector2.zero;

            float sizeX = (worldSize.x / areaBounds.size.x) * miniMapRect.width;
            float sizeY = (worldSize.y / areaBounds.size.z) * miniMapRect.height;

            return new Vector2(sizeX, sizeY);
        }
    }
}