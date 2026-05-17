using UnityEngine;

namespace BattleBase.Gameplay.MiniMap
{
    public class HorizontalAreaSizeCalculator : IAreaSizeCalculator
    {
        public Vector2 CalculateNewSize(AreaSizeInput input)
        {
            float aspect = input.WorldSize.y / input.WorldSize.x;
            float newWidth = input.CurrentMiniMapSize.y * aspect;

            return new(newWidth, input.CurrentMiniMapSize.y);
        }
    }
}