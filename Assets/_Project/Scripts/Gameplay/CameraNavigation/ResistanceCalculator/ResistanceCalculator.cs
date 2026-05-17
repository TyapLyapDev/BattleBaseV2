using System;
using BattleBase.Utils;
using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation
{
    public class ResistanceCalculator : IResistanceCalculator
    {
        private readonly ICameraBoundsLimiter _boundsLimiter;
        private readonly ICameraAreaService _cameraAreaService;

        public ResistanceCalculator(ICameraBoundsLimiter boundsLimiter, ICameraAreaService cameraAreaService)
        {
            _boundsLimiter = boundsLimiter ?? throw new ArgumentNullException(nameof(boundsLimiter));
            _cameraAreaService = cameraAreaService ?? throw new ArgumentNullException(nameof(cameraAreaService));
        }

        public Vector3 Calculate(Vector3 delta, Vector3 desiredPosition)
        {
            if (VectorValidation.IsValid(delta) == false)
                throw new ArgumentException($"Delta is invalid (NaN or Infinity): {delta}", nameof(delta));
            
            if (VectorValidation.IsValid(desiredPosition) == false)
                throw new ArgumentException($"Desired position is invalid (NaN or Infinity): {desiredPosition}", nameof(desiredPosition));

            float overshootX = _boundsLimiter.GetOvershootX(desiredPosition);
            float overshootZ = _boundsLimiter.GetOvershootZ(desiredPosition);
            float maxOvershoot = _cameraAreaService.ResistanceFadeDistance;
            float resistance = _cameraAreaService.Resistance;

            Vector3 result = delta;

            ApplyResistance(ref result.x, overshootX, maxOvershoot, resistance);
            ApplyResistance(ref result.z, overshootZ, maxOvershoot, resistance);

            return result;
        }

        private static void ApplyResistance(
            ref float component, 
            float overshoot, 
            float maxOvershoot, 
            float resistance)
        {
            if (overshoot > 0f)
            {
                float factor = 1f - Mathf.Clamp01(overshoot / maxOvershoot) * resistance;
                component *= factor;
            }
        }
    }
}