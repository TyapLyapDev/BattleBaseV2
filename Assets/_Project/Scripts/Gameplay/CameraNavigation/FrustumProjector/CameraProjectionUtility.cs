using System;
using System.Collections.Generic;
using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation
{
    public static class CameraProjectionUtility
    {
        private const float NearClipOffset = 0.01f;

        public static readonly Vector3[] ViewportCorners = new Vector3[]
        {
            new(0, 0, NearClipOffset),
            new(1, 0, NearClipOffset),
            new(1, 1, NearClipOffset),
            new(0, 1, NearClipOffset),
        };

        public static Vector3 ProjectPointOntoPlane(
            CameraProjectionType projectionType,
            Vector3 cameraPosition,
            Vector3 cameraForward,
            Vector3 worldPoint,
            Plane plane)
        {
            return projectionType switch
            {
                CameraProjectionType.Orthographic => ProjectOrthographic(worldPoint, cameraForward, plane),
                CameraProjectionType.Perspective => ProjectPerspective(cameraPosition, worldPoint, plane),
                _ => throw new ArgumentOutOfRangeException(nameof(projectionType), projectionType, $"Unsupported projection type: {projectionType}")
            };
        }

        public static void GetProjectedCorners(
            Camera camera,
            Vector3 cameraPosition,
            Plane targetPlane,
            CameraProjectionType projectionType,
            List<Vector3> outCorners)
        {
            if (camera == null)
                throw new ArgumentNullException(nameof(camera));

            if (outCorners == null)
                throw new ArgumentNullException(nameof(outCorners));

            outCorners.Clear();

            Transform originalTransform = camera.transform;
            originalTransform.GetPositionAndRotation(out Vector3 originalPosition, out Quaternion originalRotation);
            originalTransform.position = cameraPosition;

            Vector3 cameraForward = camera.transform.forward;

            foreach (Vector3 viewportCorner in ViewportCorners)
            {
                Vector3 worldCorner = camera.ViewportToWorldPoint(viewportCorner);
                Vector3 projected = ProjectPointOntoPlane(
                    projectionType,
                    cameraPosition,
                    cameraForward,
                    worldCorner,
                    targetPlane);

                outCorners.Add(projected);
            }

            originalTransform.SetPositionAndRotation(originalPosition, originalRotation);
        }

        private static Vector3 ProjectOrthographic(Vector3 worldPoint, Vector3 cameraForward, Plane plane)
        {
            Ray ray = new(worldPoint, cameraForward);

            return plane.Raycast(ray, out float distance) ? ray.GetPoint(distance) : worldPoint;
        }

        private static Vector3 ProjectPerspective(Vector3 cameraPosition, Vector3 worldPoint, Plane plane)
        {
            Vector3 direction = (worldPoint - cameraPosition).normalized;
            Ray ray = new(cameraPosition, direction);

            return plane.Raycast(ray, out float distance) ? ray.GetPoint(distance) : worldPoint;
        }
    }
}