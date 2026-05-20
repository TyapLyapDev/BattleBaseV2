namespace BattleBase.SaveService
{
    public interface IColorData : IChangeTrackable<IColorData>
    {
        public int PlayerColorIndex { get; }

        public int EnemyColorIndex { get; }
    }
}