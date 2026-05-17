using UnityEngine;

namespace BattleBase.Gameplay.MiniMap
{
    public class VerticalAreaSizeCalculator : IAreaSizeCalculator
    {
        public Vector2 CalculateNewSize(AreaSizeInput input)
        {
            float aspect = input.WorldSize.y / input.WorldSize.x;
            float newHeight = input.CurrentMiniMapSize.x * aspect;

            return new(input.CurrentMiniMapSize.x, newHeight);
        }
    }
}