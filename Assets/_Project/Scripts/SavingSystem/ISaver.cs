namespace BattleBase.SaveService
{
    public interface ISaver : IAudioVolumeSaver, IColorSaver, ITerritorySaver
    {
        public void SaveProgress();

        public void ResetProgress();
    }
}