using BattleBase.Gameplay.Actors.DamageSystem;

namespace BattleBase.Gameplay.Actors.HealthSystem
{
    public interface IDamageModifier
    {
        public float CalculateDamage(IDamage damage, IHealthConfig healthConfig);
    }
}