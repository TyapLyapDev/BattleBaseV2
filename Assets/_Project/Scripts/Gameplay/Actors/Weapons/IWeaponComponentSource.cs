namespace BattleBase.Gameplay.Actors.Weapons
{
    public interface IWeaponComponentSource : IComponentSource
    {
        public IWeaponConfig Config { get; }
    }
}