using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BattleBase.Gameplay.CameraNavigation.InputReader
{
    public class UIPointerChecker : IUIPointerChecker
    {
        private readonly List<GraphicRaycaster> _raycasters = new();
        private readonly List<RaycastResult> _raycastResults = new();

        private int _lastFrame = -1;
        private Vector2 _lastPosition;
        private bool _lastResult;

        private PointerEventData _cachedEventData;

        public void AddCanvas(Canvas canvas)
        {
            if (canvas == null)
                return;

            if (canvas.TryGetComponent(out GraphicRaycaster raycaster))
            {
                if (_raycasters.Contains(raycaster) == false)
                    _raycasters.Add(raycaster);
            }
        }

        public bool IsPointerOverUI(Vector2 screenPosition)
        {
            int currentFrame = Time.frameCount;

            if (currentFrame == _lastFrame && screenPosition == _lastPosition)
                return _lastResult;

            _lastFrame = currentFrame;
            _lastPosition = screenPosition;
            _lastResult = RaycastUI(screenPosition);

            return _lastResult;
        }

        private bool RaycastUI(Vector2 screenPosition)
        {
            if (EventSystem.current == null) return false;

            PointerEventData eventData = new(EventSystem.current) { position = screenPosition };

            foreach (GraphicRaycaster raycaster in _raycasters)
            {
                if (raycaster == null || !raycaster.enabled || !raycaster.gameObject.activeInHierarchy)
                    continue;

                _raycastResults.Clear();
                raycaster.Raycast(eventData, _raycastResults);

                if (_raycastResults.Count > 0)
                    return true;
            }

            return false;
        }
    }
}