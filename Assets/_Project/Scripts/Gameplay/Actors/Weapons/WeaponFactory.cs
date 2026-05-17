using System;

namespace BattleBase.Gameplay.Actors.Weapons
{
    public class WeaponFactory : IComponentFactory
    {
        public Type SourceType => typeof(WeaponComponentSource);

        public IActorComponent Create(IComponentSource source)
        {
            if (source is IWeaponComponentSource weaponSource == false)
                throw new ArgumentException(
                    $"{nameof(source)} 'source' does not implement {nameof(IWeaponComponentSource)}");

            return new Weapon(weaponSource.Config);
        }
    }
}
