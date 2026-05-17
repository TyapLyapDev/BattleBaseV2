namespace BattleBase.Gameplay.CameraNavigation.InputReader
{
    public interface IDragConfig
    {
        public float KeyboardSpeed { get; }

        public float DragDeltaThreshold { get; }

        public float KeyboardAxisThreshold { get; }
    }
}