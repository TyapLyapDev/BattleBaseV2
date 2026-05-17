namespace BattleBase.Gameplay.Actors.DamageSystem
{
    public interface IDamageble : IDamagebleEvents
    {
        public void TakeDamage(IDamage damage);
    }
}