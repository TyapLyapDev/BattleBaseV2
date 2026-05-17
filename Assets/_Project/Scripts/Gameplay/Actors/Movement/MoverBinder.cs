using System;

namespace BattleBase.Gameplay.Actors.Movement
{
    public class MoverBinder : IActorComponentBinder
    {
        public void Bind(IActor actor, IActorView view)
        {
            if (actor == null)
                throw new ArgumentNullException(nameof(actor));

            if (view == null)
                throw new ArgumentNullException(nameof(view));

            if (actor.TryGetComponent(out IMover mover) &&
                view.TryGetViewComponent(out IMoverViewComponent moverView))
            {
                moverView.Init(mover);
            }
            else
            {
                return;
            }

            MoverPresenter presenter = new(mover);

            if (view.TryGetViewComponent(out INavigationAgent agent))
            {
                agent.Init(presenter, mover.Config, mover);
            }
        }
    }
}
