using System;
using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation
{
    public class DragApplier : IDragApplier
    {
        private readonly ICameraHandle _cameraHandle;
        private readonly IResistanceCalculator _resistanceCalculator;
        private readonly ICameraArea _cameraArea;

        public DragApplier(
            ICameraHandle cameraHandle, 
            IResistanceCalculator resistanceCalculator,
            ICameraArea cameraArea)
        {
            _cameraHandle = cameraHandle ?? throw new ArgumentNullException(nameof(cameraHandle));
            _resistanceCalculator = resistanceCalculator ?? throw new ArgumentNullException(nameof(resistanceCalculator));
            _cameraArea = cameraArea ?? throw new ArgumentNullException(nameof(cameraArea));
        }

        public void Apply(Vector3 worldDelta)
        {
            Transform rig = _cameraHandle.CameraRigTransform;
            Vector3 deltaGround = rig.right * worldDelta.x + rig.forward * worldDelta.z;
            deltaGround.y = 0;
            Vector3 desiredPosition = _cameraHandle.CameraRigPosition - deltaGround;
            Vector3 correctedDelta = _resistanceCalculator.Calculate(deltaGround, desiredPosition);

            _cameraHandle.SetCameraRigPosition(_cameraHandle.CameraRigPosition - correctedDelta);
        }

        public void RestorePosition(Vector3 currentPosition, Vector3 positionToRestore)
        {
            Vector3 worldDelta = currentPosition - positionToRestore;
            _cameraHandle.SetCameraRigPosition(_cameraHandle.CameraRigPosition - worldDelta);
        }
    }
}