using System;
using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation
{
    public class DragApplier : IDragApplier
    {
        private readonly Transform _cameraRig;
        private readonly IResistanceCalculator _resistance;
        private readonly IPositionRestrictor _restrictor;

        public DragApplier(CameraRig cameraRig, IResistanceCalculator resistance, IPositionRestrictor restrictor)
        {
            if (cameraRig == null)
                throw new ArgumentNullException(nameof(cameraRig));

            _cameraRig = cameraRig.transform;
            _resistance = resistance ?? throw new ArgumentNullException(nameof(resistance));
            _restrictor = restrictor ?? throw new ArgumentNullException(nameof(restrictor));
        }

        public void Apply(Vector3 worldDelta)
        {
            Vector3 deltaGround = _cameraRig.right * worldDelta.x + _cameraRig.forward * worldDelta.z;
            deltaGround.y = 0;
            Vector3 desiredPos = _cameraRig.position - deltaGround;
            Vector3 correctedDelta = _resistance.Calculate(deltaGround, desiredPos);
            Vector3 finalPos = _cameraRig.position - correctedDelta;
            _cameraRig.position = _restrictor.Restrict(finalPos, _cameraRig.position);
        }
    }
}