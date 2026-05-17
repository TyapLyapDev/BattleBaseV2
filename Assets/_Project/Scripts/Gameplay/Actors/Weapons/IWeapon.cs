using BattleBase.Gameplay.Actors.DamageSystem;

namespace BattleBase.Gameplay.Actors.Weapons
{
    public interface IWeapon : IActorComponent, IUpdateable, IWeaponEvents
    {
        public IWeaponConfig Config { get; }

        public void SetTarget(ITarget target);

        public void Shoot();

        public void StopShoot();
    }
}