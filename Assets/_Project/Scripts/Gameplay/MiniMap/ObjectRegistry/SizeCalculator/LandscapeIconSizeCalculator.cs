using UnityEngine;

namespace BattleBase.Gameplay.MiniMap
{
    public class LandscapeIconSizeCalculator : IIconSizeCalculator
    {
        public Vector2 WorldToMiniMapPosition(Vector3 worldPosition, Bounds areaBounds, Rect miniMapRect)
        {
            if (miniMapRect.width <= 0f || miniMapRect.height <= 0f)
                return Vector2.zero;

            float normX = (worldPosition.z - areaBounds.min.z) / areaBounds.size.z;
            float normY = 1f - (worldPosition.x - areaBounds.min.x) / areaBounds.size.x;

            float x = (normX - 0.5f) * miniMapRect.width;
            float y = (normY - 0.5f) * miniMapRect.height;

            return new Vector2(x, y);
        }

        public Vector2 GetIconSize(Vector2 worldSize, Bounds areaBounds, Rect miniMapRect)
        {
            if (miniMapRect.width <= 0f || miniMapRect.height <= 0f)
                return Vector2.zero;

            float sizeX = (worldSize.y / areaBounds.size.z) * miniMapRect.width;
            float sizeY = (worldSize.x / areaBounds.size.x) * miniMapRect.height;

            return new Vector2(sizeX, sizeY);
        }
    }
}