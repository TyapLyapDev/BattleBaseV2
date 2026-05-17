using System;
using BattleBase.Utils;
using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation.InputReader
{
    public class TouchDragHandler : IDragHandler
    {
        private readonly Camera _camera;
        private readonly IUIPointerChecker _uiPointerChecker;
        private readonly float _dragDeltaThreshold;

        private Vector2 _lastDragPosition;
        private bool _isDragging;
        private bool _isBlockedByUI;

        public TouchDragHandler(
            Camera camera,
            IUIPointerChecker uiPointerChecker,
            ITouchConfig config)
        {
            _camera = camera != null ? camera : throw new ArgumentNullException(nameof(camera));
            _uiPointerChecker = uiPointerChecker ?? throw new ArgumentNullException(nameof(uiPointerChecker));
            _dragDeltaThreshold = config.DragDeltaThreshold;
        }

        public Vector3? Update(float _)
        {
            if (Input.touchCount != 1)
            {
                ResetDrag();

                return null;
            }

            Touch touch = Input.GetTouch(0);
            Vector2 position = touch.position;
            TouchPhase phase = touch.phase;

            if (phase == TouchPhase.Began)
                return OnTouchBegin(position);
            if (_isBlockedByUI)
                return OnBlockedByUI(phase);
            if (_isDragging)
                return OnDrag(position, phase);

            return null;
        }

        private Vector3? OnTouchBegin(Vector2 position)
        {
            if (_uiPointerChecker.IsPointerOverUI(position))
            {
                _isBlockedByUI = true;
                ResetDrag();
                return null;
            }

            _isBlockedByUI = false;
            _isDragging = true;
            _lastDragPosition = position;

            return null;
        }

        private Vector3? OnBlockedByUI(TouchPhase phase)
        {
            if (phase == TouchPhase.Ended || phase == TouchPhase.Canceled)
                ResetDrag();

            return null;
        }

        private Vector3? OnDrag(Vector2 position, TouchPhase phase)
        {
            if (phase == TouchPhase.Moved || phase == TouchPhase.Stationary)
            {
                Vector2 pixelDelta = position - _lastDragPosition;
                _lastDragPosition = position;

                if (pixelDelta.magnitude > _dragDeltaThreshold)
                    return CameraDragHelper.ConvertPixelDeltaToWorldDelta(_camera, pixelDelta);

                return Vector3.zero;
            }

            if (phase == TouchPhase.Ended || phase == TouchPhase.Canceled)
            {
                ResetDrag();

                return null;
            }

            return null;
        }

        private void ResetDrag()
        {
            _isDragging = false;
            _isBlockedByUI = false;
            _lastDragPosition = Vector2.zero;
        }
    }
}