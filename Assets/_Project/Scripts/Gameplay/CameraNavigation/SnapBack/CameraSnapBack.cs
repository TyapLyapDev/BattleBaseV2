using System;
using BattleBase.Utils.Extensions;
using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation
{
    public class CameraSnapBack : ICameraSnapBack
    {
        private readonly IFrustumProjectionService _frustumProjectionService;
        private readonly ICameraArea _cameraArea;
        private readonly ICameraHandle _cameraHandle;

        public CameraSnapBack(
            IFrustumProjectionService frustumProjectionService,
            ICameraArea cameraArea,
            ICameraHandle cameraHandle)
        {
            _frustumProjectionService = frustumProjectionService ?? throw new ArgumentNullException(nameof(frustumProjectionService));
            _cameraArea = cameraArea ?? throw new ArgumentNullException(nameof(cameraArea));
            _cameraHandle = cameraHandle ?? throw new ArgumentNullException(nameof(cameraHandle));
        }

        public void ClampByOvershoot()
        {
            _frustumProjectionService.Refresh();
            Transform cameraRig = _cameraArea.CameraRig.transform;
            Vector3 position = cameraRig.position;

            Vector3 correction = GetCorrectionOvershootBounds(position);
            _cameraHandle.SetCameraRigPosition(position + correction);
        }

        public Vector3 GetCorrectionAreaBounds(Vector3 position) =>
            GetCorrectionBounds(_cameraArea.AreaBounds, position);

        public Vector3 GetCorrectionOvershootBounds(Vector3 position) =>
            GetCorrectionBounds(_cameraArea.OvershootBounds, position);

        public Vector3 GetCorrectionBounds(Bounds bounds, Vector3 position)
        {
            if (position.IsValid() == false)
                throw new ArgumentException($"Position is invalid (NaN or Infinity): {position}", nameof(position));

            GroundProjection projection = _frustumProjectionService.GetProjection(
                FrustumSizeType.MinimumWidthAndHeight, 
                FrustumShape.Rectangle);

            Vector3 leftUp = projection.LeftUp;
            Vector3 leftDown = projection.LeftDown;
            Vector3 rightUp = projection.RightUp;
            Vector3 rightDown = projection.RightDown;

            float minimumX = Mathf.Min(leftUp.x, leftDown.x, rightUp.x, rightDown.x);
            float maximumX = Mathf.Max(leftUp.x, leftDown.x, rightUp.x, rightDown.x);
            float minimumZ = Mathf.Min(leftUp.z, leftDown.z, rightUp.z, rightDown.z);
            float maximumZ = Mathf.Max(leftUp.z, leftDown.z, rightUp.z, rightDown.z);

            Vector3 correction = Vector3.zero;

            correction.x = CalculateCorrection(
                minimumX, maximumX,
                bounds.min.x, bounds.max.x,
                projection.Center.x,
                bounds.center.x);

            correction.z = CalculateCorrection(
                minimumZ, maximumZ,
                bounds.min.z, bounds.max.z,
                projection.Center.z,
                bounds.center.z);

            return correction;
        }

        private float CalculateCorrection(
            float minimum,
            float maximum,
            float boundMinimum,
            float boundMaximum,
            float frustumCenter,
            float boundCenter)
        {
            if (Mathf.Abs(minimum - maximum) > Mathf.Abs(boundMinimum - boundMaximum))
                return boundCenter - frustumCenter;

            if (minimum < boundMinimum)
                return boundMinimum - minimum;

            if (maximum > boundMaximum)
                return boundMaximum - maximum;

            return 0f;
        }
    }
}