namespace BattleBase.Gameplay.CameraNavigation
{
    public interface ICameraInertiaConfig
    {
        public float InertiaDamping { get; }

        public float InertiaExtraDampingFactor { get; }

        public float InertiaVelocityEpsilon { get; }

        public int InertiaSmoothingWindow { get; }
    }
}