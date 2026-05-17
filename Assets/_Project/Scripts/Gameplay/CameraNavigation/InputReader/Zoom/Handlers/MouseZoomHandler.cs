using System;
using BattleBase.Utils;
using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation.InputReader
{
    public class MouseZoomHandler : IZoomHandler
    {
        private readonly IUIPointerChecker _uiPointerChecker;
        private readonly float _zoomSensitivity;
        private readonly float _scrollThreshold;

        public MouseZoomHandler(
            IUIPointerChecker uiPointerChecker,
            IZoomConfig config)
        {
            _uiPointerChecker = uiPointerChecker ?? throw new ArgumentNullException(nameof(uiPointerChecker));
            _zoomSensitivity = config.ScrollSensitivity;
            _scrollThreshold = config.ScrollThreshold;
        }

        public float? Update()
        {
            if(_uiPointerChecker.IsPointerOverUI(Input.mousePosition))
                return null;

            float scroll = Input.GetAxis(InputConstants.MouseScrollWheel);

            if (Mathf.Abs(scroll) > _scrollThreshold)
                return scroll * _zoomSensitivity;

            return null;
        }
    }
}