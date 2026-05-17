namespace BattleBase.SaveService
{
    public interface IColorSaver
    {
        public IColorData ColorData { get; }

        public void SetColorData(IColorData data);
    }
}