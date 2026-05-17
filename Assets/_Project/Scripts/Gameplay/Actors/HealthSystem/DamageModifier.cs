using BattleBase.Gameplay.Actors.DamageSystem;
using System;

namespace BattleBase.Gameplay.Actors.HealthSystem
{
    public class DamageModifier : IDamageModifier
    {
        public float CalculateDamage(IDamage damage, IHealthConfig healthConfig)
        {
            if (damage == null)
                throw new ArgumentNullException(nameof(damage));

            if (healthConfig == null)
                throw new ArgumentNullException(nameof(healthConfig));

            float coefficientDamage = 1 - healthConfig.ArmorCoefficient;

            return damage.Value * coefficientDamage;
        }
    }
}