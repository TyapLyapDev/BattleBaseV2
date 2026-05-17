namespace BattleBase.Gameplay.Actors.Movement
{
    public interface INavigationAgent : IActorViewComponent 
    {
        public void Init(IMoverPresenter presenter, IMoveConfig config, IMoverEvents moverEvents);
    }
}
