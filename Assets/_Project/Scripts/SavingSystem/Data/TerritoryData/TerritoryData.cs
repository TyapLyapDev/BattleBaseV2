using System;
using System.Collections.Generic;
using UnityEngine;

namespace BattleBase.SaveService
{
    [Serializable]
    public class TerritoryData : ITerritoryData
    {
        [SerializeField] private List<int> _conqueredTerritories = new() { 0, };

        public TerritoryData() { }

        public TerritoryData(List<int> conqueredTerritories)
        {
            _conqueredTerritories = conqueredTerritories ?? throw new ArgumentNullException(nameof(conqueredTerritories));
        }

        public IReadOnlyList<int> ConqueredTerritories => _conqueredTerritories;

        public void SetData(ITerritoryData data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            _conqueredTerritories = new(data.ConqueredTerritories);
        }
    }
}