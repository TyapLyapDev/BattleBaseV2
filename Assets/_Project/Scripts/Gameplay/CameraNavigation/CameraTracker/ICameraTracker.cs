using System;
using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation
{
    public interface ICameraTracker
    {
        public event Action PositionChanged;
        public event Action RotationChanged;
        public event Action OrthoSizeChanged;

        public Vector3 CachedPosition { get; }

        public Quaternion CachedRotation { get; }

        public float CachedOrthoSize { get; }
    }
}