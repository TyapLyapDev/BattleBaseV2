using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation
{
    public readonly struct FrustumProjection
    {
        public FrustumProjection(
            Vector3 leftUp,
            Vector3 leftDown,
            Vector3 rightUp,
            Vector3 rightDown,
            Vector3 center,
            float bottomWidth,
            float topWidth,
            float leftHeight,
            float rightHeight)
        {
            LeftUp = leftUp;
            LeftDown = leftDown;
            RightUp = rightUp;
            RightDown = rightDown;
            Center = center;
            BottomWidth = bottomWidth;
            TopWidth = topWidth;
            LeftHeight = leftHeight;
            RightHeight = rightHeight;
        }

        public Vector3 LeftUp { get; }

        public Vector3 LeftDown { get; }

        public Vector3 RightUp { get; }

        public Vector3 RightDown { get; }

        public Vector3 Center { get; }

        public float BottomWidth { get; }

        public float TopWidth { get; }

        public float LeftHeight { get; }

        public float RightHeight { get; }
    }
}