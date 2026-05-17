using BattleBase.Gameplay.Actors.DamageSystem;
using System;

namespace BattleBase.Gameplay.Actors.Weapons
{
    public interface IWeaponEvents
    {
        public event Action<ITarget> TargetSelected;

        public event Action<ITarget, IDamageConfig> TargetFired;

        public event Action Shooted;

        public event Action TargetReseted;
    }
}