using System;
using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation.InputReader
{
    public class TouchPinchHandler : IZoomHandler
    {
        private readonly Camera _camera;
        private readonly IUIPointerChecker _uiPointerChecker;
        private readonly float _minPinchDistance;
        private readonly float _zoomSensitivity;

        private bool _isPinching;
        private float _previousPinchDistance;
        private bool _isBlockedByUI;

        public TouchPinchHandler(
            Camera camera,
            IUIPointerChecker uiPointerChecker,
            ITouchConfig config)
        {
            _camera = camera != null ? camera : throw new ArgumentNullException(nameof(camera));
            _uiPointerChecker = uiPointerChecker ?? throw new ArgumentNullException(nameof(uiPointerChecker));
            _minPinchDistance = config.MinPinchDistance;
            _zoomSensitivity = config.ZoomSensitivity;
        }

        public float? Update()
        {
            if (Input.touchCount < 2)
            {
                ResetPinch();

                return null;
            }

            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            if (_isPinching == false && _isBlockedByUI == false)
            {
                if (IsAnyTouchBeganOnUI(touch0, touch1))
                {
                    _isBlockedByUI = true;
                    ResetPinch();
                    return null;
                }

                float currentDistance = Vector2.Distance(touch0.position, touch1.position);

                if (currentDistance >= _minPinchDistance)
                {
                    _isPinching = true;
                    _previousPinchDistance = currentDistance;
                }

                return null;
            }

            if (_isBlockedByUI)
                return HandleBlockedByUI(touch0, touch1);

            if (_isPinching)
                return HandlePinch(touch0, touch1);

            return null;
        }

        private float? HandleBlockedByUI(Touch touch0, Touch touch1)
        {
            if (IsEndedOrCanceled(touch0) && IsEndedOrCanceled(touch1))
                ResetPinch();

            return null;
        }

        private float? HandlePinch(Touch touch0, Touch touch1)
        {
            if (IsEndedOrCanceled(touch0) || IsEndedOrCanceled(touch1))
            {
                ResetPinch();

                return null;
            }

            if (_isPinching == false)
            {
                float currentDistance = Vector2.Distance(touch0.position, touch1.position);

                if (currentDistance >= _minPinchDistance)
                {
                    _isPinching = true;
                    _previousPinchDistance = currentDistance;
                }

                return null;
            }

            if (touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved)
            {
                float currentDistance = Vector2.Distance(touch0.position, touch1.position);
                float delta = currentDistance - _previousPinchDistance;
                _previousPinchDistance = currentDistance;
                float sensitivity = _zoomSensitivity * _camera.orthographicSize;

                return delta * sensitivity;
            }

            return null;
        }

        private bool IsAnyTouchBeganOnUI(Touch touch0, Touch touch1)
        {
            return touch0.phase == TouchPhase.Began && _uiPointerChecker.IsPointerOverUI(touch0.position) ||
                   touch1.phase == TouchPhase.Began && _uiPointerChecker.IsPointerOverUI(touch1.position);
        }

        private bool IsEndedOrCanceled(Touch touch) =>
            touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled;

        private void ResetPinch()
        {
            _isPinching = false;
            _isBlockedByUI = false;
            _previousPinchDistance = 0f;
        }
    }
}