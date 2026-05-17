using System;

namespace BattleBase.Gameplay.MiniMap
{
    public interface IEntityRotationTracker : IDisposable
    {
        public event Action Changed;

        public float RotationY { get; }
    }
}