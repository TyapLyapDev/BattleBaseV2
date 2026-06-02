using System;
using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation
{
    public static class CameraProjectionUtility
    {
        private const float NearClipOffset = 0.01f;
        private const float ParallelRayEpsilon = 1e-6f;
        private const float CenterAverageDivisor = 4f;

        public static readonly Vector3[] ViewportCorners = new Vector3[]
        {
            new(0, 0, NearClipOffset),
            new(1, 0, NearClipOffset),
            new(1, 1, NearClipOffset),
            new(0, 1, NearClipOffset),
        };

        public static FrustumProjection GetFrustumProjection(
            Camera camera,
            Plane targetPlane,
            CameraProjectionType projectionType)
        {
            if (camera == null)
                throw new ArgumentNullException(nameof(camera));

            return projectionType switch
            {
                CameraProjectionType.Perspective => GetPerspectiveProjection(camera, targetPlane),
                CameraProjectionType.Orthographic => GetOrthographicProjection(camera, targetPlane),
                _ => throw new NotImplementedException(),
            };
        }

        public static GroundProjection ConvertProjection(
            FrustumProjection projection,
            Transform cameraRig,
            FrustumSizeType frustumSize,
            FrustumShape shape)
        {
            Vector3 forward = cameraRig.forward;
            Vector3 right = cameraRig.right;
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            float width;
            float height;

            switch (frustumSize)
            {
                case FrustumSizeType.MinimumWidthAndHeight:
                    width = Mathf.Min(projection.BottomWidth, projection.TopWidth);
                    height = Mathf.Min(projection.LeftHeight, projection.RightHeight);
                    break;

                case FrustumSizeType.MaximumWidthAndHeight:
                    width = Mathf.Max(projection.BottomWidth, projection.TopWidth);
                    height = Mathf.Max(projection.LeftHeight, projection.RightHeight);
                    break;

                case FrustumSizeType.MinimumWidthAndMaximumHeight:
                    width = Mathf.Min(projection.BottomWidth, projection.TopWidth);
                    height = Mathf.Max(projection.LeftHeight, projection.RightHeight);
                    break;

                case FrustumSizeType.MaximumWidthAndMinimumHeight:
                    width = Mathf.Max(projection.BottomWidth, projection.TopWidth);
                    height = Mathf.Min(projection.LeftHeight, projection.RightHeight);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(frustumSize), frustumSize, null);
            }

            Vector3 center = projection.Center;

            if (shape == FrustumShape.Rectangle)
            {
                Vector3 halfRight = right * (width * 0.5f);
                Vector3 halfForward = forward * (height * 0.5f);

                Vector3 leftDown = center - halfRight - halfForward;
                Vector3 leftUp = center - halfRight + halfForward;
                Vector3 rightDown = center + halfRight - halfForward;
                Vector3 rightUp = center + halfRight + halfForward;

                int cornerCount = 4;

                Vector3 rectangleCenter = (leftUp + leftDown + rightUp + rightDown) / cornerCount;

                return new(
                    leftUp,
                    leftDown,
                    rightUp,
                    rightDown,
                    rectangleCenter,
                    width, height
                );
            }
            else if (shape == FrustumShape.Trapezoid)
            {
                return new(
                    projection.LeftUp,
                    projection.LeftDown,
                    projection.RightUp,
                    projection.RightDown,
                    projection.Center,
                    width,
                    height);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(shape));
            }
        }

        private static FrustumProjection GetOrthographicProjection(Camera camera, Plane targetPlane)
        {
            int cornersCount = ViewportCorners.Length;
            Span<Vector3> corners = stackalloc Vector3[cornersCount];
            Vector3 cameraForward = camera.transform.forward;

            for (int i = 0; i < ViewportCorners.Length; i++)
            {
                Vector3 worldCorner = camera.ViewportToWorldPoint(ViewportCorners[i]);
                Ray ray = new(worldCorner, cameraForward);
                Vector3 projected = targetPlane.Raycast(ray, out float distance) ? ray.GetPoint(distance) : worldCorner;
                corners[i] = projected;
            }

            Vector3 leftDown = corners[0];
            Vector3 rightDown = corners[1];
            Vector3 rightUp = corners[2];
            Vector3 leftUp = corners[3];

            return CreateFrustumProjectionFromPoints(leftDown, rightDown, rightUp, leftUp, camera.transform);
        }

        private static FrustumProjection GetPerspectiveProjection(Camera camera, Plane targetPlane)
        {
            Transform cameraTransform = camera.transform;
            Vector3 cameraPosition = cameraTransform.position;
            Vector3[] frustumCornersLocal = new Vector3[4];
            Rect viewport = new(0, 0, 1, 1);

            camera.CalculateFrustumCorners(
                viewport,
                camera.nearClipPlane,
                Camera.MonoOrStereoscopicEye.Mono,
                frustumCornersLocal);

            int cornersCount = frustumCornersLocal.Length;
            Vector3[] worldPoints = new Vector3[cornersCount];

            for (int i = 0; i < cornersCount; i++)
            {
                Vector3 direction = cameraTransform.TransformDirection(frustumCornersLocal[i]);
                float denominator = Vector3.Dot(targetPlane.normal, direction);

                if (Mathf.Abs(denominator) < ParallelRayEpsilon)
                {
                    worldPoints[i] = cameraPosition;
                }
                else
                {
                    float distanceAlongRay = -(Vector3.Dot(targetPlane.normal, cameraPosition) + targetPlane.distance) / denominator;
                    worldPoints[i] = cameraPosition + direction * distanceAlongRay;
                }
            }

            Vector3 leftDown = worldPoints[0];
            Vector3 leftUp = worldPoints[1];
            Vector3 rightUp = worldPoints[2];
            Vector3 rightDown = worldPoints[3];

            return CreateFrustumProjectionFromPoints(leftDown, rightDown, rightUp, leftUp, cameraTransform);
        }

        private static FrustumProjection CreateFrustumProjectionFromPoints(
            Vector3 leftDown,
            Vector3 rightDown,
            Vector3 rightUp,
            Vector3 leftUp,
            Transform cameraTransform)
        {
            Vector3 center = (leftUp + leftDown + rightUp + rightDown) / CenterAverageDivisor;

            Vector3 rightDir = cameraTransform.right;
            Vector3 forwardDir = cameraTransform.forward;

            rightDir.y = 0f;
            forwardDir.y = 0f;
            rightDir.Normalize();
            forwardDir.Normalize();

            float bottomWidth = Mathf.Abs(Vector3.Dot(rightDown - leftDown, rightDir));
            float topWidth = Mathf.Abs(Vector3.Dot(rightUp - leftUp, rightDir));
            float leftHeight = Mathf.Abs(Vector3.Dot(leftUp - leftDown, forwardDir));
            float rightHeight = Mathf.Abs(Vector3.Dot(rightUp - rightDown, forwardDir));

            return new FrustumProjection(
                leftUp,
                leftDown,
                rightUp,
                rightDown,
                center,
                bottomWidth,
                topWidth,
                leftHeight,
                rightHeight
            );
        }
    }
}