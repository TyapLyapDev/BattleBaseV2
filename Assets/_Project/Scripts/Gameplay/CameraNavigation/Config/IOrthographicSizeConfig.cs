using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation
{
    public interface IOrthographicSizeConfig
    {
        public Vector2 ReferenceValuePortraitOrientation { get; }

        public float MinimumOrthoSize { get; }

        public float MaximumOrthoSize { get; }
    }
}