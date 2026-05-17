namespace BattleBase.Gameplay.Actors.Weapons
{
    public interface IWeaponViewComponent : IActorViewComponent
    {
        public void Init(IWeaponEvents weaponEvents);
    }
}