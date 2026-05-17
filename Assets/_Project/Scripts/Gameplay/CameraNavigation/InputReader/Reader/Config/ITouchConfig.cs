namespace BattleBase.Gameplay.CameraNavigation.InputReader
{
    public interface ITouchConfig
    {
        float ZoomSensitivity { get; }

        float MinPinchDistance { get; }

        float DragDeltaThreshold { get; }
    }
}