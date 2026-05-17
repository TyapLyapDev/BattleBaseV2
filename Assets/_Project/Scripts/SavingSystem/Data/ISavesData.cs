namespace BattleBase.SaveService
{
    public interface ISavesData
    {
        public IVolumeData VolumeData { get; }

        public IColorData ColorData { get; }

        public ITerritoryData TerritoryData { get; }
    }
}