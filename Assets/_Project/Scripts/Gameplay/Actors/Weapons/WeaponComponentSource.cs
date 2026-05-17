using BattleBase.Utils;
using UnityEngine;

namespace BattleBase.Gameplay.Actors.Weapons
{
    [CreateAssetMenu(
    fileName = nameof(WeaponComponentSource),
    menuName = Constants.ConfigsAssetMenuPath + nameof(ActorConfig) + "/" + nameof(WeaponComponentSource))]
    public class WeaponComponentSource : ActorComponentSource, IWeaponComponentSource
    {
        [SerializeField] private WeaponConfig _weaponConfig;

        public IWeaponConfig Config => _weaponConfig;
    }
}