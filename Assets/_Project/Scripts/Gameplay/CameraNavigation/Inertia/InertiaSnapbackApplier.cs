using System;
using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation
{
    public class InertiaSnapbackApplier : IInertiaSnapbackApplier
    {
        private readonly ICameraHandle _cameraHandle;
        private readonly ICameraSnapBack _snapBack;
        private readonly IResistanceCalculator _resistanceCalculator;
        private readonly AxisInertiaHandler _inertiaRight;
        private readonly AxisInertiaHandler _inertiaForward;
        private readonly float _speed;

        public InertiaSnapbackApplier(
            ICameraHandle cameraHandle,
            ICameraSnapBack snapBack,
            IResistanceCalculator resistanceCalculator,
            ICameraInertiaConfig config,
            ICameraSnapBackConfig snapBackConfig)
        {
            _cameraHandle = cameraHandle ?? throw new ArgumentNullException(nameof(cameraHandle));

            _snapBack = snapBack ?? throw new ArgumentNullException(nameof(snapBack));
            _resistanceCalculator = resistanceCalculator ?? throw new ArgumentNullException(nameof(resistanceCalculator));
            _inertiaRight = new(config);
            _inertiaForward = new(config);
            _speed = snapBackConfig.SnapBackSpeed;
        }

        public void ResetInertia()
        {
            _inertiaRight.ResetInertia();
            _inertiaForward.ResetInertia();
        }

        public void UpdateInertia(Vector3 worldDragDelta, float deltaTime)
        {
            if (deltaTime < 0)
                throw new ArgumentOutOfRangeException(nameof(deltaTime), deltaTime, "Value mast be positive");

            _inertiaRight.AddDelta(worldDragDelta.x, deltaTime);
            _inertiaForward.AddDelta(worldDragDelta.z, deltaTime);
        }

        public void Apply(float deltaTime)
        {
            if (deltaTime < 0)
                throw new ArgumentOutOfRangeException(nameof(deltaTime), deltaTime, "Value mast be positive");

            float rightShift = ComputeRightAxis(deltaTime);
            float forwardShift = ComputeForwardAxis(deltaTime);

            Transform rig = _cameraHandle.CameraRigTransform;

            Vector3 moveDelta = rig.right * rightShift + rig.forward * forwardShift;
            Vector3 newPosition = _cameraHandle.CameraRigPosition - moveDelta;
            newPosition.y = 0;

            _cameraHandle.SetCameraRigPosition(newPosition);
        }

        private float ComputeRightAxis(float deltaTime) =>
            ComputeAxisShift(_inertiaRight, deltaTime, _cameraHandle.CameraRigTransform.right);

        private float ComputeForwardAxis(float deltaTime) =>
            ComputeAxisShift(_inertiaForward, deltaTime, _cameraHandle.CameraRigTransform.forward);

        private float ComputeAxisShift(
            AxisInertiaHandler inertia,
            float deltaTime,
            Vector3 axisDirection)
        {
            if (inertia.TryGetVelocity(deltaTime, out float velocity))
            {
                float deltaMove = velocity * deltaTime;

                bool isXAxis = Mathf.Abs(Vector3.Dot(axisDirection, Vector3.right)) >
                               Mathf.Abs(Vector3.Dot(axisDirection, Vector3.forward));

                float overshoot = isXAxis
                    ? _resistanceCalculator.GetOvershoot(ResistanceAxis.X)
                    : _resistanceCalculator.GetOvershoot(ResistanceAxis.Z);

                overshoot = Mathf.Abs(overshoot);

                if (overshoot > 0f)
                {
                    float maxOvershoot = isXAxis
                        ? _resistanceCalculator.GetMaximumOvershoot(ResistanceAxis.X)
                        : _resistanceCalculator.GetMaximumOvershoot(ResistanceAxis.Z);

                    float resistance = _resistanceCalculator.Resistance;
                    float factor = 1f - Mathf.Clamp01(overshoot / maxOvershoot) * resistance;

                    inertia.DampenVelocity(deltaTime, factor);

                    if (inertia.TryGetVelocity(deltaTime, out velocity))
                        deltaMove = velocity * deltaTime;
                    else
                        deltaMove = 0f;
                }

                return deltaMove;
            }

            Vector3 worldCorrection = _snapBack.GetCorrectionAreaBounds(_cameraHandle.CameraRigPosition);
            float snapbackShift = -Vector3.Dot(worldCorrection, axisDirection);
            float maximumSnapback = _speed * deltaTime;
            snapbackShift = Mathf.Clamp(snapbackShift, -maximumSnapback, maximumSnapback);

            return snapbackShift;
        }
    }
}