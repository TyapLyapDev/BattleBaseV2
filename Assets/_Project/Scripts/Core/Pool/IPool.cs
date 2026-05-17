namespace BattleBase.Core
{
    public interface IPool<T>
    {
        public bool TryGive(out T element);
    }
}