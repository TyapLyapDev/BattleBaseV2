using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation
{
    public interface IProjectionSizeConfig
    {
        public Vector2 ReferenceValuePortraitOrientation { get; }

        public float MinimumOrthoSize { get; }

        public float MaximumOrthoSize { get; }

        public float MinimumFOV { get; }

        public float MaximumFOV { get; }

        public float LandscapeFovFactor { get; }
    }
}