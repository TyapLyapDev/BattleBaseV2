using BattleBase.Gameplay.Actors.DamageSystem;

namespace BattleBase.Gameplay.Actors.Weapons
{
    public class WeaponPresenter : IWeaponPresenter
    {
        private readonly IWeapon _model;

        public WeaponPresenter(IWeapon model)
        {
            _model = model ?? throw new System.ArgumentNullException(nameof(model));
        }

        public void SpecifyTarget(ITarget enemy) => _model.SetTarget(enemy);

        public void EstablishAimState(bool isAimed)
        {
            if (isAimed)
                _model.Shoot();
            else
                _model.StopShoot();
        }
    }
}