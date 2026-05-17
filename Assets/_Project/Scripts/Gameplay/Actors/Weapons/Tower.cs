using BattleBase.Gameplay.Actors.DamageSystem;
using System;
using UnityEngine;

namespace BattleBase.Gameplay.Actors.Weapons
{
    public class Tower : MonoBehaviour, ITower
    {
        private const float DotAim = 0.99f;

        [SerializeField] private Muzzle _muzzle;
        [SerializeField][Min(1f)] private float _speedRotate = 25f;

        private IWeaponPresenter _presenter;
        private IWeaponEvents _weaponEvents;
        private ITargetPoint _currentTarget;
        private Transform _transform;
        private bool _isAimed;

        private void Awake()
        {
            _transform = transform;
        }

        private void OnEnable()
        {
            if (_weaponEvents != null)
            {
                _weaponEvents.TargetSelected += OnTakeAim;
                _weaponEvents.TargetReseted += OnRemoveTarget;
            }
        }

        private void Update()
        {
            if (_currentTarget != null)
                LookAtTarget();
        }

        private void OnDisable()
        {
            OnRemoveTarget();

            if (_weaponEvents != null)
            {
                _weaponEvents.TargetSelected -= OnTakeAim;
                _weaponEvents.TargetReseted -= OnRemoveTarget;
            }
        }

        public void Init(IWeaponPresenter presenter, IWeaponEvents weaponEvents)
        {
            _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
            _weaponEvents = weaponEvents ?? throw new ArgumentNullException(nameof(weaponEvents));

            if (gameObject.activeSelf)
            {
                _weaponEvents.TargetSelected += OnTakeAim;
                _weaponEvents.TargetReseted += OnRemoveTarget;
            }
        }

        private void LookAtTarget()
        {
            Vector3 direction = _currentTarget.Position - _transform.position;
            direction.y = 0f;

            if (direction.sqrMagnitude < Muzzle.MinDistance)
                return;

            Quaternion targetRotation = Quaternion.LookRotation(direction);

            _transform.rotation = Quaternion.RotateTowards(
                _transform.rotation,
                targetRotation,
                _speedRotate * Time.deltaTime
            );

            Vector3 localTargetPosition = _transform.InverseTransformPoint(_currentTarget.Position);
            _muzzle.LookAtTarget(localTargetPosition);

            float dot = Vector3.Dot(_transform.forward, direction.normalized);

            if (_isAimed != dot > DotAim)
            {
                _isAimed = dot > DotAim;
                _presenter.EstablishAimState(_isAimed);
            }
        }

        private void OnTakeAim(ITargetPoint target)
        {
            _currentTarget = target ?? throw new ArgumentNullException(nameof(target));
            _isAimed = false;
        }

        private void OnRemoveTarget()
        {
            _currentTarget = null;
        }
    }
}