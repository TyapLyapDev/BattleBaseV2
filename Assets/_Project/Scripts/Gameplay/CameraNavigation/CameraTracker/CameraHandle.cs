using System;
using BattleBase.UpdateService;
using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation
{
    public sealed class CameraHandle : ICameraHandle, IDisposable
    {
        private static readonly UpdateType s_UpdateType = UpdateType.LateUpdate;

        private readonly Camera _camera;
        private readonly Transform _cameraRig;
        private readonly Transform _cameraTransform;
        private readonly IUpdater _updater;
        private readonly ICameraTrackingConfig _config;

        private CameraProjectionType _lastProjectionType;
        private Vector3 _lastPosition;
        private Quaternion _lastRotation;
        private float _lastProjectionSize;

        public CameraHandle(Camera camera, CameraRig cameraRig, IUpdater updater, ICameraTrackingConfig config)
        {
            _camera = camera != null ? camera : throw new ArgumentNullException(nameof(camera));

            if(cameraRig == null)
                throw new ArgumentNullException(nameof(cameraRig));

            _cameraRig = cameraRig.transform;

            _updater = updater ?? throw new ArgumentNullException(nameof(updater));
            _config = config ?? throw new ArgumentNullException(nameof(config));

            _cameraTransform = camera.transform;
            _updater.Subscribe(OnUpdate, s_UpdateType);
            OnUpdate();
        }

        public event Action PositionChanged;
        public event Action RotationChanged;
        public event Action ProjectionChanged;
        public event Action SizeChanged;

        public Camera Camera => _camera;

        public Transform CameraRigTransform => _cameraRig;

        public Vector3 CameraRigPosition => _cameraRig.position;

        public CameraProjectionType ProjectionType => _camera.orthographic
            ? CameraProjectionType.Orthographic
            : CameraProjectionType.Perspective;

        public float ProjectionSize => ProjectionType == CameraProjectionType.Orthographic
            ? _camera.orthographicSize
            : _camera.transform.localPosition.y;

        public void Dispose() =>
            _updater?.Unsubscribe(OnUpdate, s_UpdateType);

        public void SetProjectionSize(float size)
        {
            if (Mathf.Approximately(ProjectionSize, size) == false)
            {
                if (ProjectionType == CameraProjectionType.Orthographic)
                {
                    _camera.orthographicSize = size;
                }
                else
                {
                    Vector3 position = _camera.transform.localPosition;
                    position.y = size;
                    _camera.transform.localPosition = position;
                }

                TrackPosition();
                TrackSize();
            }
        }

        public void SetCameraRigPosition(Vector3 position)
        {
            _cameraRig.transform.position = position;
            TrackPosition();
        }

        public void SetCameraRigEulerAngles(Vector3 rotation)
        {
            _cameraRig.transform.eulerAngles = rotation;
            TrackRotation();
        }

        private void OnUpdate()
        {
            TrackPosition();
            TrackRotation();
            TrackSize();
            TrackProjectionType();
        }

        private void TrackPosition()
        {
            Vector3 currentPosition = _cameraTransform.position;

            if ((currentPosition - _lastPosition).sqrMagnitude > _config.PositionSqrThreshold)
            {
                _lastPosition = currentPosition;
                PositionChanged?.Invoke();
            }
        }

        private void TrackRotation()
        {
            Quaternion currentRotation = _cameraTransform.rotation;
            float angle = Quaternion.Angle(currentRotation, _lastRotation);

            if (angle > _config.RotationAngleThreshold)
            {
                _lastRotation = currentRotation;
                RotationChanged?.Invoke();
            }
        }

        private void TrackSize()
        {
            if (ProjectionType == CameraProjectionType.Orthographic)
            {
                if (Math.Abs(_camera.orthographicSize - _lastProjectionSize) > _config.OrthoSizeThreshold)
                {
                    _lastProjectionSize = _camera.orthographicSize;

                    SizeChanged?.Invoke();
                }
            }
            else if (ProjectionType == CameraProjectionType.Perspective)
            {
                if (Math.Abs(_camera.transform.localPosition.y - _lastProjectionSize) > Mathf.Epsilon)
                {
                    _lastProjectionSize = _camera.transform.localPosition.y;

                    SizeChanged?.Invoke();
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(ProjectionType));
            }
        }

        private void TrackProjectionType()
        {
            CameraProjectionType currentProjection = ProjectionType;

            if (_lastProjectionType != currentProjection)
            {
                _lastProjectionType = currentProjection;

                ProjectionChanged?.Invoke();
            }
        }
    }
}