namespace BattleBase.Core
{
    public interface IFactory<T>
    {
        public T Create();
    }
}