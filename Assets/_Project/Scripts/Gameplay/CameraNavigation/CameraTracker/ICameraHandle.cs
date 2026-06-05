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

        public Transform CameraRigTransform { get; }

        public Vector3 CameraRigPosition { get; }

        public float ProjectionSize { get; }

        public CameraProjectionType ProjectionType { get; }

        public void SetProjectionSize(float size);

        public void SetCameraRigPosition(Vector3 position);

        public void SetCameraRigEulerAngles(Vector3 rotation);
    }
}