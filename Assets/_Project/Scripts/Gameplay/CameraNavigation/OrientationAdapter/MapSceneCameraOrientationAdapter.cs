using System;
using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation
{
    public class MapSceneCameraOrientationAdapter : ICameraOrientationAdapter, IDisposable
    {
        private readonly Camera _camera;
        private readonly IScreenOrientationTracker _orientationTracker;
        private readonly float _originalMinimumOrthoSize;
        private readonly float _originalMaximumOrthoSize;

        private float _effectiveMinimumOrthoSize;
        private float _effectiveMaximumOrthoSize;

        public MapSceneCameraOrientationAdapter(
            Camera camera, 
            IScreenOrientationTracker orientationTracker, 
            IOrthographicSizeConfig config)
        {
            _camera = camera != null ? camera : throw new ArgumentNullException(nameof(camera));
            _orientationTracker = orientationTracker ?? throw new ArgumentNullException(nameof(orientationTracker));

            if (config == null)
                throw new ArgumentNullException(nameof(config));

            _originalMinimumOrthoSize = config.MinimumOrthoSize;
            _originalMaximumOrthoSize = config.MaximumOrthoSize;

            if (_originalMinimumOrthoSize < 0)
                throw new ArgumentOutOfRangeException(nameof(_originalMinimumOrthoSize), _originalMinimumOrthoSize, "Value must be positive");

            if (_originalMaximumOrthoSize < 0)
                throw new ArgumentOutOfRangeException(nameof(_originalMaximumOrthoSize), _originalMaximumOrthoSize, "Value must be positive");

            if (_originalMaximumOrthoSize <= _originalMinimumOrthoSize)
            {
                throw new ArgumentException(
                    $"MaximumOrtoSize ({_originalMaximumOrthoSize}) must be greater than MinimumOrtoSize ({_originalMinimumOrthoSize})",
                    nameof(config));
            }

            RecalculateEffectiveZoomBounds();
            AdjustCameraSizeToCurrentValue01();

            _orientationTracker.OrientationChanged += OnOrientationChanged;
        }

        public event Action Changed;

        public float CurrentOrthoSize => _camera.orthographicSize;

        public float MinimumOrthoSize => _effectiveMinimumOrthoSize;

        public float MaximumOrthoSize => _effectiveMaximumOrthoSize;

        public void Dispose()
        {
            if (_orientationTracker != null)
                _orientationTracker.OrientationChanged -= OnOrientationChanged;
        }

        private void RecalculateEffectiveZoomBounds()
        {
            if (_orientationTracker.ScreenOrientation == ScreenOrientationType.Portrait)
            {
                int width = _orientationTracker.Width;
                int height = _orientationTracker.Height;
                float aspectRatio = (float)Mathf.Max(width, height) / Mathf.Min(width, height);
                _effectiveMinimumOrthoSize = _originalMinimumOrthoSize * aspectRatio;
                _effectiveMaximumOrthoSize = _originalMaximumOrthoSize * aspectRatio;
            }
            else
            {
                _effectiveMinimumOrthoSize = _originalMinimumOrthoSize;
                _effectiveMaximumOrthoSize = _originalMaximumOrthoSize;
            }
        }

        private void AdjustCameraSizeToCurrentValue01()
        {
            float currentValue01 = ComputeValue01(_camera.orthographicSize, _originalMinimumOrthoSize, _originalMaximumOrthoSize);
            SetCameraSizeFromValue01(currentValue01);
        }

        private void SetCameraSizeFromValue01(float value01)
        {
            float newSize = ComputeSizeFromValue01(value01);
            _camera.orthographicSize = Mathf.Clamp(newSize, _effectiveMinimumOrthoSize, _effectiveMaximumOrthoSize);
            InvokeChanged();
        }

        private float ComputeValue01(float currentSize, float minimumBound, float maximumBound)
        {
            float range = maximumBound - minimumBound;

            if (range <= float.Epsilon)
                throw new ArgumentOutOfRangeException(nameof(range), range, "Value must be positive");

            float normalized = (currentSize - minimumBound) / range;
            
            return 1f - normalized;
        }

        private float ComputeSizeFromValue01(float value01) =>
            _effectiveMaximumOrthoSize - value01 * (_effectiveMaximumOrthoSize - _effectiveMinimumOrthoSize);

        private void InvokeChanged() =>
            Changed?.Invoke();

        private void OnOrientationChanged()
        {
            float currentValue01 = ComputeValue01(_camera.orthographicSize, _effectiveMinimumOrthoSize, _effectiveMaximumOrthoSize);
            RecalculateEffectiveZoomBounds();
            SetCameraSizeFromValue01(currentValue01);
        }
    }
}