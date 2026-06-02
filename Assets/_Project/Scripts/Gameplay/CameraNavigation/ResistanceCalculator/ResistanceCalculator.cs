using System;
using BattleBase.Utils.Extensions;
using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation
{
    public class ResistanceCalculator : IResistanceCalculator
    {
        private readonly ICameraArea _cameraArea;
        private readonly IFrustumProjectionService _frustumService;

        public ResistanceCalculator(
            ICameraArea cameraArea,
            IFrustumProjectionService frustumService)
        {
            _cameraArea = cameraArea ?? throw new ArgumentNullException(nameof(cameraArea));
            _frustumService = frustumService ?? throw new ArgumentNullException(nameof(frustumService));
        }

        public float Resistance => _cameraArea.Resistance;

        public float GetMaximumOvershoot(ResistanceAxis axis)
        {
            Bounds area = _cameraArea.AreaBounds;
            Bounds overshoot = _cameraArea.OvershootBounds;

            return axis switch
            {
                ResistanceAxis.X => Mathf.Max(overshoot.max.x - area.max.x, area.min.x - overshoot.min.x),
                ResistanceAxis.Z => Mathf.Max(overshoot.max.z - area.max.z, area.min.z - overshoot.min.z),
                _ => throw new ArgumentOutOfRangeException(nameof(axis)),
            };
        }

        public float GetOvershoot(ResistanceAxis axis)
        {
            GroundProjection projection = _frustumService.GetProjection(FrustumSizeType.MinimumWidthAndHeight, FrustumShape.Rectangle);
            Bounds areaBounds = _cameraArea.AreaBounds;

            Vector3 leftUp = projection.LeftUp;
            Vector3 leftDown = projection.LeftDown;
            Vector3 rightUp = projection.RightUp;
            Vector3 rightDown = projection.RightDown;

            float minimum;
            float maximum;

            if (axis == ResistanceAxis.X)
            {
                minimum = Mathf.Min(leftUp.x, leftDown.x, rightUp.x, rightDown.x);
                maximum = Mathf.Max(leftUp.x, leftDown.x, rightUp.x, rightDown.x);

                if (minimum < areaBounds.min.x)
                    return areaBounds.min.x - minimum;

                if (maximum > areaBounds.max.x)
                    return areaBounds.max.x - maximum;
            }
            else if (axis == ResistanceAxis.Z)
            {
                minimum = Mathf.Min(leftUp.z, leftDown.z, rightUp.z, rightDown.z);
                maximum = Mathf.Max(leftUp.z, leftDown.z, rightUp.z, rightDown.z);

                if (minimum < areaBounds.min.z)
                    return areaBounds.min.z - minimum;

                if (maximum > areaBounds.max.z)
                    return areaBounds.max.z - maximum;
            }

            return 0f;
        }

        public Vector3 Calculate(Vector3 delta, Vector3 desiredPosition)
        {
            if (delta.IsValid() == false)
                throw new ArgumentException($"Delta is invalid: {delta}", nameof(delta));

            Vector3 result = delta;

            float overshootX = GetOvershoot(ResistanceAxis.X);
            float overshootZ = GetOvershoot(ResistanceAxis.Z);

            result.x *= GetDirectionalDampingFactor(overshootX, delta.x, ResistanceAxis.X);
            result.z *= GetDirectionalDampingFactor(overshootZ, delta.z, ResistanceAxis.Z);

            return result;
        }

        private float GetDirectionalDampingFactor(float overshoot, float delta, ResistanceAxis axis)
        {
            if (Mathf.Approximately(overshoot, 0f) || Mathf.Approximately(delta, 0f))
                return 1f;

            bool movingFurtherOut = Mathf.Sign(delta) == Mathf.Sign(overshoot);

            if (movingFurtherOut == false)
                return 1f;

            float maxOvershoot = GetMaximumOvershoot(axis);

            if (maxOvershoot <= 0f)
                return 1f;

            float absOvershoot = Mathf.Abs(overshoot);
            float t = Mathf.Clamp01(absOvershoot / maxOvershoot);
            float damping = 1f - t * Resistance;

            return Mathf.Clamp01(damping);
        }
    }
}