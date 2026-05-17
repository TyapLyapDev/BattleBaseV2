using System;
using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation
{
    [RequireComponent(typeof(BoxCollider))]
    public class CameraArea : MonoBehaviour, ICameraArea
    {
        [SerializeField] private BoxCollider _collider;
        [SerializeField][Min(0)] private float _resistanceFadeDistance = 0.5f;
        [SerializeField][Range(0f, 1f)] private float _resistance = 0.8f;
        [SerializeField] private bool _isStaticSizeCollider;
        [SerializeField] private CameraConfig _cameraConfig;

#if UNITY_EDITOR
        [SerializeField] private bool _shouldDrawGizmos;
#endif

        private Vector3 _cachedColliderSize;
        private Vector3 _cachedColliderCenter;
        private Vector3 _cachedLocalScale;

        public event Action Changed;

        public float Resistance => _resistance;

        public float ResistanceFadeDistance => _resistanceFadeDistance;

        public CameraConfig Config => _cameraConfig;

#if UNITY_EDITOR
        public bool ShouldDrawGizmos => _shouldDrawGizmos;
#endif

        public BoxCollider Collider
        {
            get
            {
                EnsureComponents();

                return _collider;
            }
        }

        private void OnValidate() =>
            InvokeChange();

        private void Start()
        {
            EnsureComponents();
            CacheColliderSize();
            InvokeChange();
        }

        private void Update()
        {
            if (_isStaticSizeCollider)
                return;

            if (_collider == null)
                return;

            if (_collider.size != _cachedColliderSize
                || _collider.center != _cachedColliderCenter
                || transform.localScale != _cachedLocalScale)
            {
                CacheColliderSize();
                InvokeChange();
            }
        }

        private void EnsureComponents()
        {
            if (_collider != null)
                return;

            _collider = GetComponent<BoxCollider>();

            if (_collider == null)
                throw new ArgumentNullException(nameof(_collider));
        }

        private void CacheColliderSize()
        {
            if (_collider != null)
            {
                _cachedColliderSize = _collider.size;
                _cachedColliderCenter = _collider.center;
                _cachedLocalScale = transform.localScale;
            }
        }

        private void InvokeChange() =>
            Changed?.Invoke();
    }
}