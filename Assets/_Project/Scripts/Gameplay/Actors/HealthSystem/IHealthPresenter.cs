using BattleBase.Gameplay.Actors.DamageSystem;

namespace BattleBase.Gameplay.Actors.HealthSystem
{
    public interface IHealthPresenter
    {
        public void SendDamage(IDamage damage);
    }
}