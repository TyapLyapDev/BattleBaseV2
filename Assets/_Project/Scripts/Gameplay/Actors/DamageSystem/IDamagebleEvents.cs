using System;

namespace BattleBase.Gameplay.Actors.DamageSystem
{
    public interface IDamagebleEvents
    {
        public event Action Destroyed;
    }
}