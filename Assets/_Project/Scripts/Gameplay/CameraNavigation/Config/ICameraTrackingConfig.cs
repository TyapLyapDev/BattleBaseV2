namespace BattleBase.Gameplay.CameraNavigation
{
    public interface ICameraTrackingConfig
    {
        public float PositionSqrThreshold { get; }

        public float RotationAngleThreshold { get; }

        public float OrthoSizeThreshold { get; }
    }
}