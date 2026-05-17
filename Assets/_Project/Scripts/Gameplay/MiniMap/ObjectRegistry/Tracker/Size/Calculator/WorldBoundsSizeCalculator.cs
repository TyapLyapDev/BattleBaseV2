using System;
using UnityEngine;

namespace BattleBase.Gameplay.MiniMap
{
    public class WorldBoundsSizeCalculator : IEntitySizeCalculator
    {
        private Renderer[] _renderers;

        public Vector2 Calculate(Transform transform)
        {
            if (transform == null)
                throw new ArgumentNullException(nameof(transform));

            if (_renderers == null)
                _renderers = transform.GetComponentsInChildren<Renderer>();

            if (_renderers.Length == 0)
            {
                Debug.LogWarning($"Object {transform.name} has no renderers, returning default size 1x1", transform);

                return Vector2.one;
            }

            Vector3 min = new(float.PositiveInfinity, 0, float.PositiveInfinity);
            Vector3 max = new(float.NegativeInfinity, 0, float.NegativeInfinity);
            bool anyPoint = false;

            foreach (Renderer rend in _renderers)
            {
                if (rend == null)
                    continue;

                Bounds localBounds = rend.localBounds;
                Vector3[] corners = GetLocalCorners(localBounds);

                foreach (Vector3 localCorner in corners)
                {
                    Vector3 worldCorner = rend.transform.TransformPoint(localCorner);

                    if (worldCorner.x < min.x)
                        min.x = worldCorner.x;

                    if (worldCorner.x > max.x)
                        max.x = worldCorner.x;

                    if (worldCorner.z < min.z)
                        min.z = worldCorner.z;

                    if (worldCorner.z > max.z)
                        max.z = worldCorner.z;

                    anyPoint = true;
                }
            }

            if (anyPoint == false)
                return Vector2.one;

            float width = max.x - min.x;
            float height = max.z - min.z;

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