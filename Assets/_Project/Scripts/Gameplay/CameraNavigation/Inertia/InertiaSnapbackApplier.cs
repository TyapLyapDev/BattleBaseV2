using System;
using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation
{
    public class InertiaSnapbackApplier : IInertiaSnapbackApplier
    {
        private readonly Transform _cameraRig;
        private readonly ICameraSnapBack _snapBack;
        private readonly ICameraBoundsLimiter _boundsLimiter;
        private readonly IPositionRestrictor _restrictor;
        private readonly ICameraAreaService _cameraAreaService;

        private readonly AxisInertiaHandler _inertiaRight;
        private readonly AxisInertiaHandler _inertiaForward;

        public InertiaSnapbackApplier(
            CameraRig cameraRig,
            ICameraSnapBack snapBack,
            ICameraBoundsLimiter boundsLimiter,
            IPositionRestrictor restrictor,
            ICameraAreaService cameraAreaService,
            ICameraInertiaConfig config)
        {
            if (cameraRig == null)
                throw new ArgumentNullException(nameof(cameraRig));

            _cameraRig = cameraRig.transform;
            _snapBack = snapBack ?? throw new ArgumentNullException(nameof(snapBack));
            _boundsLimiter = boundsLimiter ?? throw new ArgumentNullException(nameof(boundsLimiter));
            _restrictor = restrictor ?? throw new ArgumentNullException(nameof(restrictor));
            _cameraAreaService = cameraAreaService ?? throw new ArgumentNullException(nameof(cameraAreaService));

            _inertiaRight = new(config);
            _inertiaForward = new(config);
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

            Vector3 moveDelta = _cameraRig.right * rightShift + _cameraRig.forward * forwardShift;
            Vector3 newPosition = _cameraRig.position - moveDelta;
            newPosition = _restrictor.Restrict(newPosition, _cameraRig.position);
            _cameraRig.position = newPosition;
            _snapBack.ClampByOvershoot();
        }

        private float ComputeRightAxis(float deltaTime) =>
            ComputeAxisShift(_inertiaRight, deltaTime, _cameraRig.right);

        private float ComputeForwardAxis(float deltaTime) =>
            ComputeAxisShift(_inertiaForward, deltaTime, _cameraRig.forward);

        private float ComputeAxisShift(
            AxisInertiaHandler inertia,
            float deltaTime,
            Vector3 axisDirection)
        {
            float resistance = _cameraAreaService.Resistance;
            float maxOvershoot = _cameraAreaService.ResistanceFadeDistance;

            bool isXAxis = Mathf.Abs(Vector3.Dot(axisDirection, Vector3.right)) >
                           Mathf.Abs(Vector3.Dot(axisDirection, Vector3.forward));

            if (inertia.TryGetVelocity(deltaTime, out float velocity))
            {
                float deltaMove = velocity * deltaTime;
                Vector3 desiredPosition = _cameraRig.position + axisDirection * deltaMove;

                float overshoot = isXAxis
                    ? _boundsLimiter.GetOvershootX(desiredPosition)
                    : _boundsLimiter.GetOvershootZ(desiredPosition);

                if (overshoot > 0)
                {
                    float factor = 1f - Mathf.Clamp01(overshoot / maxOvershoot) * resistance;
                    inertia.DampenVelocity(deltaTime, factor);

                    if (inertia.TryGetVelocity(deltaTime, out velocity))
                        deltaMove = velocity * deltaTime;
                    else
                        deltaMove = 0f;
                }

                return deltaMove;
            }

            Vector3 worldCorrection = _snapBack.GetCorrectionAreaBounds(_cameraRig.position);
            float snapbackShift = -Vector3.Dot(worldCorrection, axisDirection);
            float maximumSnapback = _snapBack.Speed * deltaTime;
            snapbackShift = Mathf.Clamp(snapbackShift, -maximumSnapback, maximumSnapback);

            return snapbackShift;
        }
    }
}