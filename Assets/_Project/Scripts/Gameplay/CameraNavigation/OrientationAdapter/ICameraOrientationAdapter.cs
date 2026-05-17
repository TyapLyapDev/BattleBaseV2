using System;

namespace BattleBase.Gameplay.CameraNavigation
{
    public interface ICameraOrientationAdapter
    {
        public event Action Changed;

        public float CurrentOrthoSize { get; }

        public float MinimumOrthoSize { get; }

        public float MaximumOrthoSize { get; }
    }
}