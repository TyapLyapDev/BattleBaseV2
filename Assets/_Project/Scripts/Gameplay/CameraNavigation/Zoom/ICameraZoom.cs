using System;

namespace BattleBase.Gameplay.CameraNavigation
{
    public interface ICameraZoom
    {
        public event Action Changed;

        public float Value01 { get; }

        public void SetValue01(float value);

        public void Update(float? zoomDelta);
    }
}