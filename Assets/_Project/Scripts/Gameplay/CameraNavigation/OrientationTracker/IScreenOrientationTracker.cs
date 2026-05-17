using System;

namespace BattleBase.Gameplay.CameraNavigation
{
    public interface IScreenOrientationTracker
    {
        public event Action OrientationChanged;

        public ScreenOrientationType ScreenOrientation { get; }

        public int Width { get; }

        public int Height { get; }
    }
}