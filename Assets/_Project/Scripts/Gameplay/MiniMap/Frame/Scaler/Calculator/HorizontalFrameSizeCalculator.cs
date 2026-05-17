using UnityEngine;

namespace BattleBase.Gameplay.MiniMap
{
    public class HorizontalFrameSizeCalculator : IFrameSizeCalculator
    {
        public Vector2 Calculate(FrameSizeInput input)
        {
            float width = input.MiniMapAreaSize.x * (input.FrustumSize.y / input.WorldAreaSize.y);
            float height = input.MiniMapAreaSize.y * (input.FrustumSize.x / input.WorldAreaSize.x);

            return new (width, height);
        }
    }
}