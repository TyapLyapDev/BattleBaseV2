using System;

namespace BattleBase.Gameplay.CameraNavigation
{
    public class FrustumProjectionService : IFrustumProjectionService, IDisposable
    {
        private readonly FrustumProjectionEventSubscriber _subscriber;
        private readonly ICameraArea _area;
        private readonly ICameraHandle _cameraTracker;

        public FrustumProjectionService(ICameraArea area, ICameraHandle cameraTracker)
        {
            _area = area ?? throw new ArgumentNullException(nameof(area));
            _cameraTracker = cameraTracker ?? throw new ArgumentNullException(nameof(cameraTracker));

            _subscriber = new FrustumProjectionEventSubscriber(
                _area,
                _cameraTracker,
                Refresh);
        }

        public event Action Changed;

        public FrustumProjection Projection { get; private set; }

        public void Dispose() =>
            _subscriber?.Dispose();

        public GroundProjection GetProjection(FrustumSizeType frustumSize, FrustumShape shape)
        {
            return CameraProjectionUtility.ConvertProjection(
                Projection,
                _area.CameraRig.transform, 
                frustumSize, 
                shape);
        }

        public void Refresh()
        {
            Projection = CameraProjectionUtility.GetFrustumProjection(
                _cameraTracker.Camera,
                _area.GroundPlane,
                _cameraTracker.ProjectionType);

            Changed?.Invoke();
        }
    }
}