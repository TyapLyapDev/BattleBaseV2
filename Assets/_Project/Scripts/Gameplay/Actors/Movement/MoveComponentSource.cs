using BattleBase.Utils;
using UnityEngine;

namespace BattleBase.Gameplay.Actors.Movement
{
    [CreateAssetMenu(
        fileName = nameof(MoveComponentSource),
        menuName = Constants.ConfigsAssetMenuPath + nameof(ActorConfig) + "/" + nameof(MoveComponentSource))]
    public class MoveComponentSource : ActorComponentSource, IMoveComponentSource
    {
        [SerializeField] private MoveConfig _healthConfig;

        public IMoveConfig Config => _healthConfig;
    }
}
