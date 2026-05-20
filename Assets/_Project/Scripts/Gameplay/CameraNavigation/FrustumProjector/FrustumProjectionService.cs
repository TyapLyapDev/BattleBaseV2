using System;
using System.Collections.Generic;
using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation
{
    public class FrustumProjectionService : IFrustumProjectionService, IDisposable
    {
        private readonly FrustumProjectionCache _cache = new();
        private readonly FrustumProjectionEventSubscriber _subscriber;
        private readonly ICameraAreaService _areaService;
        private readonly ICameraTracker _cameraTracker;

        public FrustumProjectionService(ICameraAreaService areaService, ICameraTracker cameraTracker)
        {
            _areaService = areaService ?? throw new ArgumentNullException(nameof(areaService));
            _cameraTracker = cameraTracker ?? throw new ArgumentNullException(nameof(cameraTracker));

            _subscriber = new FrustumProjectionEventSubscriber(
                _areaService,
                _cameraTracker,
                RefreshCache);

            RefreshCache();
        }

        public event Action Changed;

        public IReadOnlyList<Vector3> Corners => _cache.Corners;

        public Vector3 ProjectedCenter => _cache.ProjectedCenter;

        public float CachedHeight => _cache.CachedHeight;

        public float CachedWidth => _cache.CachedWidth;

        public void Dispose() =>
            _subscriber?.Dispose();

        public void ProjectCornersOntoPlaneFromPosition(Vector3 cameraPosition, List<Vector3> outCorners)
        {
            Plane plane = new(Vector3.up, _areaService.GroundPlaneY);

            CameraProjectionUtility.GetProjectedCorners(
                _cameraTracker.Camera,
                cameraPosition,
                plane,
                _cameraTracker.CachedProjectionType,
                outCorners);
        }

        public void RefreshCache()
        {
            _cache.Recalculate(
                _cameraTracker.Camera,
                _cameraTracker.CachedPosition,
                _areaService,
                _cameraTracker.CachedProjectionType);

            Changed?.Invoke();
        }
    }
}