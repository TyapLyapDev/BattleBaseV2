using BattleBase.Utils;
using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation
{
    [CreateAssetMenu(
        fileName = nameof(CameraConfig),
        menuName = Constants.ConfigsAssetMenuPath + nameof(CameraConfig))]
    public class CameraConfig : ScriptableObject, ICameraInertiaConfig, ICameraSnapBackConfig, IOrthographicSizeConfig, ICameraTrackingConfig
    {
        [Header("Snap back")]
        [SerializeField][Min(0f)] private float _snapBackSpeed = 3;

        [Header("Orthographic size")]
        [SerializeField][Min(0f)] private Vector2 _referenceValuePortraitOrientation = new(1080, 1920);
        [SerializeField][Min(0f)] private float _minimumOrthoSize = 0.3f;
        [SerializeField][Min(0f)] private float _maximumOrthoSize = 1.2f;

        [Header("Inertia")]
        [SerializeField][Min(0f)] private float _inertiaDamping = 5f;
        [SerializeField][Min(0f)] private float _inertiaExtraDampingFactor = 100f;
        [SerializeField][Min(0f)] private float _inertiaVelocityEpsilon = 0.01f;
        [SerializeField][Min(1)] private int _inertiaSmoothingWindow = 4;

        [Header("Tracking")]
        [SerializeField][Min(0f)] private float _positionSqrThreshold = 0.0001f;
        [SerializeField][Min(0f)] private float _rotationAngleThreshold = 0.01f;
        [SerializeField][Min(0f)] private float _orthoSizeThreshold = 0.0001f;

        public float SnapBackSpeed => _snapBackSpeed;

        public float MinimumOrthoSize => _minimumOrthoSize;

        public float MaximumOrthoSize => _maximumOrthoSize;

        public float InertiaDamping => _inertiaDamping;

        public float InertiaExtraDampingFactor => _inertiaExtraDampingFactor;

        public float PositionSqrThreshold => _positionSqrThreshold;

        public float RotationAngleThreshold => _rotationAngleThreshold;

        public float OrthoSizeThreshold => _orthoSizeThreshold;

        public float InertiaVelocityEpsilon => _inertiaVelocityEpsilon;

        public int InertiaSmoothingWindow => _inertiaSmoothingWindow;

        public Vector2 ReferenceValuePortraitOrientation => _referenceValuePortraitOrientation;
    }
}