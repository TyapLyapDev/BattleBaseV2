using System;
using BattleBase.UpdateService;
using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation
{
    public class ScreenSizeTracker : IScreenSizeTracker, IDisposable
    {
        private static readonly UpdateType s_UpdateType = UpdateType.Update;

        private readonly IUpdater _updater;
        private int _cachedWidth;
        private int _cachedHeight;

        public ScreenSizeTracker(IUpdater updater)
        {
            _updater = updater ?? throw new ArgumentNullException(nameof(updater));

            _cachedWidth = Screen.width;
            _cachedHeight = Screen.height;

            _updater.Subscribe(OnUpdate, s_UpdateType);
        }

        public event Action SizeChanged;

        public int Width => _cachedWidth;

        public int Height => _cachedHeight;

        public void Dispose() =>
            _updater.Unsubscribe(OnUpdate, s_UpdateType);

        private void OnUpdate()
        {
            int currentWidth = Screen.width;
            int currentHeight = Screen.height;

            if (currentWidth == _cachedWidth && currentHeight == _cachedHeight)
                return;

            _cachedWidth = currentWidth;
            _cachedHeight = currentHeight;

            SizeChanged?.Invoke();
        }
    }
}