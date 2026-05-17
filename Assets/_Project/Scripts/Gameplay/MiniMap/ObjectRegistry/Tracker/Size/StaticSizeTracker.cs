using System;
using UnityEngine;

namespace BattleBase.Gameplay.MiniMap
{
    public class StaticSizeTracker : IEntitySizeTracker
    {
        public StaticSizeTracker(Transform transform, IEntitySizeCalculator sizeCalculator)
        {
            if (sizeCalculator == null)
                throw new ArgumentNullException(nameof(sizeCalculator));

            WorldSize = sizeCalculator.Calculate(transform);

            Changed?.Invoke();
        }

        public event Action Changed;

        public Vector2 WorldSize { get; private set; }

        public void Dispose() =>
            WorldSize = Vector2.zero;
    }
}