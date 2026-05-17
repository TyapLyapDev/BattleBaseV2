using UnityEngine;

namespace BattleBase.Gameplay.MiniMap
{
    public interface IFramePositionCalculator
    {
        Vector2 Calculate(FramePositionInput input);
    }
}