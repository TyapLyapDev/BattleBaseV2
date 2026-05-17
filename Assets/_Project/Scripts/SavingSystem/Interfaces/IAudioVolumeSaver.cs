namespace BattleBase.SaveService
{
    public interface IAudioVolumeSaver
    {
        public IVolumeData VolumeData { get; }

        public void SetVolumeData(IVolumeData data);
    }
}