using System;

namespace BattleBase.Gameplay.CameraNavigation
{
    public interface IFrustumProjectionService
    {
        public event Action Changed;

        public FrustumProjection Projection { get; }

        public GroundProjection GetProjection(FrustumSizeType frustumSize, FrustumShape shape);

        public void Refresh();
    }
}