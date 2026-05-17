using System;
using BattleBase.UpdateService;
using BattleBase.Utils;
using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation.InputReader
{
    public class MouseClickDetector : IClickDetector, IDisposable
    {
        private readonly Camera _camera;
        private readonly IUpdater _updater;
        private readonly IUIPointerChecker _uiPointerChecker;
        private readonly float _dragThreshold;

        private Vector2 _startPosition;
        private bool _isPointerDown;

        public MouseClickDetector(
            Camera camera,
            IUpdater uptater,
            IUIPointerChecker uiPointerChecker,
            IClickConfig config)
        {
            _camera = camera != null ? camera : throw new ArgumentNullException(nameof(camera));
            _updater = uptater ?? throw new ArgumentNullException(nameof(uptater));
            _uiPointerChecker = uiPointerChecker ?? throw new ArgumentNullException(nameof(uiPointerChecker));
            _dragThreshold = config.ClickDragThreshold;

            _updater.Subscribe(OnUpdate, UpdateType.Update);
        }

        public event Action<Collider> Clicked;

        public void Dispose() =>
            _updater.Unsubscribe(OnUpdate, UpdateType.Update);

        private void OnUpdate() =>
            ReadClick();

        public void ReadClick()
        {
            Vector2 mousePosition = Input.mousePosition;

            if (Input.GetMouseButtonDown(InputConstants.DragMouseButton))
                OnButtonDown(mousePosition);
            else if (Input.GetMouseButtonUp(InputConstants.DragMouseButton))
                OnButtonUp(mousePosition);
        }

        private void OnButtonDown(Vector2 mousePosition)
        {
            if (_uiPointerChecker.IsPointerOverUI(mousePosition))
            {
                Reset();

                return;
            }

            _isPointerDown = true;
            _startPosition = mousePosition;
        }

        private void OnButtonUp(Vector2 mousePosition)
        {
            if (_isPointerDown == false)
                return;

            Vector2 startPosition = _startPosition;
            Reset();


            if (_uiPointerChecker.IsPointerOverUI(mousePosition))
                return;

            if (Vector2.Distance(mousePosition, startPosition) > _dragThreshold)
                return;

            Ray ray = _camera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
                Clicked?.Invoke(hit.collider);
            else
                Clicked?.Invoke(null);
        }

        private void Reset()
        {
            _isPointerDown = false;
            _startPosition = Vector2.zero;
        }
    }
}