using BattleBase.Gameplay.Actors.DamageSystem;
using System;

namespace BattleBase.Gameplay.Actors.HealthSystem
{
    public class HealthPresenter : IHealthPresenter
    {
        private readonly IHealth _model;

        public HealthPresenter(IHealth model)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
        }

        public void SendDamage(IDamage damage)
        {
            _model.TakeDamage(damage);
        }
    }
}