namespace BattleBase.Gameplay.Actors.DamageSystem
{
    public interface IDamage
    {
        public DamageMask DamageMask { get; }

        public float Value { get; }
    }
}