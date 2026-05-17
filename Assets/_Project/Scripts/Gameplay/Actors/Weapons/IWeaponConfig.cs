using BattleBase.Gameplay.Actors.DamageSystem;

namespace BattleBase.Gameplay.Actors.Weapons
{
    public interface IWeaponConfig
    {
        public IDamageConfig DamageConfig { get; }

        public float FiringRange { get; }

        public float RateShooting { get; }

        public float SpeedReload { get; }

        public int NumberShells { get; }
    }
}