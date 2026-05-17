namespace BattleBase.Gameplay.Actors.Weapons
{
    public interface ITower : IActorViewComponent
    {
        public void Init(IWeaponPresenter presenter, IWeaponEvents weaponEvents);
    }
}