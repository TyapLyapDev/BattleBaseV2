using System;
using BattleBase.Utils;
using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation.InputReader
{
    public class MouseDragHandler : IMouseDragHandler
    {
        private readonly Camera _camera;
        private readonly IUIPointerChecker _uiPointerChecker;
        private readonly float _dragDeltaThreshold;

        private Vector2 _lastMousePosition;
        private bool _isDragging;
        private bool _isBlockedByUI;

        public MouseDragHandler(
            Camera camera, 
            IUIPointerChecker uiPointerChecker,
            IDragConfig config)
        {
            _camera = camera != null ? camera : throw new ArgumentNullException(nameof(camera));
            _uiPointerChecker = uiPointerChecker ?? throw new ArgumentNullException(nameof(uiPointerChecker));
            _dragDeltaThreshold = config.DragDeltaThreshold;
        }

        public Vector3? Update()
        {
            Vector2 mousePosition = Input.mousePosition;
            bool buttonDown = Input.GetMouseButtonDown(InputConstants.DragMouseButton);
            bool buttonPressed = Input.GetMouseButton(InputConstants.DragMouseButton);
            bool buttonUp = Input.GetMouseButtonUp(InputConstants.DragMouseButton);

            if (buttonDown)
                return OnButtonDown(mousePosition);

            if (_isBlockedByUI)
                return OnBlockedByUI(buttonUp);

            if (_isDragging == false)
                return null;

            if (buttonPressed)
                return OnDrag(mousePosition);

            if (buttonUp)
                return OnButtonUp();

            return null;
        }

        private Vector3? OnButtonDown(Vector2 mousePosition)
        {
            if (_uiPointerChecker.IsPointerOverUI(mousePosition))
            {
                _isBlockedByUI = true;
                ResetDrag();

                return null;
            }

            _isBlockedByUI = false;
            _isDragging = true;
            _lastMousePosition = mousePosition;

            return null;
        }

        private Vector3? OnBlockedByUI(bool buttonUp)
        {
            if (buttonUp)
                ResetDrag();

            return null;
        }

        private Vector3? OnDrag(Vector2 mousePosition)
        {
            Vector2 pixelDelta = mousePosition - _lastMousePosition;
            _lastMousePosition = mousePosition;

            if (pixelDelta.magnitude > _dragDeltaThreshold)
                return CameraDragHelper.ConvertPixelDeltaToWorldDelta(_camera, pixelDelta);

            return Vector3.zero;
        }

        private Vector3? OnButtonUp()
        {
            ResetDrag();

            return null;
        }


        private void ResetDrag()
        {
            _isDragging = false;
            _isBlockedByUI = false;
            _lastMousePosition = Vector2.zero;
        }
    }
}