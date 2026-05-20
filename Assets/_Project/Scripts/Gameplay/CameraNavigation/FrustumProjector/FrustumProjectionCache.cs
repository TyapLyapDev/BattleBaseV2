using System;
using System.Collections.Generic;
using BattleBase.Utils.Extensions;
using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation
{
    public class FrustumProjectionCache
    {
        private const int CornerCount = 4;

        private readonly List<Vector3> _cornersList = new(CornerCount);

        public IReadOnlyList<Vector3> Corners => _cornersList;

        public Vector3 ProjectedCenter { get; private set; }

        public float CachedWidth { get; private set; }

        public float CachedHeight { get; private set; }

        public void Recalculate(
            Camera camera,
            Vector3 cameraPosition,
            ICameraAreaService areaService,
            CameraProjectionType projectionType)
        {
            ValidateArguments(camera, areaService);
            UpdateProjectedCorners(camera, cameraPosition, areaService, projectionType);
            UpdateBoundsAndCenter();
        }

        private void ValidateArguments(Camera camera, ICameraAreaService areaService)
        {
            if (camera == null)
                throw new ArgumentNullException(nameof(camera));

            if (areaService == null)
                throw new ArgumentNullException(nameof(areaService));
        }

        private void UpdateProjectedCorners(
            Camera camera,
            Vector3 cameraPosition,
            ICameraAreaService areaService,
            CameraProjectionType projectionType)
        {
            Plane plane = new(Vector3.up, areaService.GroundPlaneY);

            CameraProjectionUtility.GetProjectedCorners(
                camera,
                cameraPosition,
                plane,
                projectionType,
                _cornersList);
        }

        private void UpdateBoundsAndCenter()
        {
            if (Corners.Count >= 4)
            {
                CornerBounds bounds = GetCornerBounds();
                CachedWidth = bounds.MaxX - bounds.MinX;
                CachedHeight = bounds.MaxZ - bounds.MinZ;
                ProjectedCenter = Corners.Average();
            }
            else
            {
                CachedWidth = 0f;
                CachedHeight = 0f;
                ProjectedCenter = Vector3.zero;
            }
        }

        private CornerBounds GetCornerBounds()
        {
            float minX = float.MaxValue, maxX = float.MinValue;
            float minZ = float.MaxValue, maxZ = float.MinValue;

            foreach (Vector3 corner in Corners)
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

            return new (minX, maxX, minZ, maxZ);
        }

        private Vector3 GetCenter()
        {
            Vector3 sum = Vector3.zero;

            foreach (Vector3 corner in Corners)
                sum += corner;

            return sum / Corners.Count;
        }
    }
}