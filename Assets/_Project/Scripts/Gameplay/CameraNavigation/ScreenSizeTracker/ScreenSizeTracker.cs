using System;
using BattleBase.UpdateService;
using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation
{
    public class ScreenSizeTracker : IScreenSizeTracker, IDisposable
    {
        private static readonly UpdateType s_UpdateType = UpdateType.Update;

        private readonly IUpdater _updater;
        private int _lastWidth;
        private int _lastHeight;

        public ScreenSizeTracker(IUpdater updater)
        {
            _updater = updater ?? throw new ArgumentNullException(nameof(updater));

            _lastWidth = Screen.width;
            _lastHeight = Screen.height;

            _updater.Subscribe(OnUpdate, s_UpdateType);
        }

        public event Action SizeChanged;

        public int Width => Screen.width;

        public int Height => Screen.height;

        public void Dispose() =>
            _updater.Unsubscribe(OnUpdate, s_UpdateType);

        private void OnUpdate()
        {
            int currentWidth = Screen.width;
            int currentHeight = Screen.height;

            if (currentWidth == _lastWidth && currentHeight == _lastHeight)
                return;

            _lastWidth = currentWidth;
            _lastHeight = currentHeight;

            SizeChanged?.Invoke();
        }
    }
}