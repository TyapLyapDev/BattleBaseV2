using System;
using System.Collections.Generic;
using BattleBase.Utils;
using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation
{
    public class CameraBoundsLimiter : ICameraBoundsLimiter
    {
        private readonly IFrustumProjectionService _frustumProjectionService;
        private readonly ICameraAreaService _cameraAreaService;

        public CameraBoundsLimiter(IFrustumProjectionService frustumProjectionService, ICameraAreaService cameraAreaService)
        {
            _frustumProjectionService = frustumProjectionService ?? throw new ArgumentNullException(nameof(frustumProjectionService));
            _cameraAreaService = cameraAreaService ?? throw new ArgumentNullException(nameof(cameraAreaService));
        }

        public bool IsWithinBoundsX(Vector3 position) =>
            IsValidPositionAlongAxis(position, c => c.x, bounds => bounds.min.x, bounds => bounds.max.x);

        public bool IsWithinBoundsZ(Vector3 position) =>
            IsValidPositionAlongAxis(position, c => c.z, bounds => bounds.min.z, bounds => bounds.max.z);

        public float GetOvershootX(Vector3 position) =>
            GetOvershootAlongAxis(position, c => c.x, b => b.min.x, b => b.max.x);

        public float GetOvershootZ(Vector3 position) =>
            GetOvershootAlongAxis(position, c => c.z, b => b.min.z, b => b.max.z);

        private bool IsValidPositionAlongAxis(
            Vector3 position,
            Func<Vector3, float> getCornerCoord,
            Func<Bounds, float> getMinBound,
            Func<Bounds, float> getMaxBound)
        {
            CornerBounds bounds = GetCornerBounds(position);
            Bounds areaBounds = _cameraAreaService.OvershootBounds;
            float minBound = getMinBound(areaBounds);
            float maxBound = getMaxBound(areaBounds);

            if (getCornerCoord(new Vector3(bounds.MinX, 0, 0)) == bounds.MinX)
                return bounds.MinX >= minBound && bounds.MaxX <= maxBound;
            else
                return bounds.MinZ >= minBound && bounds.MaxZ <= maxBound;
        }

        private float GetOvershootAlongAxis(
            Vector3 position,
            Func<Vector3, float> getCoord,
            Func<Bounds, float> getMin,
            Func<Bounds, float> getMax)
        {
            CornerBounds bounds = GetCornerBounds(position);
            Bounds areaBounds = _cameraAreaService.AreaBounds;
            float boundMin = getMin(areaBounds);
            float boundMax = getMax(areaBounds);
            float min = getCoord(new Vector3(bounds.MinX, 0, bounds.MinZ));
            float max = getCoord(new Vector3(bounds.MaxX, 0, bounds.MaxZ));

            float overshoot = 0f;

            if (min < boundMin)
                overshoot = boundMin - min;

            if (max > boundMax)
                overshoot = Mathf.Max(overshoot, max - boundMax);

            return overshoot;
        }

        private CornerBounds GetCornerBounds(Vector3 position)
        {
            if (VectorValidation.IsValid(position) == false)
                throw new ArgumentException($"Position is invalid (NaN or Infinity): {position}", nameof(position));

            List<Vector3> corners = new();
            _frustumProjectionService.ProjectCornersOntoPlaneFromPosition(position, corners);

            float minX = float.MaxValue, maxX = float.MinValue;
            float minZ = float.MaxValue, maxZ = float.MinValue;

            foreach (Vector3 corner in corners)
            {
                if (corner.x < minX)
                    minX = corner.x;

                if (corner.x > maxX)
                    maxX = corner.x;

                if (corner.z < minZ)
                    minZ = corner.z;

                if (corner.z > maxZ)
                    maxZ = corner.z;
            }

            return new CornerBounds(minX, maxX, minZ, maxZ);
        }
    }
}