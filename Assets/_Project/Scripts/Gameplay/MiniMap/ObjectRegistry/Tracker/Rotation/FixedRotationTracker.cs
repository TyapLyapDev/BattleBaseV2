using System;
using UnityEngine;

namespace BattleBase.Gameplay.MiniMap
{
    public class FixedRotationTracker : IEntityRotationTracker
    {
        public FixedRotationTracker(Transform transform)
        {
            if (transform == null)
                throw new ArgumentNullException(nameof(transform));

            RotationY = transform.eulerAngles.y;
            Changed?.Invoke();
        }

        public event Action Changed;

        public float RotationY { get; private set; }

        public void Dispose() =>
            RotationY = 0f;
    }
}