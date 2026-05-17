using System;
using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation
{
    public class GameSceneCameraOrientationAdapter : ICameraOrientationAdapter, IDisposable
    {
        private const float AspectComparisonEpsilon = 0.01f;

        private readonly Camera _camera;
        private readonly IScreenSizeTracker _screenSizeTracker;
        private readonly IScreenOrientationTracker _orientationTracker;
        private readonly IVerticalFactorCalculator _verticalFactorCalculator;
        private readonly ICameraTracker _cameraTracker;
        private readonly float _originalMinimumOrthoSize;
        private readonly float _originalMaximumOrthoSize;
        private readonly float _portraitReferenceAspect;

        private float _effectiveMinimumOrthoSize;
        private float _effectiveMaximumOrthoSize;
        private float _lastAspect;

        public GameSceneCameraOrientationAdapter(
            Camera camera,
            IScreenSizeTracker screenSizeTracker,
            IScreenOrientationTracker orientationTracker,
            IOrthographicSizeConfig config,
            IVerticalFactorCalculator verticalFactorCalculator,
            ICameraTracker cameraTracker)
        {
            _camera = camera != null ? camera : throw new ArgumentNullException(nameof(camera));
            _screenSizeTracker = screenSizeTracker ?? throw new ArgumentNullException(nameof(screenSizeTracker));
            _orientationTracker = orientationTracker ?? throw new ArgumentNullException(nameof(orientationTracker));
            _verticalFactorCalculator = verticalFactorCalculator ?? throw new ArgumentNullException(nameof(verticalFactorCalculator));
            _cameraTracker = cameraTracker ?? throw new ArgumentNullException(nameof(cameraTracker));

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

            _portraitReferenceAspect =
                Mathf.Min(config.ReferenceValuePortraitOrientation.x, config.ReferenceValuePortraitOrientation.y) /
                Mathf.Max(config.ReferenceValuePortraitOrientation.x, config.ReferenceValuePortraitOrientation.y);

            _lastAspect = GetAspect();
            RecalculateEffectiveZoomBounds();
            AdjustCameraSizeToCurrentValue01();

            _screenSizeTracker.SizeChanged += OnSizeChanged;
            _orientationTracker.OrientationChanged += OnOrientationChanged;
            _cameraTracker.RotationChanged += OnRotationChanged;
        }

        public event Action Changed;

        public float CurrentOrthoSize => _camera.orthographicSize;

        public float MinimumOrthoSize => _effectiveMinimumOrthoSize;

        public float MaximumOrthoSize => _effectiveMaximumOrthoSize;

        public void Dispose()
        {
            if (_screenSizeTracker != null)
                _screenSizeTracker.SizeChanged -= OnSizeChanged;

            if (_orientationTracker != null)
                _orientationTracker.OrientationChanged -= OnOrientationChanged;

            if (_cameraTracker != null)
                _cameraTracker.RotationChanged -= OnRotationChanged;
        }

        private void RecalculateEffectiveZoomBounds()
        {
            if (_orientationTracker.ScreenOrientation == ScreenOrientationType.Portrait)
            {
                float currentAspect = GetAspect();
                float multiplier = _portraitReferenceAspect / currentAspect;
                _effectiveMinimumOrthoSize = _originalMinimumOrthoSize * multiplier;
                _effectiveMaximumOrthoSize = _originalMaximumOrthoSize * multiplier;
            }
            else
            {
                float multiplier = _portraitReferenceAspect;
                float verticalFactor = _verticalFactorCalculator.CalculateVerticalFactor();
                float tiltCompensation = 1f / verticalFactor;

                multiplier *= tiltCompensation;

                _effectiveMinimumOrthoSize = _originalMinimumOrthoSize * multiplier;
                _effectiveMaximumOrthoSize = _originalMaximumOrthoSize * multiplier;
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

        private float GetAspect() =>
            (float)_screenSizeTracker.Width / _screenSizeTracker.Height;

        private void InvokeChanged() =>
            Changed?.Invoke();

        private float ComputeSizeFromValue01(float value01) =>
            _effectiveMaximumOrthoSize - value01 * (_effectiveMaximumOrthoSize - _effectiveMinimumOrthoSize);

        private void OnSizeChanged()
        {
            float currentAspect = GetAspect();

            if (Mathf.Abs(currentAspect - _lastAspect) < AspectComparisonEpsilon)
                return;

            _lastAspect = currentAspect;

            float currentValue01 = ComputeValue01(_camera.orthographicSize, _effectiveMinimumOrthoSize, _effectiveMaximumOrthoSize);
            RecalculateEffectiveZoomBounds();
            SetCameraSizeFromValue01(currentValue01);
        }

        private void OnOrientationChanged()
        {
            float currentValue01 = ComputeValue01(_camera.orthographicSize, _effectiveMinimumOrthoSize, _effectiveMaximumOrthoSize);
            RecalculateEffectiveZoomBounds();
            SetCameraSizeFromValue01(currentValue01);
        }

        private void OnRotationChanged()
        {
            float currentAspect = GetAspect();
            _lastAspect = currentAspect;
            float currentValue01 = ComputeValue01(_camera.orthographicSize, _effectiveMinimumOrthoSize, _effectiveMaximumOrthoSize);
            RecalculateEffectiveZoomBounds();
            SetCameraSizeFromValue01(currentValue01);
        }
    }
}