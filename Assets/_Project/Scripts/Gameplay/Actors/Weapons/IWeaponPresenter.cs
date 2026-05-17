using BattleBase.Gameplay.Actors.DamageSystem;

namespace BattleBase.Gameplay.Actors.Weapons
{
    public interface IWeaponPresenter
    {
        void SpecifyTarget(ITarget enemy);

        void EstablishAimState(bool isAimed);
    }
}