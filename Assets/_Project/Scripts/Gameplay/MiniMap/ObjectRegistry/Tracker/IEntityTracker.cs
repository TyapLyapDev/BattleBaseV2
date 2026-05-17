using System;
using UnityEngine;

namespace BattleBase.Gameplay.MiniMap
{
    public interface IEntityTracker
    {
        public event Action<IEntityTracker> Disposed;
        public event Action<IEntityTracker> ColorChanged;
        public event Action<IEntityTracker> SizeChanged;
        public event Action<IEntityTracker> PositionChanged;
        public event Action<IEntityTracker> RotationChanged;

        public Color Color { get; }

        public Vector2 WorldSize { get; }

        public Vector3 WorldPosition { get; }

        public float RotationY { get; }
    }
}