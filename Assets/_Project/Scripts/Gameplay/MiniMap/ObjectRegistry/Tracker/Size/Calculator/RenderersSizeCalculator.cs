using System;
using UnityEngine;

namespace BattleBase.Gameplay.MiniMap
{
    public class RenderersSizeCalculator : IEntitySizeCalculator
    {
        private Renderer[] _renderers;

        public Vector2 Calculate(Transform transform)
        {
            if (transform == null)
                throw new ArgumentNullException(nameof(transform));

            Vector2 localSize = ComputeLocalSize(transform);
            Vector3 scale = transform.lossyScale;

            float width = localSize.x * Mathf.Abs(scale.x);
            float height = localSize.y * Mathf.Abs(scale.z);

            return new(width, height);
        }

        private Vector2 ComputeLocalSize(Transform transform)
        {
            _renderers ??= transform.GetComponentsInChildren<Renderer>();

            Vector3 localMin;
            Vector3 localMax;

            if (_renderers.Length == 0)
            {
                Debug.LogWarning($"Build {transform.name} has no renderers, returning default size 1x1", transform);

                return Vector2.one;
            }

            localMin = Vector3.positiveInfinity;
            localMax = Vector3.negativeInfinity;
            bool anyPoint = false;

            foreach (Renderer rend in _renderers)
            {
                if (rend == null)
                    continue;

                Bounds localBounds = rend.localBounds;
                Vector3[] corners = GetLocalCorners(localBounds);

                foreach (Vector3 corner in corners)
                {
                    Vector3 worldCorner = rend.transform.TransformPoint(corner);
                    Vector3 cornerInBuildingSpace = transform.InverseTransformPoint(worldCorner);

                    if (cornerInBuildingSpace.x < localMin.x)
                        localMin.x = cornerInBuildingSpace.x;

                    if (cornerInBuildingSpace.x > localMax.x)
                        localMax.x = cornerInBuildingSpace.x;

                    if (cornerInBuildingSpace.z < localMin.z)
                        localMin.z = cornerInBuildingSpace.z;

                    if (cornerInBuildingSpace.z > localMax.z)
                        localMax.z = cornerInBuildingSpace.z;

                    anyPoint = true;
                }
            }

            if (anyPoint == false)
                return Vector2.one;

            float width = localMax.x - localMin.x;
            float height = localMax.z - localMin.z;

            return new Vector2(width, height);
        }

        private Vector3[] GetLocalCorners(Bounds bounds)
        {
            Vector3 center = bounds.center;
            Vector3 extents = bounds.extents;

            return new Vector3[]
            {
                center + new Vector3(-extents.x, -extents.y, -extents.z),
                center + new Vector3(-extents.x, -extents.y,  extents.z),
                center + new Vector3(-extents.x,  extents.y, -extents.z),
                center + new Vector3(-extents.x,  extents.y,  extents.z),
                center + new Vector3( extents.x, -extents.y, -extents.z),
                center + new Vector3( extents.x, -extents.y,  extents.z),
                center + new Vector3( extents.x,  extents.y, -extents.z),
                center + new Vector3( extents.x,  extents.y,  extents.z)
            };
        }
    }
}