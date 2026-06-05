using System;

namespace BattleBase.Gameplay.CameraNavigation
{
    public interface ICameraOrientationAdapter
    {
        public event Action Changed;

        public float CurrentSize { get; }

        public float MinimumSize { get; }

        public float MaximumSize { get; }

        public void Refresh();
    }
}