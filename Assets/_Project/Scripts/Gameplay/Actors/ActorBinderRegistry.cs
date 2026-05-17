using System;
using System.Collections.Generic;

namespace BattleBase.Gameplay.Actors
{
    public class ActorBinderRegistry
    {
        private readonly HashSet<IActorComponentBinder> _builders;

        public ActorBinderRegistry(ICollection<IActorComponentBinder> builders)
        {
            if (builders == null)
                throw new ArgumentNullException(nameof(builders));

            if (builders.Count == 0)
                throw new ArgumentException($"{nameof(builders)} cannot be empty");

            _builders = new HashSet<IActorComponentBinder>(builders);
        }

        public void Bind(IActor actor, IActorView view)
        {
            foreach (var binder in _builders)
                binder.Bind(actor, view);
        }
    }
}
