using System;
using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation
{
    public class CameraZoom : ICameraZoom, IDisposable
    {
        private readonly ICameraHandle _cameraHandle;
        private readonly ICameraOrientationAdapter _orientationAdapter;

        public CameraZoom(ICameraHandle cameraHandle, ICameraOrientationAdapter orientationAdapter)
        {
            _cameraHandle = cameraHandle ?? throw new ArgumentNullException(nameof(cameraHandle));
            _orientationAdapter = orientationAdapter ?? throw new ArgumentNullException(nameof(orientationAdapter));

            _orientationAdapter.Changed += OnOrientationAdapterChanged;
        }

        public event Action Changed;

        public float Value01
        {
            get
            {
                float range = MaximumSize - MinimumSize;

                if (range <= 0f)
                    throw new InvalidOperationException("Invalid size range");

                float normalized = (CurrentSize - MinimumSize) / range;

                return 1f - normalized;
            }
        }

        private float CurrentSize => _orientationAdapter.CurrentSize;

        private float MinimumSize => _orientationAdapter.MinimumSize;

        private float MaximumSize => _orientationAdapter.MaximumSize;

        public void Dispose() =>
            _orientationAdapter.Changed -= OnOrientationAdapterChanged;

        public void SetValue01(float value)
        {
            float clampedValue = Mathf.Clamp01(value);
            float targetSize = MaximumSize - clampedValue * (MaximumSize - MinimumSize);
            SetCameraSize(targetSize);
        }

        public void Update(float? zoomDelta)
        {
            if (zoomDelta.HasValue)
            {
                float newSize = CurrentSize - zoomDelta.Value;
                SetCameraSize(newSize);
            }           
        }

        private void SetCameraSize(float size)
        {
            float clamped = Mathf.Clamp(size, MinimumSize, MaximumSize);

            if (Mathf.Approximately(CurrentSize, clamped) == false)
            {
                _cameraHandle.SetProjectionSize(clamped);

                Changed?.Invoke();
            }            
        }

        private void OnOrientationAdapterChanged() =>
            Changed?.Invoke();
    }
}