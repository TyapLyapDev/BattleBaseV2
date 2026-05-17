using System;

namespace BattleBase.Gameplay.CameraNavigation
{
    public class ScreenOrientationTracker : IScreenOrientationTracker, IDisposable
    {
        private readonly IScreenSizeTracker _screenSizeTracker;

        public ScreenOrientationTracker(IScreenSizeTracker screenSizeTracker)
        {
            _screenSizeTracker = screenSizeTracker ?? throw new ArgumentNullException(nameof(screenSizeTracker));

            ScreenOrientation = GetScreenOrientation();
            _screenSizeTracker.SizeChanged += OnSizeChanged;
        }

        public event Action OrientationChanged;

        public ScreenOrientationType ScreenOrientation { get; private set; }

        public int Width => _screenSizeTracker.Width;

        public int Height => _screenSizeTracker.Height;

        public void Dispose() =>
            _screenSizeTracker.SizeChanged -= OnSizeChanged;

        private ScreenOrientationType GetScreenOrientation()
        {
            if(_screenSizeTracker.Height > _screenSizeTracker.Width)
                return ScreenOrientationType.Portrait;
            else
                return ScreenOrientationType.Landscape;
        }

        private void OnSizeChanged()
        {
            ScreenOrientationType currentScreenOrientation = GetScreenOrientation();

            if (currentScreenOrientation == ScreenOrientation)
                return;

            ScreenOrientation = currentScreenOrientation;
            OrientationChanged?.Invoke();
        }
    }
}