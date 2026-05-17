using UnityEngine;

namespace BattleBase.Gameplay.MiniMap
{
    public class VerticalFramePositionCalculator : IFramePositionCalculator
    {
        public Vector2 Calculate(FramePositionInput input)
        {
            float normalizedX = (input.WorldCenter.x - input.AreaBounds.min.x) / input.AreaBounds.size.x;
            float normalizedZ = (input.WorldCenter.z - input.AreaBounds.min.z) / input.AreaBounds.size.z;

            float x = (normalizedX - 0.5f) * input.MiniMapRect.width;
            float y = (normalizedZ - 0.5f) * input.MiniMapRect.height;

            return new(x, y);
        }
    }
}