using UnityEngine;

namespace BattleBase.Gameplay.MiniMap
{
    public interface IEntitySizeCalculator
    {
        public Vector2 Calculate(Transform transform);
    }
}