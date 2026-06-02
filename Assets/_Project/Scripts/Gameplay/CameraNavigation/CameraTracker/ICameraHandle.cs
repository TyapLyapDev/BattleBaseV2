using System;
using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation
{
    public interface ICameraHandle
    {
        public event Action PositionChanged;
        public event Action RotationChanged;
        public event Action SizeChanged;
        public event Action ProjectionChanged;

        public Camera Camera { get; }

        public CameraRig CameraRig { get; }

        public Vector3 Position { get; }

        public Quaternion Rotation { get; }

        public float ProjectionSize { get; }

        public CameraProjectionType ProjectionType { get; }

        public void SetProjectionSize(float size);

        public void SetCameraRigPosition(Vector3 position);

        public void SetCameraRigEulerAngles(Vector3 rotation);
    }
}