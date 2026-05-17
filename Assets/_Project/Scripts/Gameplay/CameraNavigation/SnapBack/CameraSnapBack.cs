using System;
using System.Collections.Generic;
using BattleBase.Utils;
using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation
{
    public class CameraSnapBack : ICameraSnapBack
    {
        private const float Half = 0.5f;

        private readonly Transform _cameraRig;
        private readonly IFrustumProjectionService _frustumProjectionService;
        private readonly ICameraAreaService _cameraAreaService;
        private readonly ICameraSnapBackConfig _snapBackConfig;

        public CameraSnapBack(
            CameraRig cameraRig,
            IFrustumProjectionService frustumProjectionService,
            ICameraAreaService cameraAreaService,
            ICameraSnapBackConfig snapBackConfig)
        {
            _cameraRig = cameraRig != null ? cameraRig.transform : throw new ArgumentNullException(nameof(cameraRig));
            _frustumProjectionService = frustumProjectionService ?? throw new ArgumentNullException(nameof(frustumProjectionService));
            _cameraAreaService = cameraAreaService ?? throw new ArgumentNullException(nameof(cameraAreaService));
            _snapBackConfig = snapBackConfig ?? throw new ArgumentNullException(nameof(snapBackConfig));
        }

        public float Speed => _snapBackConfig.SnapBackSpeed;

        public void ClampByOvershoot()
        {
            Vector3 position = _cameraRig.position;
            Vector3 correction = GetCorrectionOvershootBounds(position);
            _cameraRig.position = position + correction;
        }

        public Vector3 GetCorrectionAreaBounds(Vector3 position) =>
            GetCorrectionBounds(_cameraAreaService.AreaBounds, position);

        public Vector3 GetCorrectionOvershootBounds(Vector3 position) =>
            GetCorrectionBounds(_cameraAreaService.OvershootBounds, position);

        public Vector3 GetCorrectionBounds(Bounds bounds, Vector3 position)
        {
            if (VectorValidation.IsValid(position) == false)
                throw new ArgumentException($"Position is invalid (NaN or Infinity): {position}", nameof(position));

            List<Vector3> corners = new();
            _frustumProjectionService.ProjectCornersOntoPlaneFromPosition(position, corners);

            Vector2 minMaxX = GetMinMax(corners, true);
            Vector2 minMaxZ = GetMinMax(corners, false);
            Vector3 correction = Vector3.zero;

            correction.x = CalculateCorrection(
                minMaxX.x, minMaxX.y,
                bounds.min.x, bounds.max.x,
                bounds.center.x);

            correction.z = CalculateCorrection(
                minMaxZ.x, minMaxZ.y,
                bounds.min.z, bounds.max.z,
                bounds.center.z);

            return correction;
        }

        private Vector2 GetMinMax(List<Vector3> corners, bool isX)
        {
            float min = float.MaxValue;
            float max = float.MinValue;

            foreach (Vector3 corner in corners)
            {
                float value = isX ? corner.x : corner.z;

                if (value < min)
                    min = value;

                if (value > max)
                    max = value;
            }

            return new Vector2(min, max);
        }

        private float CalculateCorrection(float min, float max, float boundMin, float boundMax, float boundCenter)
        {
            bool minInside = min >= boundMin;
            bool maxInside = max <= boundMax;

            if (minInside == false && maxInside == false)
            {
                float frustumCenter = (min + max) * Half;

                return boundCenter - frustumCenter;
            }

            if (minInside == false)
                return boundMin - min;

            if (maxInside == false)
                return boundMax - max;

            return 0f;
        }
    }
}