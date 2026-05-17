using System;
using UnityEngine;

namespace BattleBase.SaveService
{
    [Serializable]
    public class SavesData : ISavesData
    {
        [SerializeField] private VolumeData _volumeData = new();
        [SerializeField] private ColorData _colorData = new();
        [SerializeField] private TerritoryData _territoryData = new();

        public IVolumeData VolumeData => _volumeData;

        public IColorData ColorData => _colorData;

        public ITerritoryData TerritoryData => _territoryData;

        public void SetVolumeData(IVolumeData data) =>
            _volumeData.SetData(data);

        public void SetColorData(IColorData data) =>
            _colorData.SetData(data);

        public void SetTerritoryData(ITerritoryData data) =>
            _territoryData.SetData(data);
    }
}