using YG;

namespace BattleBase.SaveService
{
    public class YandexGameSaveSystemAdapter : ISaver
    {
        private bool _isDirty;

        public IVolumeData VolumeData => Data.VolumeData;

        public IColorData ColorData => Data.ColorData;

        public ITerritoryData TerritoryData => Data.TerritoryData;

        private SavesData Data => YG2.saves.SavesData;

        public void SaveProgress()
        {
            if (_isDirty)
            {
                YG2.SaveProgress();
                _isDirty = false;
            }
        }

        public void ResetProgress()
        {
            YG2.SetDefaultSaves();
            _isDirty = true;
            SaveProgress();
        }

        public void SetVolumeData(IVolumeData data)
        {
            if (Data.VolumeData.IsChangedFrom(data))
            {
                Data.SetVolumeData(data);
                _isDirty = true;
            }
        }

        public void SetColorData(IColorData data)
        {
            if (Data.ColorData.IsChangedFrom(data))
            {
                Data.SetColorData(data);
                _isDirty = true;
            }
        }

        public void SetTerritoryData(ITerritoryData data)
        {
            if (Data.TerritoryData.IsChangedFrom(data))
            {
                Data.SetTerritoryData(data);
                _isDirty = true;
            }
        }
    }
}