using System;
using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation
{
    public class GameSceneCameraOrientationAdapter : ICameraOrientationAdapter, IDisposable
    {
        private readonly IScreenSizeTracker _screenSizeTracker;
        private readonly IScreenOrientationTracker _orientationTracker;
        private readonly IVerticalFactorCalculator _verticalFactorCalculator;
        private readonly ICameraHandle _cameraHandle;
        private readonly IProjectionSizeConfig _config;
        private readonly float _portraitReferenceAspect;

        private float _originalMinimumSize;
        private float _originalMaximumSize;

        private float _effectiveMinimumSize;
        private float _effectiveMaximumSize;
        private float _lastAspect;

        public GameSceneCameraOrientationAdapter(
            IScreenSizeTracker screenSizeTracker,
            IScreenOrientationTracker orientationTracker,
            IProjectionSizeConfig config,
            IVerticalFactorCalculator verticalFactorCalculator,
            ICameraHandle cameraHandle)
        {
            _screenSizeTracker = screenSizeTracker ?? throw new ArgumentNullException(nameof(screenSizeTracker));
            _orientationTracker = orientationTracker ?? throw new ArgumentNullException(nameof(orientationTracker));
            _verticalFactorCalculator = verticalFactorCalculator ?? throw new ArgumentNullException(nameof(verticalFactorCalculator));
            _cameraHandle = cameraHandle ?? throw new ArgumentNullException(nameof(cameraHandle));
            _config = config ?? throw new ArgumentNullException(nameof(config));

            _portraitReferenceAspect =
                Mathf.Min(config.ReferenceValuePortraitOrientation.x, config.ReferenceValuePortraitOrientation.y) /
                Mathf.Max(config.ReferenceValuePortraitOrientation.x, config.ReferenceValuePortraitOrientation.y);

            UpdateOriginalSize();
            RecalculateEffectiveZoomBounds();

            _screenSizeTracker.SizeChanged += OnScreenSizeChanged;
            Refresh();
        }

        public event Action Changed;

        public float CurrentSize => Mathf.Clamp(_cameraHandle.ProjectionSize, MinimumSize, MaximumSize);

        public float MinimumSize => _effectiveMinimumSize;

        public float MaximumSize => _effectiveMaximumSize;

        public void Dispose()
        {
            if (_screenSizeTracker != null)
                _screenSizeTracker.SizeChanged -= OnScreenSizeChanged;
        }

        public void Refresh()
        {
            float currentAspect = GetAspect();

            if (Mathf.Approximately(currentAspect, _lastAspect) == false)
            {
                _lastAspect = currentAspect;
                UpdateOriginalSize();
                float currentValue01 = ComputeValue01(CurrentSize, _effectiveMinimumSize, _effectiveMaximumSize);
                RecalculateEffectiveZoomBounds();
                SetCameraSizeFromValue01(currentValue01);

                InvokeChanged();
            }
        }

        private void RecalculateEffectiveZoomBounds()
        {
            if (_orientationTracker.ScreenOrientation == ScreenOrientationType.Portrait)
            {
                float currentAspect = GetAspect();
                float multiplier = _portraitReferenceAspect / currentAspect;

                _effectiveMinimumSize = _originalMinimumSize * multiplier;
                _effectiveMaximumSize = _originalMaximumSize * multiplier;
            }
            else
            {
                float multiplier = _portraitReferenceAspect;
                float verticalFactor = _verticalFactorCalculator.CalculateVerticalFactor();
                float tiltCompensation = 1f / verticalFactor;
                multiplier *= tiltCompensation;

                _effectiveMinimumSize = _originalMinimumSize * multiplier;
                _effectiveMaximumSize = _originalMaximumSize * multiplier;
            }
        }

        private float ComputeValue01(float currentSize, float minimumBound, float maximumBound)
        {
            float range = maximumBound - minimumBound;

            if (range <= 0)
                throw new ArgumentOutOfRangeException(nameof(range), range, "Value must be positive");

            float normalized = (currentSize - minimumBound) / range;

            return 1f - normalized;
        }

        private float GetAspect() =>
            (float)_screenSizeTracker.Width / _screenSizeTracker.Height;

        private void InvokeChanged() =>
            Changed?.Invoke();

        private void SetCameraSizeFromValue01(float value01)
        {
            float newSize = ComputeSizeFromValue01(value01);
            _cameraHandle.SetProjectionSize(Mathf.Clamp(newSize, _effectiveMinimumSize, _effectiveMaximumSize));
        }

        private float ComputeSizeFromValue01(float value01) =>
            _effectiveMaximumSize - value01 * (_effectiveMaximumSize - _effectiveMinimumSize);

        private void UpdateOriginalSize()
        {
            if (_cameraHandle.ProjectionType == CameraProjectionType.Orthographic)
            {
                _originalMinimumSize = _config.MinimumOrthoSize;
                _originalMaximumSize = _config.MaximumOrthoSize;
            }
            else
            {
                _originalMinimumSize = _config.MinimumFOV;
                _originalMaximumSize = _config.MaximumFOV;
            }
        }

        private void OnScreenSizeChanged() =>
            Refresh();
    }
}