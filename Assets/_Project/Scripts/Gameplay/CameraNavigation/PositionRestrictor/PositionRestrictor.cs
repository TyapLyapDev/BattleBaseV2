using System;
using BattleBase.Utils;
using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation
{
    public class PositionRestrictor : IPositionRestrictor
    {
        private readonly ICameraBoundsLimiter _boundsLimiter;

        public PositionRestrictor(ICameraBoundsLimiter boundsLimiter)
        {
            _boundsLimiter = boundsLimiter ?? throw new ArgumentNullException(nameof(boundsLimiter));
        }

        public Vector3 Restrict(Vector3 desiredPosition, Vector3 currentPosition)
        {
            if (VectorValidation.IsValid(desiredPosition) == false)
                throw new ArgumentException($"Desired position is invalid (NaN or Infinity): {desiredPosition}", nameof(desiredPosition));

            Vector3 result = currentPosition;

            if (_boundsLimiter.IsWithinBoundsX(desiredPosition))
                result.x = desiredPosition.x;

            if (_boundsLimiter.IsWithinBoundsZ(desiredPosition))
                result.z = desiredPosition.z;

            return result;
        }
    }
}