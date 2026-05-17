using BattleBase.Gameplay.Actors.DamageSystem;
using System;

namespace BattleBase.Gameplay.Actors.HealthSystem
{
    public class Health : IHealth
    {
        private readonly IHealthConfig _config;
        private readonly IDamageModifier _damageModifier;

        private float _currentHealth;

        public event Action<float, float> HealthChanged;
        public event Action Destroyed;

        public Health(IHealthConfig config, IDamageModifier damageModifier)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _damageModifier = damageModifier ?? throw new ArgumentNullException(nameof(damageModifier));
        }

        public bool IsAlive => _currentHealth > 0;

        public void Enable()
        {
            _currentHealth = _config.MaxHealth;

            HealthChanged?.Invoke(_config.MaxHealth, _currentHealth);
        }

        public void Disable()
        {
            _currentHealth = 0;
        }

        public void TakeDamage(IDamage damage)
        {
            if (damage == null)
                throw new ArgumentNullException(nameof(damage));

            float finalDamage = _damageModifier.CalculateDamage(damage, _config);

            if (finalDamage < 0)
                throw new InvalidOperationException($"{nameof(finalDamage)} cannot be less than 0");

            _currentHealth -= finalDamage;

            if (IsAlive)
            {
                HealthChanged?.Invoke(_config.MaxHealth, _currentHealth);
            }
            else
            {
                _currentHealth = 0;

                Destroyed?.Invoke();
            }
        }
    }
}