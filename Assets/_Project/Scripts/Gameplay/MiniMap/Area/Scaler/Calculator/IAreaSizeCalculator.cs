using UnityEngine;

namespace BattleBase.Gameplay.MiniMap
{
    public interface IAreaSizeCalculator
    {
        public Vector2 CalculateNewSize(AreaSizeInput input);
    }
}