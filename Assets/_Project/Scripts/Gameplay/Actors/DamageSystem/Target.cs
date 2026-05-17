using BattleBase.Gameplay.Actors.HealthSystem;
using System;
using UnityEngine;

namespace BattleBase.Gameplay.Actors.DamageSystem
{
    [RequireComponent(typeof(BoxCollider))]
    public class Target : MonoBehaviour, ITarget
    {
        [SerializeField] private Transform _aimingPoint;
        [SerializeField, Min(0.1f)] private float _hitDistance = 0.5f;

        [Header("Dubug")]
        [SerializeField] private bool _isDebugEnable = false;

        private IHealthPresenter _healthPresenter;
        private IDamagebleEvents _damagebleEvents;
        private ITeamable _teamable;

        public event Action Destroyed;

        private void OnValidate()
        {
            if (_aimingPoint == null)
                _aimingPoint = transform;
        }

        private void OnEnable()
        {
            if (_healthPresenter != null)
                _damagebleEvents.Destroyed += OnDied;
        }

        private void OnDisable()
        {
            if (_healthPresenter != null)
                _damagebleEvents.Destroyed -= OnDied;
        }

        private void OnDrawGizmosSelected()
        {
            if (_isDebugEnable == false)
                return;

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_aimingPoint.position, _hitDistance);
        }

        public void Init(IHealthPresenter healthPresenter, IDamagebleEvents damagebleEvents, ITeamable teamable)
        {
            _healthPresenter = healthPresenter ?? throw new ArgumentNullException(nameof(healthPresenter));
            _damagebleEvents = damagebleEvents ?? throw new ArgumentNullException(nameof(damagebleEvents));
            _teamable = teamable ?? throw new ArgumentNullException(nameof(teamable));

            if (gameObject.activeSelf)
                _damagebleEvents.Destroyed += OnDied;
        }

        public TeamType TeamType => _teamable.TeamType;

        public Vector3 Position => _aimingPoint.position;

        public bool HasHit(Vector3 hitPosition)
        {
            float sqrDistance = (_aimingPoint.position - hitPosition).sqrMagnitude;

            return sqrDistance < _hitDistance * _hitDistance;
        }

        public void TakeDamage(IDamage damage)
        {
            _healthPresenter.SendDamage(damage);
        }

        public void OnDied()
        {
            Destroyed?.Invoke();
        }
    }
}