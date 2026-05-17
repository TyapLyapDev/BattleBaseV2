namespace BattleBase.Gameplay.Actors.DamageSystem
{
    public interface IDamageConfig
    {
        public string MissleId { get; }

        public DamageMask DamageMask { get; }

        public float Damage { get; }
    }
}
