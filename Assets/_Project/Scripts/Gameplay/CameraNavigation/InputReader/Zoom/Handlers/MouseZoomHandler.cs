using System;
using BattleBase.Utils.Constants;
using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation.InputReader
{
    public class MouseZoomHandler : IZoomHandler
    {
        private readonly IUIPointerChecker _uiPointerChecker;
        private readonly IZoomConfig _config;

        public MouseZoomHandler(
            IUIPointerChecker uiPointerChecker,
            IZoomConfig config)
        {
            _uiPointerChecker = uiPointerChecker ?? throw new ArgumentNullException(nameof(uiPointerChecker));
            _config = config ?? throw new ArgumentNullException(nameof(uiPointerChecker));
        }

        public float? Update()
        {
            if (_uiPointerChecker.IsPointerOverUI(Input.mousePosition))
                return null;

            float scroll = Input.GetAxis(Inputs.MouseScrollWheel);

            if (Mathf.Abs(scroll) > _config.ScrollThreshold)
                return scroll * _config.ScrollSensitivity;

            return null;
        }
    }
}