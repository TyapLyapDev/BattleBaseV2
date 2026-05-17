namespace BattleBase.Gameplay.Actors
{
    public interface IActorComponentBinder
    {
        void Bind(IActor actor, IActorView view);
    }
}
