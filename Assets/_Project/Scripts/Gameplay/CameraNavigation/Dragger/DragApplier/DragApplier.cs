using System;
using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation
{
    public class DragApplier : IDragApplier
    {
        private readonly ICameraHandle _cameraHandle;
        private readonly IResistanceCalculator _resistanceCalculator;

        public DragApplier(
            ICameraHandle cameraHandle, 
            IResistanceCalculator resistanceCalculator)
        {
            _cameraHandle = cameraHandle ?? throw new ArgumentNullException(nameof(cameraHandle));
            _resistanceCalculator = resistanceCalculator ?? throw new ArgumentNullException(nameof(resistanceCalculator));
        }

        public void Apply(Vector3 worldDelta)
        {
            Transform rig = _cameraHandle.CameraRig.transform;

            Vector3 deltaGround = rig.right * worldDelta.x + rig.forward * worldDelta.z;
            deltaGround.y = 0;

            Vector3 desiredPosition = rig.position - deltaGround;
            Vector3 correctedDelta = _resistanceCalculator.Calculate(deltaGround, desiredPosition);

            rig.position -= correctedDelta;
        }
    }
}