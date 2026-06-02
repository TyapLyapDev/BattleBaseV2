using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation
{
    public readonly struct GroundProjection
    {
        public GroundProjection(
            Vector3 leftUp,
            Vector3 leftDown,
            Vector3 rightUp,
            Vector3 rightDown,
            Vector3 center, 
            float width,
            float height)
        {
            LeftUp = leftUp;
            LeftDown = leftDown;
            RightUp = rightUp;
            RightDown = rightDown;
            Center = center;
            Width = width;            
            Height = height;
        }

        public Vector3 LeftUp { get; }

        public Vector3 LeftDown { get; }

        public Vector3 RightUp { get; }

        public Vector3 RightDown { get; }

        public Vector3 Center { get; }

        public float Width { get; }

        public float Height { get; }
    }
}