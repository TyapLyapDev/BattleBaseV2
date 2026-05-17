using BattleBase.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleBase.Gameplay.Actors
{
    [CreateAssetMenu(
        fileName = nameof(ActorConfig),
        menuName = Constants.ConfigsAssetMenuPath + nameof(ActorConfig) + "/" + nameof(ActorConfig))]
    public class ActorConfig : ScriptableObject
    {
        [SerializeField] private ActorView _prefab;
        [SerializeField] private ActorData _data;
        
        [SerializeField] private List<ActorComponentSource> _components;

        public ActorView Prefab => _prefab;

        public IActorData Data => _data;

        public IEnumerable<IComponentSource> GetComponentSources()
        {
            return _components
                .Where(c => c is IComponentSource)
                .Select(c => (IComponentSource)c)
                .ToList();
        }
    }
}
