using System;

namespace BattleBase.Gameplay.Actors.Movement
{
    public class MoverFactory : IComponentFactory
    {
        public Type SourceType => typeof(MoveComponentSource);

        public IActorComponent Create(IComponentSource source)
        {
            if (source is IMoveComponentSource moveSource == false)
                throw new ArgumentException(
                    $"{nameof(source)} 'source' does not implement {nameof(IMoveComponentSource)}");

            return new Mover(moveSource.Config);
        }
    }
}
