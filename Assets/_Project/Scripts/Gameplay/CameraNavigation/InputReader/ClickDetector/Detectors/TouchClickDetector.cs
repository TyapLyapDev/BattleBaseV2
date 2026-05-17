using System;
using BattleBase.UpdateService;
using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation.InputReader
{
    public class TouchClickDetector : IClickDetector, IDisposable
    {
        private readonly Camera _camera;
        private readonly IUpdater _updater;
        private readonly IUIPointerChecker _uiPointerChecker;
        private readonly float _dragThreshold;

        private Vector2 _startPosition;
        private bool _isPointerDown;

        public TouchClickDetector(
            Camera camera,
            IUpdater updater,
            IUIPointerChecker uiPointerChecker,
            IClickConfig config)
        {
            _camera = camera != null ? camera : throw new ArgumentNullException(nameof(camera));
            _updater = updater ?? throw new ArgumentNullException(nameof(updater));
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
            int touchCount = Input.touchCount;

            if (touchCount != 1)
            {
                Reset();

                return;
            }

            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = touch.position;

            if (touch.phase == TouchPhase.Began)
                OnTouchBegin(touchPosition);
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                OnTouchEnd(touchPosition);
        }

        private void OnTouchBegin(Vector2 position)
        {
            if (_uiPointerChecker.IsPointerOverUI(position))
            {
                Reset();

                return;
            }

            _isPointerDown = true;
            _startPosition = position;
        }

        private void OnTouchEnd(Vector2 position)
        {
            if (_isPointerDown == false)
                return;

            Vector2 startPosition = _startPosition;
            Reset();

            if (_uiPointerChecker.IsPointerOverUI(position))
                return;

            if (Vector2.Distance(position, startPosition) > _dragThreshold)
                return;

            Ray ray = _camera.ScreenPointToRay(position);

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