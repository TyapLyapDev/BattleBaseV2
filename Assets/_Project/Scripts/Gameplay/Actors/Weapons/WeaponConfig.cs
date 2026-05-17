using BattleBase.Gameplay.Actors.DamageSystem;
using UnityEngine;

namespace BattleBase.Gameplay.Actors.Weapons
{
    [System.Serializable]
    public class WeaponConfig : IWeaponConfig
    {
        [SerializeField] private DamageConfig _damageConfig;
        [SerializeField][Min(5f)] private float _firingRange = 20f;
        [SerializeField][Min(0.1f)] private float _rateShooting = 0.5f;
        [SerializeField][Min(1f)] private float _speedReload = 2f;
        [SerializeField][Min(1)] private int _numberShells = 1;

        public IDamageConfig DamageConfig => _damageConfig;

        public float RateShooting => _rateShooting;

        public float SpeedReload => _speedReload;

        public int NumberShells => _numberShells;

        public float FiringRange => _firingRange;
    }
}