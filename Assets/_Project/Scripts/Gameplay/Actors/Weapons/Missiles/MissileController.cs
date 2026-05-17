using BattleBase.Gameplay.Actors.DamageSystem;
using BattleBase.Gameplay.Actors.Spawn;
using System;
using UnityEngine;

namespace BattleBase.Gameplay.Actors.Weapons.Missiles
{
    public class MissileController : MonoBehaviour, IWeaponViewComponent
    {
        [SerializeField] private Transform _shotPoint;

        private IWeaponEvents _weaponEvents;
        private IMissileSpawner _spawner;

        private void OnEnable()
        {
            if (_weaponEvents != null)
                _weaponEvents.TargetFired += OnShootMissile;
        }

        private void OnDisable()
        {
            if (_weaponEvents != null)
                _weaponEvents.TargetFired -= OnShootMissile;
        }

        public void Init(IWeaponEvents weaponEvents)
        {
            _weaponEvents = weaponEvents ?? throw new ArgumentNullException(nameof(weaponEvents));

            if (gameObject.activeSelf)
                _weaponEvents.TargetFired += OnShootMissile;

            //todo change for Inject
            _spawner = FindAnyObjectByType<MissileSpawner>();
        }

        private void OnShootMissile(ITarget target, IDamageConfig damageConfig)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            if (damageConfig == null)
                throw new ArgumentNullException(nameof(damageConfig));

            IMissile missile = _spawner.Spawn(damageConfig.MissleId);
            Damage damage = new(damageConfig);
            missile.SetDamage(damage);
            missile.ShootTarget(_shotPoint.position, target);
        }
    }
}