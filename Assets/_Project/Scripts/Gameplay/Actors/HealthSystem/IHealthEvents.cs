using System;

namespace BattleBase.Gameplay.Actors.HealthSystem
{
    public interface IHealthEvents
    {
        public event Action<float, float> HealthChanged;
    }
}