using UnityEngine;

namespace BattleBase.Gameplay.MiniMap
{
    public interface IFrameSizeCalculator
    {
        public Vector2 Calculate(FrameSizeInput input);
    }
}