using BattleBase.Gameplay.Actors.DamageSystem;
using UnityEngine;

namespace BattleBase.Gameplay.Actors.Weapons.Missiles
{
    public interface IMissile
    {
        public void SetDamage(IDamage damage);

        public void ShootTarget(Vector3 startPosition, ITarget target);
    }
}