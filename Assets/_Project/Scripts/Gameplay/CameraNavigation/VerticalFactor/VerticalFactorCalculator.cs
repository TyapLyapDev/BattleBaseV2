using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation
{
    public class VerticalFactorCalculator : IVerticalFactorCalculator
    {
        private const float DefaultFactor = 1f;
        private const float SinEpsilon = 0.001f;

        private readonly Camera _camera;

        public VerticalFactorCalculator(Camera camera)
        {
            _camera = camera != null ? camera : throw new System.ArgumentNullException(nameof(camera));
        }

        public float CalculateVerticalFactor()
        {
            float angleX = Mathf.Abs(_camera.transform.eulerAngles.x);
            float sin = Mathf.Sin(angleX * Mathf.Deg2Rad);

            return sin > SinEpsilon ? 1f / sin : DefaultFactor;
        }
    }
}