using System;
using BattleBase.Utils;
using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation.InputReader
{
    public class KeyboardDragHandler : IKeyboardDragHandler
    {
        private readonly ICameraOrientationAdapter _orientationAdapter;
        private readonly float _keyboardSpeed;
        private readonly float _axisThreshold;

        public KeyboardDragHandler(IDragConfig config, ICameraOrientationAdapter orientationAdapter)
        {
            _orientationAdapter = orientationAdapter ?? throw new ArgumentNullException(nameof(orientationAdapter));
            
            if(config == null)
                throw new ArgumentNullException(nameof(config));

            _keyboardSpeed = config.KeyboardSpeed;
            _axisThreshold = config.KeyboardAxisThreshold;
        }

        public Vector3? Update(float deltaTime)
        {
            if (deltaTime < 0)
                throw new ArgumentOutOfRangeException(nameof(deltaTime), deltaTime, "Value must be positive");

            float x = Input.GetAxisRaw(InputConstants.KeyboardAxisX);
            float z = Input.GetAxisRaw(InputConstants.KeyboardAxisY);

            if (Mathf.Abs(x) > _axisThreshold || Mathf.Abs(z) > _axisThreshold)
            {
                float zoomFactor = _orientationAdapter.CurrentOrthoSize;
                float finalSpeed = _keyboardSpeed * zoomFactor;
                Vector3 move = finalSpeed * deltaTime * new Vector3(x, 0, z);

                return -move;
            }
            else
            {
                return null;
            }
        }
    }
}