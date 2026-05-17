using System;
using BattleBase.UpdateService;
using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation.InputReader
{
    public class CameraInputReader : ICameraInputReader, IDisposable
    {
        private readonly IUpdater _updater;
        private readonly IDragHandler _dragHandler;
        private readonly IZoomHandler _zoomHandler;

        private bool _disposed;

        public CameraInputReader(
            IUpdater updater,
            IDragHandler dragHandler,
            IZoomHandler zoomHandler)
        {
            _updater = updater ?? throw new ArgumentNullException(nameof(updater));
            _dragHandler = dragHandler ?? throw new ArgumentNullException(nameof(dragHandler));
            _zoomHandler = zoomHandler ?? throw new ArgumentNullException(nameof(zoomHandler));

            _updater.Subscribe(OnUpdate, UpdateType.Update);
        }

        public Vector3? WorldDragDelta { get; private set; }

        public float? ZoomDelta { get; private set; }

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;
            _updater?.Unsubscribe(OnUpdate, UpdateType.Update);
        }

        private void OnUpdate(float deltaTime)
        {
            float? zoom = _zoomHandler.Update();

            if (zoom.HasValue)
            {
                ZoomDelta = zoom;
                WorldDragDelta = null;
            }
            else
            {
                WorldDragDelta = _dragHandler.Update(deltaTime);
                ZoomDelta = null;
            }
        }
    }
}