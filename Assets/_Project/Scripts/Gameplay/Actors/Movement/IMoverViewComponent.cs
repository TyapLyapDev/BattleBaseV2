namespace BattleBase.Gameplay.Actors.Movement
{
    public interface IMoverViewComponent : IActorViewComponent 
    {
        public void Init(IMoverEvents moverEvents);
    }
}
