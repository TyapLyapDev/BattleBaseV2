using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation
{
    public static class BoundsExtensions
    {
        public static GroundProjection GetGroundProjection(this Bounds bounds, Vector3 forward, Vector3 right)
        {
            Vector3 minimum = bounds.min;
            Vector3 maximum = bounds.max;
            float groundY = bounds.center.y;

            Vector3[] worldCorners = new Vector3[4];
            worldCorners[0] = new(minimum.x, groundY, minimum.z);
            worldCorners[1] = new(minimum.x, groundY, maximum.z);
            worldCorners[2] = new(maximum.x, groundY, minimum.z);
            worldCorners[3] = new(maximum.x, groundY, maximum.z);

            float minimumU = float.MaxValue;
            float maximumU = float.MinValue;
            float minimumV = float.MaxValue;
            float maximumV = float.MinValue;

            foreach (Vector3 world in worldCorners)
            {
                float u = Vector3.Dot(world, right);
                float v = Vector3.Dot(world, forward);

                if (u < minimumU)
                    minimumU = u;

                if (u > maximumU)
                    maximumU = u;

                if (v < minimumV)
                    minimumV = v;

                if (v > maximumV)
                    maximumV = v;
            }

            Vector3 leftUpWorld = minimumU * right + maximumV * forward;
            Vector3 leftDownWorld = minimumU * right + minimumV * forward;
            Vector3 rightUpWorld = maximumU * right + maximumV * forward;
            Vector3 rightDownWorld = maximumU * right + minimumV * forward;

            leftUpWorld.y = groundY;
            leftDownWorld.y = groundY;
            rightUpWorld.y = groundY;
            rightDownWorld.y = groundY;

            float width = maximumU - minimumU;
            float height = maximumV - minimumV;
            Vector3 center = (leftUpWorld + leftDownWorld + rightUpWorld + rightDownWorld) / worldCorners.Length;

            return new(
                leftUpWorld,
                leftDownWorld,
                rightUpWorld,
                rightDownWorld,
                center,
                width,
                height);
        }
    }
}