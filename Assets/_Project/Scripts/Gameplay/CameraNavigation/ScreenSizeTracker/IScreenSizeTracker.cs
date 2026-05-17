using System;

namespace BattleBase.Gameplay.CameraNavigation
{
    public interface IScreenSizeTracker
    {
        public event Action SizeChanged;

        public int Width { get; }

        public int Height { get; }
    }
}