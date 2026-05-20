using BattleBase.Utils.Constants;
using UnityEngine;

namespace BattleBase.Gameplay.Actors.Movement
{
    [CreateAssetMenu(
        fileName = nameof(MoveComponentSource),
        menuName = AssetMenuPaths.ScriptableObjects + nameof(ActorConfig) + "/" + nameof(MoveComponentSource))]
    public class MoveComponentSource : ActorComponentSource, IMoveComponentSource
    {
        [SerializeField] private MoveConfig _healthConfig;

        public IMoveConfig Config => _healthConfig;
    }
}
