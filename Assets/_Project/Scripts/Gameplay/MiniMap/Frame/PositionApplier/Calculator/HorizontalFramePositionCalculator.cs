using UnityEngine;

namespace BattleBase.Gameplay.MiniMap
{
    public class HorizontalFramePositionCalculator : IFramePositionCalculator
    {
        public Vector2 Calculate(FramePositionInput input)
        {
            float normalizedX = (input.WorldCenter.z - input.AreaBounds.min.z) / input.AreaBounds.size.z;
            float normalizedZ = (input.WorldCenter.x - input.AreaBounds.min.x) / input.AreaBounds.size.x;

            float x = (normalizedX - 0.5f) * input.MiniMapRect.width;
            float y = (0.5f - normalizedZ) * input.MiniMapRect.height;

            return new(x, y);
        }
    }
}