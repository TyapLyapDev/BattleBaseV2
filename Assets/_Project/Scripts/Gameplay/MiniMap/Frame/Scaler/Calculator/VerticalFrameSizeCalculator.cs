using UnityEngine;

namespace BattleBase.Gameplay.MiniMap
{
    public class VerticalFrameSizeCalculator : IFrameSizeCalculator
    {
        public Vector2 Calculate(FrameSizeInput input)
        {
            float width = input.MiniMapAreaSize.x * (input.FrustumSize.x / input.WorldAreaSize.x);
            float height = input.MiniMapAreaSize.y * (input.FrustumSize.y / input.WorldAreaSize.y);

            return new Vector2(width, height);
        }
    }
}