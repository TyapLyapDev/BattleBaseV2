namespace BattleBase.Gameplay.Actors.HealthSystem
{
    public interface IHealthViewComponent : IActorViewComponent
    {
        public void Init(IHealthEvents healthEvents);
    }
}