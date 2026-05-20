using BattleBase.Utils.Constants;
using UnityEngine;

namespace BattleBase.Gameplay.Actors.HealthSystem
{
    [CreateAssetMenu(
        fileName = nameof(HealthComponentSource),
        menuName = AssetMenuPaths.ScriptableObjects + nameof(ActorConfig) + "/" + nameof(HealthComponentSource))]
    public class HealthComponentSource : ActorComponentSource, IHealthComponentSource
    {
        [SerializeField] private HealthConfig _healthConfig;

        public IHealthConfig Config => _healthConfig;
    }
}
