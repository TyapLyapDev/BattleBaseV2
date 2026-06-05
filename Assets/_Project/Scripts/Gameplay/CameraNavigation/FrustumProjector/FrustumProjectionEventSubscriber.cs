using System;

namespace BattleBase.Gameplay.CameraNavigation
{
    public class FrustumProjectionEventSubscriber : IDisposable
    {
        private readonly Action _refreshCallback;
        private readonly ICameraArea _area;
        private readonly ICameraHandle _cameraTracker;

        public FrustumProjectionEventSubscriber(
            ICameraArea area,
            ICameraHandle cameraTracker,
            Action refreshCallback)
        {
            _area = area ?? throw new ArgumentNullException(nameof(area));
            _cameraTracker = cameraTracker ?? throw new ArgumentNullException(nameof(cameraTracker));
            _refreshCallback = refreshCallback ?? throw new ArgumentNullException(nameof(refreshCallback));

            Subscribe();
            OnChanged();
        }

        public void Dispose() =>
            Unsubscribe();

        private void Subscribe()
        {
            _area.Changed += OnChanged;
            _cameraTracker.PositionChanged += OnChanged;
            _cameraTracker.RotationChanged += OnChanged;
            _cameraTracker.SizeChanged += OnChanged;
            _cameraTracker.ProjectionChanged += OnChanged;
        }

        private void Unsubscribe()
        {
            _area.Changed -= OnChanged;
            _cameraTracker.PositionChanged -= OnChanged;
            _cameraTracker.RotationChanged -= OnChanged;
            _cameraTracker.SizeChanged -= OnChanged;
            _cameraTracker.ProjectionChanged -= OnChanged;
        }

        private void OnChanged() =>
            _refreshCallback?.Invoke();
    }
}