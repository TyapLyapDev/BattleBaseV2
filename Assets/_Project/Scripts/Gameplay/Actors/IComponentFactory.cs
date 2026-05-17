using System;

namespace BattleBase.Gameplay.Actors
{
    public interface IComponentFactory
    {
        public Type SourceType { get; }

        public IActorComponent Create(IComponentSource source);
    }
}
