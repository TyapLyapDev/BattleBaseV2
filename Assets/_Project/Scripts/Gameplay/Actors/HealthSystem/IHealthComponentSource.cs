namespace BattleBase.Gameplay.Actors.HealthSystem
{
    public interface IHealthComponentSource : IComponentSource
    {
        public IHealthConfig Config { get; }
    }
}
