namespace BattleBase.Gameplay.Actors.Weapons
{
    public interface ITargetFinder : IActorViewComponent
    {
        public void Init(IWeaponPresenter presenter, IWeaponConfig weaponConfig, ITeamable teamable);
    }
}
