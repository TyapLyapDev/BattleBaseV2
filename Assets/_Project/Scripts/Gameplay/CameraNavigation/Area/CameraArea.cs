using System;
using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation
{
    [RequireComponent(typeof(BoxCollider))]
    public class CameraArea : MonoBehaviour, ICameraArea
    {
#if UNITY_EDITOR
        [SerializeField] private bool _shouldDrawGizmos;
#endif

        [SerializeField] private bool _isStaticSizeCollider;
        [SerializeField][Range(0f, 1f)] private float _resistance = 0.8f;
        [SerializeField] private BoxCollider _area;
        [SerializeField] private BoxCollider _overshoot;

        [field: SerializeField] public CameraConfig Config { get; private set; }

        [field: SerializeField] public CameraRig CameraRig { get; private set; }

        private Vector3 _cachedColliderSize;
        private Vector3 _cachedColliderCenter;
        private Vector3 _cachedLocalScale;

        public event Action Changed;

        public Bounds AreaBounds => _area.bounds;

        public Bounds OvershootBounds => _overshoot.bounds;

        public Plane GroundPlane => new(-transform.up, GroundPlaneY);

        public float Resistance => _resistance;

        public float GroundPlaneY => AreaBounds.center.y;

#if UNITY_EDITOR
        public bool ShouldDrawGizmos => _shouldDrawGizmos;
#endif

        private void Awake()
        {
            if (_area != null)
                UpdateCachedColliderProperties();
        }

        private void Update()
        {
            if (_isStaticSizeCollider)
                return;

            if (_area == null)
                return;

            if (_area.size != _cachedColliderSize
                || _area.center != _cachedColliderCenter
                || transform.localScale != _cachedLocalScale)
            {
                UpdateCachedColliderProperties();
            }
        }

        public GroundProjection GetAreaGroundProjection(ScreenOrientationType orientationType) =>
            AreaBounds.GetGroundProjection(CameraRig.transform.forward, CameraRig.transform.right);

        public GroundProjection GetOvershootGroundProjection(ScreenOrientationType orientationType) =>
            OvershootBounds.GetGroundProjection(CameraRig.transform.forward, CameraRig.transform.right);

        private void UpdateCachedColliderProperties()
        {
            _cachedColliderSize = _area.size;
            _cachedColliderCenter = _area.center;
            _cachedLocalScale = transform.localScale;

            Changed?.Invoke();
        }
    }
}