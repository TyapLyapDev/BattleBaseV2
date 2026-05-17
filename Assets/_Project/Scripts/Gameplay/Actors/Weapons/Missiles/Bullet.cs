using BattleBase.Gameplay.Actors.DamageSystem;
using System;
using UnityEngine;

namespace BattleBase.Gameplay.Actors.Weapons.Missiles
{
    [RequireComponent(typeof(LineMover))]
    public class Bullet : Missile
    {
        private IMissileMover _mover;
        private IDamage _damage;
        private ITarget _target;

        public override event Action<Missile> Deactivated;

        private void Awake()
        {
            _mover = GetComponent<IMissileMover>();
        }

        private void FixedUpdate()
        {
            if (_target == null)
                return;

            _mover.Move(Time.fixedDeltaTime);

            if (HasHit())
                return;

            if (_mover.IsFinished)
                Deactivate();
        }

        public override void SetDamage(IDamage damage)
        {
            _damage = damage ?? throw new ArgumentNullException(nameof(damage));
        }

        public override void ShootTarget(Vector3 startPosition, ITarget target)
        {
            if (_target != null)
                return;

            _target = target ?? throw new ArgumentNullException(nameof(target));
            _mover.SetStartPosition(startPosition);
            _mover.SetPointPosition(_target.Position);
        }

        private bool HasHit()
        {
            if (_target.HasHit(_mover.CurrentPosition) == false)
                return false;

            _target.TakeDamage(_damage);

            Deactivate();

            return true;
        }

        private void Deactivate()
        {
            _target = null;

            Deactivated?.Invoke(this);
        }
    }
}