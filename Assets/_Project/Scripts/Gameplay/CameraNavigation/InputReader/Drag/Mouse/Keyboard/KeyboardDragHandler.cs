using System;
using BattleBase.Utils.Constants;
using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation.InputReader
{
    public class KeyboardDragHandler : IKeyboardDragHandler
    {
        private readonly ICameraHandle _cameraHandle;
        private readonly float _keyboardSpeed;
        private readonly float _axisThreshold;

        public KeyboardDragHandler(IDragConfig config, ICameraHandle cameraHandle)
        {
            _cameraHandle = cameraHandle ?? throw new ArgumentNullException(nameof(cameraHandle));

            if (config == null)
                throw new ArgumentNullException(nameof(config));

            _keyboardSpeed = config.KeyboardSpeed;
            _axisThreshold = config.KeyboardAxisThreshold;
        }

        public Vector3? Update(float deltaTime)
        {
            if (deltaTime < 0)
                throw new ArgumentOutOfRangeException(nameof(deltaTime), deltaTime, "Value must be positive");

            float x = Input.GetAxisRaw(Inputs.KeyboardAxisX);
            float z = Input.GetAxisRaw(Inputs.KeyboardAxisY);

            if (Mathf.Abs(x) > _axisThreshold || Mathf.Abs(z) > _axisThreshold)
            {
                float zoomFactor = _cameraHandle.ProjectionSize;
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