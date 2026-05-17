using System;

namespace BattleBase.Gameplay.Actors.Weapons
{
    public class WeaponBinder : IActorComponentBinder
    {
        public void Bind(IActor actor, IActorView view)
        {
            if (actor == null)
                throw new ArgumentNullException(nameof(actor));

            if (view == null)
                throw new ArgumentNullException(nameof(view));

            if (actor.TryGetComponent(out IWeapon weapon) &&
                view.TryGetViewComponent(out IWeaponViewComponent weaponView))
            {
                weaponView.Init(weapon);
            }
            else
            {
                return;
            }

            WeaponPresenter presenter = new(weapon);

            if (view.TryGetViewComponent(out ITower tower))
            {
                tower.Init(presenter, weapon);
            }

            if (view.TryGetViewComponent(out ITargetFinder targetFinder))
            {
                targetFinder.Init(presenter, weapon.Config, actor.Data);
            }
        }
    }
}