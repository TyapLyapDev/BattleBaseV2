namespace BattleBase.Gameplay.Actors.Movement
{
    public interface IMoveComponentSource : IComponentSource
    {
        public IMoveConfig Config { get; }
    }
}
