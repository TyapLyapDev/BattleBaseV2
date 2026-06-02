using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation
{
    public class VerticalFactorCalculator : IVerticalFactorCalculator
    {
        private const float DefaultFactor = 1f;
        private const float SinEpsilon = 0.001f;

        private readonly ICameraHandle _cameraHandle;
        private readonly IProjectionSizeConfig _config;

        public VerticalFactorCalculator(ICameraHandle cameraHandle, IProjectionSizeConfig config)
        {
            _cameraHandle = cameraHandle ?? throw new System.ArgumentNullException(nameof(cameraHandle));
            _config = config ?? throw new System.ArgumentNullException(nameof(config));
        }

        public float CalculateVerticalFactor()
        {
            if (_cameraHandle.ProjectionType == CameraProjectionType.Orthographic)
            {
                float angleX = Mathf.Abs(_cameraHandle.Camera.transform.eulerAngles.x);
                float sin = Mathf.Sin(angleX * Mathf.Deg2Rad);

                return sin > SinEpsilon ? 1f / sin : DefaultFactor;
            }
            else
            {
                return _config.LandscapeFovFactor;
            }
        }
    }
}