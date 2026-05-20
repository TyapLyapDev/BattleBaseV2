using BattleBase.Utils.Constants;
using UnityEngine;

namespace BattleBase.Gameplay.Actors.Weapons
{
    [CreateAssetMenu(
    fileName = nameof(WeaponComponentSource),
    menuName = AssetMenuPaths.ScriptableObjects + nameof(ActorConfig) + "/" + nameof(WeaponComponentSource))]
    public class WeaponComponentSource : ActorComponentSource, IWeaponComponentSource
    {
        [SerializeField] private WeaponConfig _weaponConfig;

        public IWeaponConfig Config => _weaponConfig;
    }
}