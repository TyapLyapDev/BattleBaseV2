namespace BattleBase.SaveService
{
    public interface IChangeTrackable<T>
    {
        public bool IsChangedFrom(T other);
    }
}