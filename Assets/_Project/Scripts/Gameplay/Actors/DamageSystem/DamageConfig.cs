using BattleBase.Gameplay.Actors.DamageSystem;
using BattleBase.Gameplay.Actors.Weapons.Missiles;
using BattleBase.Utils;
using UnityEngine;

namespace BattleBase.Gameplay.Actors
{
    [CreateAssetMenu(
    fileName = nameof(DamageConfig),
    menuName = Constants.ConfigsAssetMenuPath + nameof(ActorConfig) + "/" + nameof(DamageConfig))]
    public class DamageConfig : ScriptableObject, IDamageConfig
    {
        [SerializeField] private Missile _missilePrefab;

        [SerializeField] private DamageMask _damageMask;
        [SerializeField][Min(1f)] private float _damage = 20f;

        public string MissleId => _missilePrefab.name;

        public DamageMask DamageMask => _damageMask;

        public float Damage => _damage;
    }
}
