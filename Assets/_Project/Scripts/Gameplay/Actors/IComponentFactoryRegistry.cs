namespace BattleBase.Gameplay.Actors
{
    public interface IComponentFactoryRegistry
    {
        public IActorComponent Create(IComponentSource componentSource);
    }
}
