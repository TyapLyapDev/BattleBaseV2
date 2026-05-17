using BattleBase.Core;
using BattleBase.Gameplay.Actors.DamageSystem;
using System;
using UnityEngine;

namespace BattleBase.Gameplay.Actors.Weapons.Missiles
{
    public abstract class Missile : MonoBehaviour, IMissile, IPoolable<Missile>
    {
        public string Id => name;

        public abstract event Action<Missile> Deactivated;

        public abstract void SetDamage(IDamage damage);

        public abstract void ShootTarget(Vector3 startPosition, ITarget target);
    }
}