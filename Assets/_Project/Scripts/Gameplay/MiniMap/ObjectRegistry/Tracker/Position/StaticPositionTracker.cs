using System;
using UnityEngine;

namespace BattleBase.Gameplay.MiniMap
{
    public class StaticPositionTracker : IEntityPositionTracker
    {
        public StaticPositionTracker(Transform transform)
        {
            if (transform == null)
                throw new ArgumentNullException(nameof(transform));

            WorldPosition = transform.position;
            Changed?.Invoke();
        }

        public event Action Changed;

        public Vector3 WorldPosition { get; private set; }

        public void Dispose() =>
            WorldPosition = Vector3.zero;
    }
}