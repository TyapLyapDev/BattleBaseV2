using System;

namespace BattleBase.Gameplay.CameraNavigation
{
    public class FrustumProjectionEventSubscriber : IDisposable
    {
        private readonly Action _refreshCallback;
        private readonly ICameraAreaService _areaService;
        private readonly ICameraTracker _cameraTracker;

        public FrustumProjectionEventSubscriber(
            ICameraAreaService areaService,
            ICameraTracker cameraTracker,
            Action refreshCallback)
        {
            _areaService = areaService ?? throw new ArgumentNullException(nameof(areaService));
            _cameraTracker = cameraTracker ?? throw new ArgumentNullException(nameof(cameraTracker));
            _refreshCallback = refreshCallback ?? throw new ArgumentNullException(nameof(refreshCallback));

            Subscribe();
        }

        public void Dispose() =>
            Unsubscribe();

        private void Subscribe()
        {
            _areaService.Changed += _refreshCallback;
            _cameraTracker.PositionChanged += _refreshCallback;
            _cameraTracker.RotationChanged += _refreshCallback;
            _cameraTracker.OrthoSizeChanged += _refreshCallback;
            _cameraTracker.ProjectionChanged += _refreshCallback;
        }

        private void Unsubscribe()
        {
            _areaService.Changed -= _refreshCallback;
            _cameraTracker.PositionChanged -= _refreshCallback;
            _cameraTracker.RotationChanged -= _refreshCallback;
            _cameraTracker.OrthoSizeChanged -= _refreshCallback;
            _cameraTracker.ProjectionChanged -= _refreshCallback;
        }
    }
}