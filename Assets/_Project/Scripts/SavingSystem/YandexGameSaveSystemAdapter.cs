using YG;

namespace BattleBase.SaveService
{
    public class YandexGameSaveSystemAdapter : ISaver
    {
        public IVolumeData VolumeData => Data.VolumeData;

        public IColorData ColorData => Data.ColorData;

        public ITerritoryData TerritoryData => Data.TerritoryData;

        private SavesData Data => YG2.saves.SavesData;

        public void SaveProgress() =>
            YG2.SaveProgress();

        public void ResetProgress()
        {
            YG2.SetDefaultSaves();
            SaveProgress();
        }

        public void SetVolumeData(IVolumeData data) =>
            Data.SetVolumeData(data);

        public void SetColorData(IColorData data) =>
            Data.SetColorData(data);

        public void SetTerritoryData(ITerritoryData data) =>
            Data.SetTerritoryData(data);
    }
}