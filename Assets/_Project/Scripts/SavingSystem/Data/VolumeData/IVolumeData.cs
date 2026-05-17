namespace BattleBase.SaveService
{
    public interface IVolumeData
    {
        public float GeneralVolume { get; }

        public float MusicVolume { get; }

        public float SfxVolume { get; }
    }
}