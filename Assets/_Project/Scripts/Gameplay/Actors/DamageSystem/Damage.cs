using System;

namespace BattleBase.Gameplay.Actors.DamageSystem
{
    public class Damage : IDamage
    {
        private readonly IDamageConfig _config;

        public Damage(IDamageConfig attributes)
        {
            _config = attributes ?? throw new ArgumentNullException(nameof(attributes));
        }

        public DamageMask DamageMask => _config.DamageMask;

        public float Value => _config.Damage;
    }
}